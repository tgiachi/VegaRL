using System.Collections;
using GoRogue.Random;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Config.WorldMap;
using Vega.Framework.Data.Entities.Names;
using Vega.Framework.Data.Entities.Terrain;
using Vega.Framework.Map;
using Vega.Framework.Map.GameObjects.World;
using Vega.Framework.Noise;
using Vega.Framework.Noise.AccidentalNoise.Enums;
using Vega.Framework.Noise.AccidentalNoise.Implicit;
using Vega.Framework.Utils;
using Vega.Framework.Utils.Random;

namespace Vega.Engine.Services;

[VegaService(10)]
public class WorldService : BaseVegaService<WorldService>, IWorldService
{
    private readonly ITileService _tileService;
    private readonly INameService _nameService;
    public Point WorldMapSize { get; } = new(180, 180);
    public Point GridMapSize { get; } = new(250, 250);
    public WorldMap CurrentWorldMap { get; set; }
    public GridMap CurrentGridMap { get; set; }

    public WorldService(
        ILogger<WorldService> logger, IMessageBusService messageBusService, ITileService tileService,
        INameService nameService
    ) : base(
        logger,
        messageBusService
    )
    {
        _tileService = tileService;
        _nameService = nameService;
    }

    public override Task<bool> LoadAsync()
    {
        GenerateWorldMapAsync(new WorldMapConfig());

        return Task.FromResult(true);
    }

    public async Task<WorldMap> GenerateWorldMapAsync(WorldMapConfig config)
    {
        Logger.LogInformation("Generating world map...");
        var worldMap = new WorldMap(WorldMapSize.X, WorldMapSize.Y)
        {
            Name = _nameService.RandomName(NameTypeEnum.World)
        };
        await GenerateNoiseMap(worldMap, config);
        await PlaceRivers(worldMap, config);
        await PlaceCities(worldMap, config);

        Logger.LogInformation("World map generated");
        return worldMap;
    }


    private async Task<WorldMap> GenerateNoiseMap(WorldMap worldMap, WorldMapConfig config)
    {
        Logger.LogInformation("Generating noise map type {Type}...", config.NoiseType);

        if (config.NoiseType != WorldMapNoiseType.AccidentalNoise)
        {
            var values = config.NoiseType == WorldMapNoiseType.Perlin
                ? await GeneratePerlinNoiseMap()
                : await GenerateFastNoiseMap();

            var mappedValues = await GetWorldMapTerrains();

            await FillOverMapAsync(worldMap, mappedValues, values);

            var zones = FindZones(worldMap);
        }
        else
        {
            var result = await GenerateAccidentalNoiseMap();
            var tiles = await GetWorldMapTerrains();
            NormalizeAccidentalNoise(ref result.mapData, result.noise);
            FillWorldMap(worldMap, tiles, result.mapData);
            UpdateNeighbors(worldMap);
            UpdateBitmasks(worldMap);
            var floodFill = await FloodFill(worldMap);
        }


        return worldMap;
    }

    private Task<MapData> GenerateHeatMap(WorldMap worldMap, int heatOctaves = 4, double heatFrequency = 3.0)
    {
        var heatmap = new MapData(WorldMapSize.X, WorldMapSize.Y);
        var gradient = new ImplicitGradient(1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1);
        var heatFractal = new ImplicitFractal(
            FractalType.Multi,
            BasisType.Simplex,
            InterpolationType.Quintic,
            heatOctaves,
            heatFrequency,
            GlobalRandom.DefaultRNG.NextInt(1, Int32.MaxValue)
        );

        var heatMap = new ImplicitCombiner(CombinerType.Multiply);
        heatMap.AddSource(gradient);
        heatMap.AddSource(heatFractal);

        for (int x = 0; x < WorldMapSize.X; x++)
        {
            for (int y = 0; y < WorldMapSize.Y; y++)
            {
                var tile = worldMap.GetTerrainAt<TerrainWorldGameObject>(new Point(x, y));
                if (tile.HeightType == "GRASS")
                {
                    heatmap.Data[tile.Position.X, tile.Position.Y] -= 0.1f * tile.HeightValue;
                }
                else if (tile.HeightType == "FOREST")
                {
                    heatmap.Data[tile.Position.X, tile.Position.Y] -= 0.2f * tile.HeightValue;
                }
                else if (tile.HeightType == "MOUNTAIN")
                {
                    heatmap.Data[tile.Position.X, tile.Position.Y] += 0.2f * tile.HeightValue;
                }
                else if (tile.HeightType == "SNOW")
                {
                    heatmap.Data[tile.Position.X, tile.Position.Y] += 0.3f * tile.HeightValue;
                }
            }
        }

        return Task.FromResult(heatmap);
    }

    private Task<List<TerrainGroupObject>> FloodFill(WorldMap worldMap)
    {
        var tileGroups = new List<TerrainGroupObject>();
        var tileStack = new Stack<TerrainWorldGameObject>();
        for (int x = 0; x < WorldMapSize.X; x++)
        {
            for (int y = 0; y < WorldMapSize.Y; y++)
            {
                var tile = worldMap.GetTerrainAt<TerrainWorldGameObject>(new Point(x, y));
                if (tile.FloodFilled)
                {
                    continue;
                }

                if (tile.IsWalkable)
                {
                    var group = new TerrainGroupObject()
                    {
                        TileType = tile.HeightType
                    };
                    tileStack.Push(tile);
                    while (tileStack.Count > 0)
                    {
                        FloodFill(worldMap, tileStack.Pop(), ref group, ref tileStack);
                    }

                    if (group.Tiles.Count > 0)
                    {
                        tileGroups.Add(group);
                    }
                }
                else
                {
                    var group = new TerrainGroupObject()
                    {
                        TileType = tile.HeightType
                    };
                    tileStack.Push(tile);

                    while (tileStack.Count > 0)
                    {
                        FloodFill(worldMap, tileStack.Pop(), ref group, ref tileStack);
                    }

                    if (group.Tiles.Count > 0)
                    {
                        tileGroups.Add(group);
                    }
                }
            }
        }

        return Task.FromResult(tileGroups);
    }

    private void FloodFill(
        WorldMap map,
        TerrainWorldGameObject tile, ref TerrainGroupObject tiles, ref Stack<TerrainWorldGameObject> stack
    )
    {
        // Validate
        if (tile.FloodFilled)
            return;
        if ((tiles.TileType.Contains("SAND") || tiles.TileType.Contains("GRASS") || tiles.TileType.Contains("FOREST") ||
             tiles.TileType.Contains("SNOW")) && !tile.IsWalkable)
            return;
        if ((tiles.TileType.Contains("DEEP_WATER") || tiles.TileType.Contains("WATER")) && tile.IsWalkable)
            return;

        // Add to TileGroup
        tiles.Tiles.Add(tile);
        tile.FloodFilled = true;

        // floodfill into neighbors
        var t = map.GetTop(tile, WorldMapSize);
        if (!t.FloodFilled && tile.IsWalkable == t.IsWalkable)
            stack.Push(t);
        t = map.GetBottom(tile, WorldMapSize);
        if (!t.FloodFilled && tile.IsWalkable == t.IsWalkable)
            stack.Push(t);
        t = map.GetLeft(tile, WorldMapSize);
        if (!t.FloodFilled && tile.IsWalkable == t.IsWalkable)
            stack.Push(t);
        t = map.GetRight(tile, WorldMapSize);
        if (!t.FloodFilled && tile.IsWalkable == t.IsWalkable)
            stack.Push(t);
    }


    private Task<WorldMap> PlaceRivers(WorldMap worldMap, WorldMapConfig config)
    {
        Logger.LogInformation("Placing rivers...");
        Logger.LogInformation("Rivers placed");
        return Task.FromResult(worldMap);
    }

    private Task<WorldMap> PlaceCities(WorldMap worldMap, WorldMapConfig config)
    {
        Logger.LogInformation("Placing cities...");
        Logger.LogInformation("Cities placed");
        return Task.FromResult(worldMap);
    }

    private Task<float[,]> GeneratePerlinNoiseMap()
    {
        var perlinNoise = new PerlinNoise(Random.Shared);
        var overMapResults = new float[WorldMapSize.X, WorldMapSize.Y];
        for (var i = 0; i < WorldMapSize.X; i++)
        {
            for (var j = 0; j < WorldMapSize.Y; j++)
            {
                var x = (float)j / WorldMapSize.X;
                var y = (float)i / WorldMapSize.Y;
                overMapResults[i, j] = (float)perlinNoise.Noise(x * 10, y * 10, 0.8f);
            }
        }

        Logger.LogInformation("Perlin noise map generated");
        return Task.FromResult(overMapResults);
    }

    private Task<(MapData mapData, ImplicitFractal noise)> GenerateAccidentalNoiseMap(
        int terrainOctaves = 6, double terrainFrequency = 1.25
    )
    {
        var mapData = new MapData(WorldMapSize.X, WorldMapSize.Y);

        var implicitNoise = new ImplicitFractal(
            FractalType.Multi,
            BasisType.Simplex,
            InterpolationType.Quintic,
            terrainOctaves,
            terrainFrequency,
            GlobalRandom.DefaultRNG.NextInt(1, Int32.MaxValue)
        );
        for (var x = 0; x < WorldMapSize.X; x++)
        {
            for (var y = 0; y < WorldMapSize.Y; y++)
            {
                //Sample the noise at smaller intervals
                float x1 = x / (float)WorldMapSize.X;
                float y1 = y / (float)WorldMapSize.Y;

                float value = (float)implicitNoise.Get(x1, y1);

                //keep track of the max and min values found
                if (value > mapData.Max) mapData.Max = value;
                if (value < mapData.Min) mapData.Min = value;

                mapData.Data[x, y] = value;
            }
        }


        Logger.LogInformation("Accidental noise map generated");
        return Task.FromResult((mapData, implicitNoise));
    }

    private void NormalizeAccidentalNoise(ref MapData mapData, ImplicitFractal implicitFractal)
    {
        mapData = new MapData(WorldMapSize.X, WorldMapSize.Y);

        for (var x = 0; x < WorldMapSize.X; x++)
        {
            for (var y = 0; y < WorldMapSize.Y; y++)
            {
                // Noise range
                float x1 = 0, x2 = 2;
                float y1 = 0, y2 = 2;
                float dx = x2 - x1;
                float dy = y2 - y1;

                // Sample noise at smaller intervals
                float s = x / (float)WorldMapSize.X;
                float t = y / (float)WorldMapSize.Y;

                // Calculate our 4D coordinates
                float nx = (float)(x1 + Math.Cos(s * 2 * Math.PI) * dx / (2 * Math.PI));
                float ny = (float)(y1 + Math.Cos(t * 2 * Math.PI) * dy / (2 * Math.PI));
                float nz = (float)(x1 + Math.Sin(s * 2 * Math.PI) * dx / (2 * Math.PI));
                float nw = (float)(y1 + Math.Sin(t * 2 * Math.PI) * dy / (2 * Math.PI));

                float heightValue = (float)implicitFractal.Get(nx, ny, nz, nw);

                // keep track of the max and min values found
                if (heightValue > mapData.Max) mapData.Max = heightValue;
                if (heightValue < mapData.Min) mapData.Min = heightValue;

                mapData.Data[x, y] = heightValue;
            }
        }
    }


    private Task<float[,]> GenerateFastNoiseMap()
    {
        Logger.LogInformation("Fast noise map generated");
        var noise = new FastNoiseLite(GlobalRandom.DefaultRNG.NextInt(1, Int32.MaxValue));
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        var overMapResults = new float[WorldMapSize.X, WorldMapSize.Y];

        for (var i = 0; i < WorldMapSize.X; i++)
        {
            for (var j = 0; j < WorldMapSize.Y; j++)
            {
                var x = (float)j / WorldMapSize.X;
                var y = (float)i / WorldMapSize.Y;
                overMapResults[i, j] = (float)noise.GetNoise(x * 10, y * 10, 0.8f);
            }
        }

        return Task.FromResult(overMapResults);
    }

    private Task<SortedDictionary<double, TerrainEntity>> GetWorldMapTerrains()
    {
        var terrains = new SortedDictionary<double, TerrainEntity>();
        var values = _tileService.FindTerrainByFlags("WORLDMAP")
            .SelectMany(s => s.Flags.Where(f => f.Contains("VAL")).Select(f => new { Tile = s, Flag = f }))
            .ToList();
        foreach (var value in values)
        {
            terrains.Add(double.Parse(value.Flag.Replace("VAL=", "")) / 10, value.Tile);
        }

        terrains.Values.ToList().ForEach(s => s.SanitizeWorldFlags());

        return Task.FromResult(terrains);
    }

    public IEnumerable<Zone> FindZones(WorldMap map, int count = 5, int radius = 6)
    {
        var zoneFinder = new ZoneFinder(map);
        var zones = new List<Zone>();
        foreach (var _ in Enumerable.Range(0, count))
        {
            Zone zone = null;

            while (zone == null)
            {
                zone = zoneFinder.FindZone(map.WalkabilityView.Positions().RandomElement(), radius);
            }


            zones.Add(zone);
        }

        return zones;
    }

    private async Task UpdateBitmasks(WorldMap map)
    {
        for (var x = 0; x < WorldMapSize.X; x++)
        {
            for (var y = 0; y < WorldMapSize.Y; y++)
            {
                map.GetTerrainAt<TerrainWorldGameObject>(new Point(x, y)).UpdateBitmask();
            }
        }
    }

    private async Task FillWorldMap(WorldMap worldMap, SortedDictionary<double, TerrainEntity> perlinLimits, MapData mapData)
    {
        for (var x = 0; x < WorldMapSize.X; x++)
        {
            for (var y = 0; y < WorldMapSize.Y; y++)
            {
                float value = mapData.Data[x, y];

                //normalize our value between 0 and 1
                value = (value - mapData.Min) / (mapData.Max - mapData.Min);

                foreach (var perlinValue in perlinLimits)
                {
                    if (value <= perlinValue.Key)
                    {
                        var tile = _tileService.GetGlyphFromHasTileEntity(perlinValue.Value);
                        var t = new TerrainWorldGameObject(
                            new Point(x, y),
                            tile,
                            perlinValue.Value.IsWalkable,
                            perlinValue.Value.IsTransparent,
                            perlinValue.Value.Flags
                        );


                        t.HeightValue = value;
                        worldMap.SetTerrain(t);
                        break;
                    }
                }
            }
        }
    }

    private Task UpdateNeighbors(WorldMap worldMap)
    {
        for (var x = 0; x < WorldMapSize.X; x++)
        {
            for (var y = 0; y < WorldMapSize.Y; y++)
            {
                var terrain = worldMap.GetTerrainAt<TerrainWorldGameObject>(new Point(x, y));

                terrain.Top = worldMap.GetTop(terrain, WorldMapSize);
                terrain.Bottom = worldMap.GetBottom(terrain, WorldMapSize);
                terrain.Left = worldMap.GetLeft(terrain, WorldMapSize);
                terrain.Right = worldMap.GetRight(terrain, WorldMapSize);
            }
        }


        return Task.CompletedTask;
    }

    private async Task FillOverMapAsync(WorldMap overMap, SortedDictionary<double, TerrainEntity> perlinLimits, float[,] map)
    {
        for (var x = 0; x < WorldMapSize.X; x++)
        {
            for (int y = 0; y < WorldMapSize.Y; y++)
            {
                foreach (var perlinValue in perlinLimits)
                {
                    if (map[x, y] <= perlinValue.Key)
                    {
                        var tile = _tileService.GetGlyphFromHasTileEntity(perlinValue.Value);
                        overMap.SetTerrain(
                            new TerrainWorldGameObject(
                                new Point(x, y),
                                tile,
                                perlinValue.Value.IsWalkable,
                                perlinValue.Value.IsTransparent,
                                perlinValue.Value.Flags
                            )
                        );
                        break;
                    }
                }
            }
        }
    }
}

﻿using System.Diagnostics;
using System.Numerics;
using GoRogue.GameFramework;
using GoRogue.Random;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Config.WorldMap;
using Vega.Framework.Data.Entities.Names;
using Vega.Framework.Data.Entities.Terrain;
using Vega.Framework.Map;
using Vega.Framework.Map.WorldMap;
using Vega.Framework.Map.WorldMap.Biomes;
using Vega.Framework.Map.WorldMap.GameObjects;
using Vega.Framework.Map.WorldMap.HeatMap;
using Vega.Framework.Map.WorldMap.Rivers;
using Vega.Framework.Noise;
using Vega.Framework.Noise.AccidentalNoise.Enums;
using Vega.Framework.Noise.AccidentalNoise.Implicit;
using Vega.Framework.Utils;
using Vega.Framework.Utils.Random;

namespace Vega.Engine.Services;

using TerrainZonesDict = Dictionary<string, List<TerrainGroupObject>>;

[VegaService(10)]
public class WorldService : BaseVegaService<WorldService>, IWorldService
{
    private readonly ITileService _tileService;
    private readonly INameService _nameService;
    private readonly IMapSpawnerService _mapSpawnerService;
    public Point WorldMapSize { get; } = new(180, 180);
    public Point GridMapSize { get; } = new(250, 250);
    public WorldMap CurrentWorldMap { get; set; }
    public GridMap CurrentGridMap { get; set; }

    public WorldService(
        ILogger<WorldService> logger, IMessageBusService messageBusService, ITileService tileService,
        INameService nameService, IMapSpawnerService mapSpawnerService
    ) : base(
        logger,
        messageBusService
    )
    {
        _tileService = tileService;
        _nameService = nameService;
        _mapSpawnerService = mapSpawnerService;
    }

    public override async Task<bool> LoadAsync()
    {
        while (CurrentWorldMap == null)
        {
            try
            {
                CurrentWorldMap = await GenerateWorldMapAsync(new WorldMapConfig());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error generating world map");
            }
        }


        return true;
    }

    public async Task<WorldMap> GenerateWorldMapAsync(WorldMapConfig config)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.LogInformation("Generating world map...");
            var worldMap = new WorldMap(WorldMapSize.X, WorldMapSize.Y)
            {
                Name = _nameService.RandomName(NameTypeEnum.World)
            };
            var result = await GenerateNoiseMap(worldMap, config);
            await PlaceRivers(worldMap, config);
            var lands = await PlaceLands(worldMap, config, result.Item2);
            if (!lands.Any())
            {
                return await GenerateWorldMapAsync(config);
            }

            await PlaceRoads(worldMap, lands);

            if (!lands.SelectMany(s => s.LandGameObjects).Any())
            {
                return await GenerateWorldMapAsync(config);
            }

            // await PlacePlayer(worldMap, lands.SelectMany(s => s.LandGameObjects));

            stopwatch.Stop();
            Logger.LogInformation("World map generated in {Ms} ms", stopwatch.ElapsedMilliseconds);
            return worldMap;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error generating world map");
            throw;
        }
    }

    private Task PlaceRoads(Map map, IEnumerable<LandZoneObject> zones)
    {
        return Task.CompletedTask;
    }


    private async Task<(WorldMap, TerrainZonesDict)> GenerateNoiseMap(
        WorldMap worldMap, WorldMapConfig config
    )
    {
        Logger.LogInformation("Generating noise");


        var result = await GenerateAccidentalNoiseMap();
        var tiles = await GetWorldMapTerrains();
        NormalizeAccidentalNoise(ref result.mapData, result.noise);
        await FillWorldMap(worldMap, tiles, result.mapData);
        await UpdateNeighbors(worldMap);
        await UpdateBitmasks(worldMap);
        var floodFill = await FloodFill(worldMap);
        var zonesData = new TerrainZonesDict();
        foreach (var flood in floodFill)
        {
            if (!zonesData.ContainsKey(flood.TileType))
            {
                zonesData.Add(flood.TileType, new List<TerrainGroupObject>());
            }

            zonesData[flood.TileType].Add(flood);
        }


        await GenerateHeatMap(worldMap);
        var moistureData = await GenerateMoistureMap(worldMap);
        await PlaceRivers(worldMap, config);
        await AdjustMoistureMap(worldMap, moistureData);
        await GenerateBiomeMap(worldMap);
        await GenerateClouds(worldMap);


        return (worldMap, zonesData);
    }

    private static Task GenerateBiomeMap(Map map)
    {
        for (var x = 0; x < map.Width; x++)
        {
            for (var y = 0; y < map.Height; y++)
            {
                var tile = map.GetTerrainAt<TerrainWorldGameObject>(new Point(x, y));
                if (!tile.IsWalkable)
                {
                    continue;
                }

                tile.BiomeType = BiomeUtils.GetBiomeType(tile);
            }
        }

        return Task.CompletedTask;
    }

    private Task<MapData> GenerateClouds(
        Map map, float cloudCutOff = 0.55f, int cloudOctaves = 5, float cloudFrequency = 1.65f
    )
    {
        var cloudMapData = new MapData(WorldMapSize.X, WorldMapSize.Y);
        var cloudMap = new ImplicitFractal(
            FractalType.Billow,
            BasisType.Simplex,
            InterpolationType.Quintic,
            cloudOctaves,
            cloudFrequency,
            GlobalRandom.DefaultRNG.NextInt(0, int.MaxValue)
        );
        var cloudTile = _tileService.FindTerrainByFlags("CLOUD").FirstOrDefault();
        var coloredTile = _tileService.GetGlyphFromHasTileEntity(cloudTile);
        coloredTile.Background = new Color(
            coloredTile.Background.R,
            coloredTile.Background.G,
            coloredTile.Background.B,
            0.5f
        );
        coloredTile.Foreground = new Color(
            coloredTile.Foreground.R,
            coloredTile.Foreground.G,
            coloredTile.Foreground.B,
            0.5f
        );

        for (int x = 0; x < map.Width; x++)
        {
            for (var y = 0; y < map.Height; y++)
            {
                var val = (float)cloudMap.Get(x, y);
                cloudMapData.AddData(x, y, val);
            }
        }

        for (int x = 0; x < map.Width; x++)
        {
            for (var y = 0; y < map.Height; y++)
            {
                var value = cloudMapData.GetData(x, y);
                value = (value - cloudMapData.Min) / (cloudMapData.Max - cloudMapData.Min);
                if (value > cloudCutOff)
                {
                    map.AddEntity(new CloudWorldGameObject(new Point(x, y), coloredTile));
                }
            }
        }

        return Task.FromResult(cloudMapData);
    }

    private Task<MapData> GenerateMoistureMap(WorldMap worldMap, int moistureOctaves = 4, double moistureFrequency = 3.0)
    {
        var moistureData = new MapData(WorldMapSize.X, WorldMapSize.Y);
        var gradient = new ImplicitGradient(1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1);
        var moistureFractal = new ImplicitFractal(
            FractalType.Multi,
            BasisType.Simplex,
            InterpolationType.Quintic,
            moistureOctaves,
            moistureFrequency,
            GlobalRandom.DefaultRNG.NextInt(1, Int32.MaxValue)
        );

        var moistureGenerator = new ImplicitCombiner(CombinerType.Multiply);
        moistureGenerator.AddSource(gradient);
        moistureGenerator.AddSource(moistureFractal);

        for (int x = 0; x < WorldMapSize.X; x++)
        {
            for (int y = 0; y < WorldMapSize.Y; y++)
            {
                var tile = worldMap.GetTerrainAt<TerrainWorldGameObject>(new Point(x, y));
                if (tile.HeightType == "DEEP_WATER")
                {
                    moistureData.Data[tile.Position.X, tile.Position.Y] += 8f * tile.HeightValue;
                }
                else if (tile.HeightType == "WATER")
                {
                    moistureData.Data[tile.Position.X, tile.Position.Y] += 3f * tile.HeightValue;
                }
                else if (tile.HeightType == "SAND")
                {
                    moistureData.Data[tile.Position.X, tile.Position.Y] += 1f * tile.HeightValue;
                }

                tile.MoistureType = MoistureUtils.GetMoistureType(moistureData.GetData(tile.Position.X, tile.Position.Y));
            }
        }

        return Task.FromResult(moistureData);
    }

    private Task<MapData> GenerateHeatMap(Map worldMap, int heatOctaves = 4, double heatFrequency = 3.0)
    {
        var heatmap = new MapData(WorldMapSize.X, WorldMapSize.Y);
        var gradient = new ImplicitGradient(1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1);
        var heatFractal = new ImplicitFractal(
            FractalType.Multi,
            BasisType.Simplex,
            InterpolationType.Quintic,
            heatOctaves,
            heatFrequency,
            GlobalRandom.DefaultRNG.NextInt(1, int.MaxValue)
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

                tile.HeatType = HeatMapUtils.ValueToHeat(heatmap.GetData(tile.Position.X, tile.Position.Y));
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
        var t = map.GetTop(tile);
        if (!t.FloodFilled && tile.IsWalkable == t.IsWalkable)
        {
            stack.Push(t);
        }

        t = map.GetBottom(tile);
        if (!t.FloodFilled && tile.IsWalkable == t.IsWalkable)
        {
            stack.Push(t);
        }

        t = map.GetLeft(tile);
        if (!t.FloodFilled && tile.IsWalkable == t.IsWalkable)
        {
            stack.Push(t);
        }

        t = map.GetRight(tile);
        if (!t.FloodFilled && tile.IsWalkable == t.IsWalkable)
        {
            stack.Push(t);
        }
    }


    private async Task<WorldMap> PlaceRivers(WorldMap worldMap, WorldMapConfig config)
    {
        Logger.LogInformation("Placing rivers...");

        var attempts = 0;

        //var maxAttempts = config.MaxRiverAttempts;
        var rivers = new List<River>();

        var riverCount = config.NumRivers.Range.RandomRange();
        while (riverCount > 0 && attempts < config.MaxRiverAttempts)
        {
            var tile = worldMap.GetTerrainAt<TerrainWorldGameObject>(worldMap.GetRandomPoint());
            if (!tile.IsWalkable)
            {
                continue;
            }

            if (tile.Rivers.Count > 0)
            {
                continue;
            }

            if (tile.HeightValue > config.MinRiverHeight)
            {
                var river = new River(riverCount)
                {
                    CurrentDirection = tile.GetLowestNeighbor()
                };
                FindPathToWater(worldMap, tile, river.CurrentDirection, ref river);
                if (river.TurnCount < config.MinRiverTurns || river.Tiles.Count < config.MinRiverLength ||
                    river.Intersections > config.MaxRiverIntersections)
                {
                    for (int i = 0; i < river.Tiles.Count; i++)
                    {
                        var t = river.Tiles[i];
                        t.Rivers.Remove(river);
                    }
                }
                else if (river.Tiles.Count >= config.MinRiverLength)
                {
                    //Validation passed - Add river to list
                    rivers.Add(river);
                    tile.Rivers.Add(river);
                    riverCount--;
                }
            }

            attempts++;
        }

        var riverGroups = await BuildRiverGroups(worldMap);
        var riverTile = _tileService.FindTerrainByFlags("RIVER").FirstOrDefault();
        var riverAppearance = _tileService.GetTile(riverTile);

        await DigRiverGroups(riverGroups);
        foreach (var riverGroup in riverGroups)
        {
            foreach (var river in riverGroup.Rivers)
            {
                foreach (var tile in river.Tiles)
                {
                    tile.Appearance.Background = riverAppearance.coloredGlyph.Background;
                    tile.Appearance.Foreground = riverAppearance.coloredGlyph.Foreground;
                    tile.Appearance.Glyph = riverAppearance.coloredGlyph.Glyph;
                }
            }
        }

        Logger.LogInformation("Rivers placed");
        return worldMap;
    }

    private Task AdjustMoistureMap(WorldMap map, MapData moistureData)
    {
        for (var x = 0; x < map.Width; x++)
        {
            for (var y = 0; y < map.Height; y++)
            {
                var t = map.GetTerrainAt<TerrainWorldGameObject>(new Point(x, y));
                if (t.HeightType == "RIVER")
                {
                    AddMoisture(map, moistureData, t, 60);
                }
            }
        }

        return Task.CompletedTask;
    }

    private void AddMoisture(WorldMap map, MapData moistureData, TerrainWorldGameObject t, float amount)
    {
        moistureData.Data[t.Position.X, t.Position.Y] += amount;
        t.MoistureValue += amount;
        if (t.MoistureValue > 1)
            t.MoistureValue = 1;

        //set moisture type
        if (t.MoistureValue < MoistureUtils.DryerValue) t.MoistureType = MoistureType.Dryest;
        else if (t.MoistureValue < MoistureUtils.DryValue) t.MoistureType = MoistureType.Dryer;
        else if (t.MoistureValue < MoistureUtils.WetValue) t.MoistureType = MoistureType.Dry;
        else if (t.MoistureValue < MoistureUtils.WetterValue) t.MoistureType = MoistureType.Wet;
        else if (t.MoistureValue < MoistureUtils.WettestValue) t.MoistureType = MoistureType.Wetter;
        else t.MoistureType = MoistureType.Wettest;
    }

    private void AddMoisture(WorldMap map, MapData moistureData, TerrainWorldGameObject t, int radius)
    {
        int startx = MathsHelper.Mod(t.Position.X - radius, map.Width);
        int endx = MathsHelper.Mod(t.Position.X + radius, map.Width);
        var center = new Vector2(t.Position.X, t.Position.Y);
        int curr = radius;

        while (curr > 0)
        {
            int x1 = MathsHelper.Mod(t.Position.X - curr, map.Width);
            int x2 = MathsHelper.Mod(t.Position.X + curr, map.Width);
            int y = t.Position.Y;


            AddMoisture(
                map,
                moistureData,
                map.GetTerrainAt<TerrainWorldGameObject>(new Point(x1, y)),
                (0.025f / (center - new Vector2(x1, y)).Magnitude())
            );

            for (int i = 0; i < curr; i++)
            {
                AddMoisture(
                    map,
                    moistureData,
                    map.GetTerrainAt<TerrainWorldGameObject>(x1, MathsHelper.Mod(y + i + 1, map.Height)),
                    (int)(0.025f / (center - new Vector2(x1, MathsHelper.Mod(y + i + 1, map.Height))).Magnitude())
                );
                AddMoisture(
                    map,
                    moistureData,
                    map.GetTerrainAt<TerrainWorldGameObject>(x1, MathsHelper.Mod(y - (i + 1), map.Height)),
                    .025f / (center - new Vector2(x1, MathsHelper.Mod(y - (i + 1), map.Height))).Magnitude()
                );

                AddMoisture(
                    map,
                    moistureData,
                    map.GetTerrainAt<TerrainWorldGameObject>(x2, MathsHelper.Mod(y + i + 1, map.Height)),
                    0.025f / (center - new Vector2(x2, MathsHelper.Mod(y + i + 1, map.Height))).Magnitude()
                );
                AddMoisture(
                    map,
                    moistureData,
                    map.GetTerrainAt<TerrainWorldGameObject>(x2, MathsHelper.Mod(y - (i + 1), map.Height)),
                    0.025f / (center - new Vector2(x2, MathsHelper.Mod(y - (i + 1), map.Height))).Magnitude()
                );
            }

            curr--;
        }
    }

    private Task<List<RiverGroup>> BuildRiverGroups(WorldMap map)
    {
        var riverGroups = new List<RiverGroup>();
        for (var x = 0; x < map.Width; x++)
        {
            for (var y = 0; y < map.Height; y++)
            {
                var t = map.GetTerrainAt<TerrainWorldGameObject>(new Point(x, y));
                if (t.Rivers.Count > 1)
                {
                    RiverGroup riverGroup = null;
                    // Does a rivergroup already exist for this group?
                    for (int n = 0; n < t.Rivers.Count; n++)
                    {
                        var tileriver = t.Rivers[n];
                        for (int i = 0; i < riverGroups.Count; i++)
                        {
                            for (int j = 0; j < riverGroups[i].Rivers.Count; j++)
                            {
                                River river = riverGroups[i].Rivers[j];
                                if (river.Id == tileriver.Id)
                                {
                                    riverGroup = riverGroups[i];
                                }

                                if (riverGroup != null)
                                {
                                    break;
                                }
                            }

                            if (riverGroup != null)
                            {
                                break;
                            }
                        }

                        if (riverGroup != null)
                        {
                            break;
                        }
                    }

                    // existing group found -- add to it
                    if (riverGroup != null)
                    {
                        for (int n = 0; n < t.Rivers.Count; n++)
                        {
                            if (!riverGroup.Rivers.Contains(t.Rivers[n]))
                            {
                                riverGroup.Rivers.Add(t.Rivers[n]);
                            }
                        }
                    }
                    else //No existing group found - create a new one
                    {
                        riverGroup = new RiverGroup();
                        for (int n = 0; n < t.Rivers.Count; n++)
                        {
                            riverGroup.Rivers.Add(t.Rivers[n]);
                        }

                        riverGroups.Add(riverGroup);
                    }
                }
            }
        }

        return Task.FromResult(riverGroups);
    }

    private Task DigRiverGroups(List<RiverGroup> riverGroups)
    {
        for (int i = 0; i < riverGroups.Count; i++)
        {
            RiverGroup group = riverGroups[i];
            River longest = null;

            //Find longest river in this group
            for (int j = 0; j < group.Rivers.Count; j++)
            {
                River river = group.Rivers[j];
                if (longest == null)
                    longest = river;
                else if (longest.Tiles.Count < river.Tiles.Count)
                    longest = river;
            }

            if (longest != null)
            {
                //Dig out longest path first
                DigRiver(longest);

                for (int j = 0; j < group.Rivers.Count; j++)
                {
                    River river = group.Rivers[j];
                    if (river != longest)
                    {
                        DigRiver(river, longest);
                    }
                }
            }
        }

        return Task.CompletedTask;
    }

    private void DigRiver(River river, River parent)
    {
        int intersectionID = 0;
        int intersectionSize = 0;

        // determine point of intersection
        for (int i = 0; i < river.Tiles.Count; i++)
        {
            var t1 = river.Tiles[i];
            for (int j = 0; j < parent.Tiles.Count; j++)
            {
                var t2 = parent.Tiles[j];
                if (t1 == t2)
                {
                    intersectionID = i;
                    intersectionSize = t2.RiverSize;
                }
            }
        }

        int counter = 0;
        int intersectionCount = river.Tiles.Count - intersectionID;
        int size = RandomUtils.Range(intersectionSize, 5);
        river.Length = river.Tiles.Count;

        // randomize size change
        int two = river.Length / 2;
        int three = two / 2;
        int four = three / 2;
        int five = four / 2;

        int twomin = two / 3;
        int threemin = three / 3;
        int fourmin = four / 3;
        int fivemin = five / 3;

        // randomize length of each size
        int count1 = RandomUtils.Range(fivemin, five);
        if (size < 4)
        {
            count1 = 0;
        }

        int count2 = count1 + RandomUtils.Range(fourmin, four);
        if (size < 3)
        {
            count2 = 0;
            count1 = 0;
        }

        int count3 = count2 + RandomUtils.Range(threemin, three);
        if (size < 2)
        {
            count3 = 0;
            count2 = 0;
            count1 = 0;
        }

        int count4 = count3 + RandomUtils.Range(twomin, two);

        // Make sure we are not digging past the river path
        if (count4 > river.Length)
        {
            int extra = count4 - river.Length;
            while (extra > 0)
            {
                if (count1 > 0)
                {
                    count1--;
                    count2--;
                    count3--;
                    count4--;
                    extra--;
                }
                else if (count2 > 0)
                {
                    count2--;
                    count3--;
                    count4--;
                    extra--;
                }
                else if (count3 > 0)
                {
                    count3--;
                    count4--;
                    extra--;
                }
                else if (count4 > 0)
                {
                    count4--;
                    extra--;
                }
            }
        }

        // adjust size of river at intersection point
        if (intersectionSize == 1)
        {
            count4 = intersectionCount;
            count1 = 0;
            count2 = 0;
            count3 = 0;
        }
        else if (intersectionSize == 2)
        {
            count3 = intersectionCount;
            count1 = 0;
            count2 = 0;
        }
        else if (intersectionSize == 3)
        {
            count2 = intersectionCount;
            count1 = 0;
        }
        else if (intersectionSize == 4)
        {
            count1 = intersectionCount;
        }
        else
        {
            count1 = 0;
            count2 = 0;
            count3 = 0;
            count4 = 0;
        }

        // dig out the river
        for (int i = river.Tiles.Count - 1; i >= 0; i--)
        {
            var t = river.Tiles[i];

            if (counter < count1)
            {
                t.DigRiver(river, 4);
            }
            else if (counter < count2)
            {
                t.DigRiver(river, 3);
            }
            else if (counter < count3)
            {
                t.DigRiver(river, 2);
            }
            else if (counter < count4)
            {
                t.DigRiver(river, 1);
            }
            else
            {
                t.DigRiver(river, 0);
            }

            counter++;
        }
    }

    private void DigRiver(River river)
    {
        int counter = 0;

        // How wide are we digging this river?
        int size = RandomUtils.Range(1, 5);
        river.Length = river.Tiles.Count;

        // randomize size change
        int two = river.Length / 2;
        int three = two / 2;
        int four = three / 2;
        int five = four / 2;

        int twomin = two / 3;
        int threemin = three / 3;
        int fourmin = four / 3;
        int fivemin = five / 3;

        // randomize lenght of each size
        int count1 = RandomUtils.Range(fivemin, five);
        if (size < 4)
        {
            count1 = 0;
        }

        int count2 = count1 + RandomUtils.Range(fourmin, four);
        if (size < 3)
        {
            count2 = 0;
            count1 = 0;
        }

        int count3 = count2 + RandomUtils.Range(threemin, three);
        if (size < 2)
        {
            count3 = 0;
            count2 = 0;
            count1 = 0;
        }

        int count4 = count3 + RandomUtils.Range(twomin, two);

        // Make sure we are not digging past the river path
        if (count4 > river.Length)
        {
            int extra = count4 - river.Length;
            while (extra > 0)
            {
                if (count1 > 0)
                {
                    count1--;
                    count2--;
                    count3--;
                    count4--;
                    extra--;
                }
                else if (count2 > 0)
                {
                    count2--;
                    count3--;
                    count4--;
                    extra--;
                }
                else if (count3 > 0)
                {
                    count3--;
                    count4--;
                    extra--;
                }
                else if (count4 > 0)
                {
                    count4--;
                    extra--;
                }
            }
        }

        // Dig it out
        for (int i = river.Tiles.Count - 1; i >= 0; i--)
        {
            var t = river.Tiles[i];

            if (counter < count1)
            {
                t.DigRiver(river, 4);
            }
            else if (counter < count2)
            {
                t.DigRiver(river, 3);
            }
            else if (counter < count3)
            {
                t.DigRiver(river, 2);
            }
            else if (counter < count4)
            {
                t.DigRiver(river, 1);
            }
            else
            {
                t.DigRiver(river, 0);
            }

            counter++;
        }
    }

    private void FindPathToWater(WorldMap worldMap, TerrainWorldGameObject tile, Direction direction, ref River river)
    {
        if (tile.Rivers.Contains(river))
        {
            return;
        }

        // check if there is already a river on this tile
        if (tile.Rivers.Count > 0)
        {
            river.Intersections++;
        }

        river.AddTile(tile);

        // get neighbors
        var left = worldMap.GetLeft(tile);
        var right = worldMap.GetRight(tile);
        var top = worldMap.GetTop(tile);
        var bottom = worldMap.GetBottom(tile);

        float leftValue = int.MaxValue;
        float rightValue = int.MaxValue;
        float topValue = int.MaxValue;
        float bottomValue = int.MaxValue;

        // query height values of neighbors
        if (left.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(left))
            leftValue = left.HeightValue;
        if (right.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(right))
            rightValue = right.HeightValue;
        if (top.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(top))
            topValue = top.HeightValue;
        if (bottom.GetRiverNeighborCount(river) < 2 && !river.Tiles.Contains(bottom))
            bottomValue = bottom.HeightValue;

        // if neighbor is existing river that is not this one, flow into it
        if (bottom.Rivers.Count == 0 && !bottom.IsWalkable)
            bottomValue = 0;
        if (top.Rivers.Count == 0 && !top.IsWalkable)
            topValue = 0;
        if (left.Rivers.Count == 0 && !left.IsWalkable)
            leftValue = 0;
        if (right.Rivers.Count == 0 && !right.IsWalkable)
            rightValue = 0;

        // override flow direction if a tile is significantly lower
        if (direction == Direction.Left)
            if (Math.Abs(rightValue - leftValue) < 0.1f)
                rightValue = int.MaxValue;
        if (direction == Direction.Right)
            if (Math.Abs(rightValue - leftValue) < 0.1f)
                leftValue = int.MaxValue;
        if (direction == Direction.Up)
            if (Math.Abs(topValue - bottomValue) < 0.1f)
                bottomValue = int.MaxValue;
        if (direction == Direction.Down)
            if (Math.Abs(topValue - bottomValue) < 0.1f)
                topValue = int.MaxValue;

        // find mininum
        float min = Math.Min(Math.Min(Math.Min(leftValue, rightValue), topValue), bottomValue);

        // if no minimum found - exit
        if (min == int.MaxValue)
            return;

        //Move to next neighbor
        if (min == leftValue)
        {
            if (left.IsWalkable)
            {
                if (river.CurrentDirection != Direction.Left)
                {
                    river.TurnCount++;
                    river.CurrentDirection = Direction.Left;
                }

                FindPathToWater(worldMap, left, direction, ref river);
            }
        }
        else if (min == rightValue)
        {
            if (right.IsWalkable)
            {
                if (river.CurrentDirection != Direction.Right)
                {
                    river.TurnCount++;
                    river.CurrentDirection = Direction.Right;
                }

                FindPathToWater(worldMap, right, direction, ref river);
            }
        }
        else if (min == bottomValue)
        {
            if (bottom.IsWalkable)
            {
                if (river.CurrentDirection != Direction.Down)
                {
                    river.TurnCount++;
                    river.CurrentDirection = Direction.Down;
                }

                FindPathToWater(worldMap, bottom, direction, ref river);
            }
        }
        else if (min == topValue)
        {
            if (top.IsWalkable)
            {
                if (river.CurrentDirection != Direction.Up)
                {
                    river.TurnCount++;
                    river.CurrentDirection = Direction.Up;
                }

                FindPathToWater(worldMap, top, direction, ref river);
            }
        }
    }

    private async Task<IEnumerable<LandZoneObject>> PlaceLands(
        Map worldMap, WorldMapConfig config, Dictionary<string, List<TerrainGroupObject>> zones
    )
    {
        var count = RandomUtils.Range(2, 30);
        var landGameObjects = new List<LandGameObject>();
        var landZones = new List<LandZoneObject>();
        var placebleZones = zones.Where(s => s.Key != "WATER" && s.Key != "MOUNTAIN")
            .ToDictionary(pair => pair.Key, pair => pair.Value);


        Logger.LogInformation("Placing cities...");

        foreach (var _ in Enumerable.Range(0, count))
        {
            var spawnResult = await _mapSpawnerService.SpawnAsync(
                worldMap,
                placebleZones[placebleZones.Keys.RandomElement()].RandomElement()
            );
            if (spawnResult != null)
            {
                landGameObjects.AddRange(spawnResult.LandGameObjects);
                landZones.Add(spawnResult);

                foreach (var gameObject in spawnResult.LandGameObjects)
                {
                    worldMap.AddEntity(gameObject);
                }
            }
        }

        Logger.LogInformation("Cities placed");
        return landZones;
    }

    private async Task PlacePlayer(WorldMap worldMap, IEnumerable<LandGameObject> lands)
    {
        var playerPosition = lands.RandomElement();

        var playerTile = _mapSpawnerService.SearchLandTileByFlag("player").FirstOrDefault();

        if (playerTile == null)
        {
            throw new Exception("Player tile not found");
        }

        var playerResult = _tileService.GetTile(playerTile);

        worldMap.AddEntity(
            new WorldPlayerGameObject("player", playerPosition.Position, playerResult.coloredGlyph, true, true)
        );
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

                mapData.AddData(x, y, value);
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

                mapData.AddData(x, y, heightValue);
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
                float value = mapData.GetData(x, y);

                //normalize our value between 0 and 1
                value = (value - mapData.Min) / (mapData.Max - mapData.Min);

                var key = perlinLimits.CheckValueInDictionary<double, TerrainEntity>((double)value);
                var tile = _tileService.GetGlyphFromHasTileEntity(key);
                var t = new TerrainWorldGameObject(
                    new Point(x, y),
                    tile,
                    key.IsWalkable,
                    key.IsTransparent,
                    key.Flags
                );


                t.HeightValue = value;
                worldMap.SetTerrain(t);
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

                terrain.Top = worldMap.GetTop(terrain);
                terrain.Bottom = worldMap.GetBottom(terrain);
                terrain.Left = worldMap.GetLeft(terrain);
                terrain.Right = worldMap.GetRight(terrain);
            }
        }


        return Task.CompletedTask;
    }
}

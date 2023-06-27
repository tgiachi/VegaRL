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
using Vega.Framework.Map.GameObjects.World;
using Vega.Framework.Noise;

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
        var values = config.NoiseType == WorldMapNoiseType.Perlin
            ? await GeneratePerlinNoiseMap()
            : await GenerateFastNoiseMap();
        var mappedValues = await GetWorldMapTerrains();

        await FillOverMapAsync(worldMap, mappedValues, values);


        return worldMap;
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

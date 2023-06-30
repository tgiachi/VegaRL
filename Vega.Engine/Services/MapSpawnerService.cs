using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.WorldMap;

using Vega.Framework.Map;
using Vega.Framework.Map.WorldMap;
using Vega.Framework.Map.WorldMap.GameObjects;
using Vega.Framework.Utils.Random;

namespace Vega.Engine.Services;

[VegaService(5)]
public class MapSpawnerService : BaseDataLoaderVegaService<MapSpawnerService>, IMapSpawnerService
{
    private readonly Dictionary<string, LandEntity> _lands = new();
    private readonly Dictionary<string, LandSpawnGroupEntity> _landSpawnGroups = new();
    private readonly Dictionary<string, List<WorldMapSpawnLocationEntity>> _spawnLocations = new();

    private readonly ITileService _tileService;


    public MapSpawnerService(
        ILogger<MapSpawnerService> logger, IDataService dataService, IMessageBusService messageBusService,
        ITileService tileService
    ) : base(logger, dataService, messageBusService)
    {
        _tileService = tileService;
    }

    private Task LoadLands()
    {
        foreach (var land in LoadData<LandEntity>())
        {
            _lands.Add(land.Id.ToLower(), land);
        }

        return Task.CompletedTask;
    }

    private Task LoadLandGroups()
    {
        foreach (var groupEntity in LoadData<LandSpawnGroupEntity>())
        {
            _landSpawnGroups.Add(groupEntity.Id.ToLower(), groupEntity);
        }

        return Task.CompletedTask;
    }

    private Task LoadWorldMapSpawnLocations()
    {
        foreach (var spawnLocationEntity in LoadData<WorldMapSpawnLocationEntity>())
        {
            if (!_spawnLocations.ContainsKey(spawnLocationEntity.Group.ToLower()))
            {
                _spawnLocations.Add(spawnLocationEntity.Group.ToLower(), new List<WorldMapSpawnLocationEntity>());
            }

            _spawnLocations[spawnLocationEntity.Group.ToLower()].Add(spawnLocationEntity);
        }

        return Task.CompletedTask;
    }

    public override Task<bool> LoadAsync()
    {
        LoadLands();
        LoadLandGroups();
        LoadWorldMapSpawnLocations();

        return Task.FromResult(true);
    }

    public Task<LandZoneObject?> SpawnAsync(Map map, TerrainGroupObject group)
    {
        var gameObjects = new List<LandGameObject>();

        if (!_spawnLocations.ContainsKey(group.TileType.ToLower()))
        {
            Logger.LogWarning("No spawn locations found for group {Type}", group.TileType);
            return Task.FromResult<LandZoneObject>(null);
        }

        var spawnLocation = _spawnLocations[group.TileType.ToLower()].RandomElement();

        var landGroup = _landSpawnGroups[spawnLocation.LandGroupId.ToLower()];
        var totalPositions = group.Rectangle.Width * group.Rectangle.Height;
        var spawnCount = RandomUtils.Range(totalPositions / 2, totalPositions);
        var spawnCurrent = 0;
        var visited = new HashSet<Point>();
        var positions = new List<Point>();
        while (spawnCurrent < spawnCount)
        {
            var pos = group.Rectangle.Positions().RandomElement();
            if (!visited.Contains(pos))
            {
                visited.Add(pos);
                positions.Add(pos);
                spawnCurrent++;
            }
        }

        foreach (var position in positions)
        {
            var land = _lands[landGroup.Lands.RandomElement().ToLower()];
            var tile = _tileService.GetGlyphFromHasTileEntity(land);
            gameObjects.Add(new LandGameObject(land.Id, position, tile, true, false));
        }

        return Task.FromResult(new LandZoneObject(gameObjects) );
    }

    public IEnumerable<LandEntity> SearchLandTileByFlag(string flag)
    {
        return _lands.Values.Where(x => x.SearchFlags(flag) != null);
    }
}

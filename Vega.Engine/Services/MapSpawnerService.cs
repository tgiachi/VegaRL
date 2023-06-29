using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.WorldMap;
using Vega.Framework.Map;

namespace Vega.Engine.Services;

[VegaService(5)]
public class MapSpawnerService : BaseDataLoaderVegaService<MapSpawnerService>, IMapSpawnerService
{
    private readonly Dictionary<string, LandEntity> _lands = new();
    private readonly Dictionary<string, LandSpawnGroupEntity> _landSpawnGroups = new();
    private readonly Dictionary<string, List<WorldMapSpawnLocationEntity>> _spawnLocations = new();


    public MapSpawnerService(
        ILogger<MapSpawnerService> logger, IDataService dataService, IMessageBusService messageBusService
    ) : base(logger, dataService, messageBusService)
    {
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

    public Task<IEnumerable<GameObject>?> SpawnAsync(Map map, TerrainGroupObject group)
    {
        var gameObjects = new List<GameObject>();

        if (!_spawnLocations.ContainsKey(group.TileType.ToLower()))
        {
            Logger.LogWarning("No spawn locations found for group {Type}", group.TileType);
            return Task.FromResult<IEnumerable<GameObject>>(null);
        }

        

        return Task.FromResult<IEnumerable<GameObject>>(gameObjects);
    }
}

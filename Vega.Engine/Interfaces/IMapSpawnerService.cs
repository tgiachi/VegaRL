

using GoRogue.GameFramework;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Data.Entities.WorldMap;
using Vega.Framework.Map;

namespace Vega.Engine.Interfaces;

public interface IMapSpawnerService : IVegaService
{
    Task<IEnumerable<IGameObject>?> SpawnAsync(Map map, TerrainGroupObject group);

    public IEnumerable<LandEntity> SearchLandTileByFlag(string flag);
}

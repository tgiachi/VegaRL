using Vega.Engine.Interfaces.Services;
using Vega.Framework.Map.WorldMap;

namespace Vega.Engine.Interfaces;

public interface ISaveLoadService : IVegaService
{
    Task<bool> SaveWorldMapAsync(WorldMap worldMap, string path);
}

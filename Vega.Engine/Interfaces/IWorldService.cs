using Vega.Engine.Interfaces.Services;
using Vega.Framework.Map;

namespace Vega.Engine.Interfaces;

public interface IWorldService : IVegaService
{
    WorldMap CurrentWorldMap { get; set; }
    GridMap CurrentGridMap { get; set; }
}

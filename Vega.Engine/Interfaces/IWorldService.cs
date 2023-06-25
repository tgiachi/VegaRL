using Vega.Api.Map;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface IWorldService : IVegaService
{
    WorldMap CurrentWorldMap { get; set; }
    GridMap CurrentGridMap { get; set; }
}

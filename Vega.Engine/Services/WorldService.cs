using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Api.Map;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(10)]
public class WorldService : BaseVegaService<WorldService>, IWorldService
{
    public WorldMap CurrentWorldMap { get; set; }
    public GridMap CurrentGridMap { get; set; }

    public WorldService(ILogger<WorldService> logger, IMessageBusService messageBusService) : base(logger, messageBusService)
    {
    }
}

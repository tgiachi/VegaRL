using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Map;

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

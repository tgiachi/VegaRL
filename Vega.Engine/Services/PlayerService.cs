using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(10)]
public class PlayerService : BaseVegaService<PlayerService>, IPlayerService
{
    public PlayerService(ILogger<PlayerService> logger) : base(logger)
    {
    }
}

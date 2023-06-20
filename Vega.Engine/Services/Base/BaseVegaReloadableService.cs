using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Services.Base;

public abstract class BaseVegaReloadableService<TService> : BaseVegaService<TService>, IVegaReloadableService
{
    public BaseVegaReloadableService(ILogger<TService> logger) : base(logger)
    {
    }

    public virtual Task<bool> ReloadAsync() => Task.FromResult(true);
}

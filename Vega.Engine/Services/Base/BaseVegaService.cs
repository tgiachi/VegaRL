using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Services.Base;

public abstract class BaseVegaService<TService> : IVegaService
{
    protected ILogger Logger { get; }

    protected BaseVegaService(ILogger<TService> logger)
    {
        Logger = logger;
    }

    public virtual Task<bool> LoadAsync() => Task.FromResult(true);

    public virtual Task<bool> InitializeAsync() => Task.FromResult(true);
}

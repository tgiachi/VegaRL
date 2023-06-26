using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Services.Base;

public abstract class BaseVegaService<TService> : IVegaService
{
    protected ILogger Logger { get; }
    protected IMessageBusService MessageBus { get; }

    protected BaseVegaService(ILogger<TService> logger, IMessageBusService messageBusService)
    {
        Logger = logger;
        MessageBus = messageBusService;
    }

    public virtual Task<bool> LoadAsync() => Task.FromResult(true);

    public virtual Task<bool> InitializeAsync() => Task.FromResult(true);
}

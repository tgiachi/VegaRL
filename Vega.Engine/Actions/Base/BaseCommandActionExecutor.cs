using Microsoft.Extensions.Logging;
using Vega.Api.Data.Events;
using Vega.Api.Interfaces.Actions;
using Vega.Engine.Interfaces;

namespace Vega.Engine.Actions.Base;

public abstract class BaseCommandActionExecutor<TAction> : IActionExecutor where TAction : ICommandAction
{
    private readonly IMessageBusService _eventBusService;
    protected ILogger Logger { get; }


    protected BaseCommandActionExecutor(
        ILogger<BaseCommandActionExecutor<TAction>> logger, IMessageBusService eventBusService
    )
    {
        Logger = logger;
        _eventBusService = eventBusService;
    }

    public Task Execute(ICommandAction action) => Execute((TAction)action);

    public virtual Task Execute(TAction action) => Task.CompletedTask;

    protected Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : BaseEvent
    {
        _eventBusService.Send(@event);

        return Task.CompletedTask;
    }
}

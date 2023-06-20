using GoRogue.Messaging;
using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Api.Data.Events;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(0)]
public class MessageBusService : BaseVegaService<MessageBusService>, IMessageBusService
{
    private readonly MessageBus _messageBus = new();

    public MessageBusService(ILogger<MessageBusService> logger) : base(logger)
    {
    }

    public void Send<T>(T message) where T : BaseEvent
    {
        try
        {
            _messageBus.Send(message);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error sending message: {Message}", ex.Message);
        }
    }

    public void Subscribe<T>(ISubscriber<T> subscriber) where T : BaseEvent
    {
        Logger.LogDebug("Subscribing to {Subscriber}", typeof(T).Name);
        _messageBus.RegisterSubscriber(subscriber);
    }
}

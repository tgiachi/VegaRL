using GoRogue.Messaging;
using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Api.Data.Events;
using Vega.Engine.Interfaces;


namespace Vega.Engine.Services;

[VegaService(0)]
public class MessageBusService :  IMessageBusService
{
    private readonly MessageBus _messageBus = new();
    private readonly ILogger _logger;

    public MessageBusService(ILogger<MessageBusService> logger)
    {
        _logger = logger;
    }

    public void Send<T>(T message) where T : BaseEvent
    {
        try
        {
            _messageBus.Send(message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error sending message: {Message}", ex.Message);
        }
    }

    public void Subscribe<T>(ISubscriber<T> subscriber) where T : BaseEvent
    {
        _logger.LogDebug("Subscribing to {Subscriber}", typeof(T).Name);
        _messageBus.RegisterSubscriber(subscriber);
    }

    public Task<bool> LoadAsync() => Task.FromResult(true);

    public Task<bool> InitializeAsync() => Task.FromResult(true);
}

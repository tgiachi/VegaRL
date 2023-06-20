using GoRogue.Messaging;
using Vega.Api.Data.Events;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface IMessageBusService : IVegaService
{
    void Send<T>(T message) where T : BaseEvent;
    void Subscribe<T>(ISubscriber<T> subscriber) where T : BaseEvent;
}

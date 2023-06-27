using GoRogue.Messaging;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Data.Events;

namespace Vega.Engine.Interfaces;

public interface IMessageBusService : IVegaService
{
    void Send<T>(T message) where T : BaseEvent;
    void Subscribe<T>(ISubscriber<T> subscriber) where T : BaseEvent;
}

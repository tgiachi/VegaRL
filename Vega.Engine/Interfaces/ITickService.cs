using Vega.Api.Interfaces.Actions;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface ITickService : IVegaService
{
    DateTime CurrentDateTime { get; }

    int TotalTicks { get; }

    void EnqueueAction(ICommandAction action);

    Task Tick();

}

using Vega.Engine.Interfaces.Services;
using Vega.Framework.Interfaces.Actions;

namespace Vega.Engine.Interfaces;

public interface ITickService : IVegaService
{
    DateTime CurrentDateTime { get; }

    int TotalTicks { get; }

    void EnqueueAction(ICommandAction action);

    Task Tick();

}

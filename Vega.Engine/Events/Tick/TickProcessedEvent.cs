using Vega.Framework.Data.Events;

namespace Vega.Engine.Events.Tick;

public class TickProcessedEvent : BaseEvent
{
    public int TotalTicks { get; set; }

    public DateTime CurrentDateTime { get; set; }

    public override string ToString() =>
        $"{base.ToString()}, {nameof(TotalTicks)}: {TotalTicks}, {nameof(CurrentDateTime)}: {CurrentDateTime}";
}

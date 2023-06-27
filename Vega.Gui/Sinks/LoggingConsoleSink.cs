using Serilog.Core;
using Serilog.Events;
using Vega.Engine.Events.Engine;

namespace Vega.Gui.Sinks;

public class LoggingConsoleSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        if (InstancesHolder.Manager != null)
        {
            InstancesHolder.Manager.PublishEvent(
                new LogEmittedEvent()
                {
                    Timestamp = logEvent.Timestamp.DateTime,
                    LogLevel = logEvent.Level.ToString(),
                    Message = logEvent.RenderMessage()
                }
            );
        }
    }
}


using Vega.Framework.Data.Events;

namespace Vega.Engine.Events.Engine;

public class LogEmittedEvent : BaseEvent
{
    public string LogLevel { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;


    public LogEmittedEvent(string logLevel, string message, DateTime timestamp)
    {
        LogLevel = logLevel;
        Message = message;
        Timestamp = timestamp;
    }

    public LogEmittedEvent()
    {

    }

    public override string ToString() => $"{Timestamp} [{LogLevel}] {Message}";
}

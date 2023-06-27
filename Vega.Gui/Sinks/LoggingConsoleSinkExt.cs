using System;
using Serilog;
using Serilog.Configuration;

namespace Vega.Gui.Sinks;

public static class LoggingConsoleSinkExt
{
    public static LoggerConfiguration LogConsoleSink(
        this LoggerSinkConfiguration loggerConfiguration,
        IFormatProvider formatProvider = null
    ) =>
        loggerConfiguration.Sink(new LoggingConsoleSink());
}

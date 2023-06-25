using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

public class TickService : BaseVegaService<ITickService>, ITickService
{
    public TickService(ILogger<ITickService> logger) : base(logger)
    {
    }

    /// <summary>
    ///  Parses a string into a TimeSpan.
    ///  Example: 1d 2h 3m 4s
    /// </summary>
    /// <param name="timeSpanString"></param>
    /// <returns></returns>
    private TimeSpan ParseTimeSpanFromString(string timeSpanString)
    {
        var days = 0;
        var hours = 0;
        var minutes = 0;
        var seconds = 0;

        var parts = timeSpanString.Split(' ');

        foreach (string part in parts)
        {
            if (part.EndsWith("d"))
            {
                days = int.Parse(part.TrimEnd('d'));
            }
            else if (part.EndsWith("h"))
            {
                hours = int.Parse(part.TrimEnd('h'));
            }
            else if (part.EndsWith("m"))
            {
                minutes = int.Parse(part.TrimEnd('m'));
            }
            else if (part.EndsWith("s"))
            {
                seconds = int.Parse(part.TrimEnd('s'));
            }
        }

        var timeSpan = new TimeSpan(days, hours, minutes, seconds);

        return timeSpan;
    }
}

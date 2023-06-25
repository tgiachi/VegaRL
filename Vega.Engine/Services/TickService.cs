using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

public class TickService : BaseVegaService<ITickService>, ITickService
{
    private static string _pattern = @"(?:(\d+)d\s*)?(?:(\d+)h\s*)?(?:(\d+)m\s*)?(?:(\d+)s)?";

    // Execute the regex on the input string
    //Match match = Regex.Match(input, pattern);

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
        // Define the regex pattern to match the values of days, hours, minutes, and seconds

        int days = 0;
        int hours = 0;
        int minutes = 0;
        int seconds = 0;

        // Extract the values from the regex matches
        if (match.Success)
        {
            if (match.Groups[1].Success)
                days = int.Parse(match.Groups[1].Value);

            if (match.Groups[2].Success)
                hours = int.Parse(match.Groups[2].Value);

            if (match.Groups[3].Success)
                minutes = int.Parse(match.Groups[3].Value);

            if (match.Groups[4].Success)
                seconds = int.Parse(match.Groups[4].Value);
        }

        // Create a TimeSpan object using the extracted values
        TimeSpan timeSpan = new TimeSpan(days, hours, minutes, seconds);

        return timeSpan;
    }
    }
}

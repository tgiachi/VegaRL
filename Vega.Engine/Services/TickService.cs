using System.Text.RegularExpressions;
using GoRogue.Messaging;
using Microsoft.Extensions.Logging;
using Vega.Engine.Events.Tick;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Interfaces.Actions;

namespace Vega.Engine.Services;

[VegaService(10)]
public partial class TickService : BaseVegaService<ITickService>, ITickService, ISubscriber<TickRequestEvent>
{
    public DateTime CurrentDateTime { get; private set; }

    private readonly Dictionary<string, IActionExecutor> _actionExecutors = new();
    private readonly SortedDictionary<int, List<ICommandAction>> _actionQueue = new();

    public int TotalTicks { get; private set; }


    private readonly Regex _regex = TimeSpanParserRegex();

    // Execute the regex on the input string
    //Match match = Regex.Match(input, pattern);

    public TickService(ILogger<ITickService> logger, IMessageBusService messageBusService) : base(logger, messageBusService)
    {
        CurrentDateTime = DateTime.Parse("2028-12-06 04:00:00");
        TotalTicks = 0;
        MessageBus.Subscribe(this);
    }

    public void EnqueueAction(ICommandAction action)
    {
        var key = TotalTicks + action.Turns;
        if (!_actionQueue.ContainsKey(key))
        {
            _actionQueue.Add(key, new List<ICommandAction>());
        }

        _actionQueue[key].Add(action);
    }

    private void Remove(ICommandAction action)
    {
        var actionListFound = new KeyValuePair<int, List<ICommandAction>>(-1, null);
        foreach (var actionList in _actionQueue)
        {
            if (actionList.Value.Contains(action))
            {
                actionListFound = actionList;
                break;
            }
        }

        if (actionListFound.Value != null)
        {
            actionListFound.Value.Remove(action);
            if (actionListFound.Value.Count <= 0)
            {
                _actionQueue.Remove(actionListFound.Key);
            }
        }
    }

    private ICommandAction NextAction()
    {
        var firstActionGroup = _actionQueue.First();
        var firstAction = firstActionGroup.Value.First();
        Remove(firstAction);
        TotalTicks += firstActionGroup.Key;
        return firstAction;
    }

    public async Task Tick()
    {
        try
        {
            var action = NextAction();
            if (action != null)
            {
                var actionExecutor = _actionExecutors[action.Action];
                await actionExecutor.Execute(action);
                if (action.TimeExecution != null)
                {
                    var timeSpan = ParseTimeSpanFromString(action.TimeExecution);
                    CurrentDateTime = CurrentDateTime.Add(timeSpan);
                }
                else
                {
                    CurrentDateTime = CurrentDateTime.AddMinutes(1);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("Error in TickService.Tick: {Error}", ex.Message);
        }
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

        var match = _regex.Match(timeSpanString);
        // Extract the values from the regex matches
        if (match.Success)
        {
            if (match.Groups[1].Success)
            {
                days = int.Parse(match.Groups[1].Value);
            }

            if (match.Groups[2].Success)
            {
                hours = int.Parse(match.Groups[2].Value);
            }

            if (match.Groups[3].Success)
            {
                minutes = int.Parse(match.Groups[3].Value);
            }

            if (match.Groups[4].Success)
            {
                seconds = int.Parse(match.Groups[4].Value);
            }
        }

        // Create a TimeSpan object using the extracted values
        var timeSpan = new TimeSpan(days, hours, minutes, seconds);

        return timeSpan;
    }

    [GeneratedRegex("(?:(\\d+)d\\s*)?(?:(\\d+)h\\s*)?(?:(\\d+)m\\s*)?(?:(\\d+)s)?", RegexOptions.Compiled)]
    private static partial Regex TimeSpanParserRegex();

    public void Handle(TickRequestEvent message)
    {
        _ = Task.Run(Tick);
    }
}

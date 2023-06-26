namespace Vega.Api.Interfaces.Actions;

public interface ICommandAction
{
    string Action { get; }
    int Turns { get; set; }

    string? TimeExecution { get; set; }
}

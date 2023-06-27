namespace Vega.Framework.Interfaces.Actions;

public interface ICommandAction
{
    string Action { get; }
    int Turns { get; set; }

    string? TimeExecution { get; set; }
}

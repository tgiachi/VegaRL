namespace Vega.Framework.Interfaces.Actions;

public interface IActionExecutor
{
    Task Execute(ICommandAction action);
}

namespace Vega.Api.Interfaces.Actions;

public interface IActionExecutor
{
    Task Execute(ICommandAction action);
}

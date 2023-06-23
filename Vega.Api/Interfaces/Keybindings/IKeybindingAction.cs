namespace Vega.Api.Interfaces.Keybindings;

public interface IKeybindingAction
{
    Task ExecuteAsync(string command);
}

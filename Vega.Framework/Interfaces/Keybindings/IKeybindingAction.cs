namespace Vega.Framework.Interfaces.Keybindings;

public interface IKeybindingAction
{
    Task ExecuteAsync(string command);
}

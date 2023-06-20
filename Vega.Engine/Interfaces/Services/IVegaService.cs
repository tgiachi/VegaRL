namespace Vega.Engine.Interfaces.Services;

/// <summary>
///  Base interface for all services.
/// </summary>
public interface IVegaService
{
    Task<bool> LoadAsync();
    Task<bool> InitializeAsync();
}

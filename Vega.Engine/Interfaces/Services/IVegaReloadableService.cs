namespace Vega.Engine.Interfaces.Services;

public interface IVegaReloadableService
{
    Task<bool> ReloadAsync();
}

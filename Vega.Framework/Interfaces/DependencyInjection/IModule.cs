using Microsoft.Extensions.DependencyInjection;

namespace Vega.Framework.Interfaces.DependencyInjection;

public interface IModule
{
    IServiceCollection Load(IServiceCollection services);
}

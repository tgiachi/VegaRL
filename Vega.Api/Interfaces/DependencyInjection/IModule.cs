using Microsoft.Extensions.DependencyInjection;

namespace Vega.Api.Interfaces.DependencyInjection;

public interface IModule
{
    IServiceCollection Load(IServiceCollection services);
}

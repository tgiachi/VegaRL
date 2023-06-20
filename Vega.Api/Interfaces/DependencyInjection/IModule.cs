using Microsoft.Extensions.DependencyInjection;

namespace Vega.Api.Interfaces.DependencyInjection;

public interface IModule
{
    ServiceCollection Load(ServiceCollection services);
}

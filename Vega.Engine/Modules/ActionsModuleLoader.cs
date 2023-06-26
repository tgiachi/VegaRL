using Microsoft.Extensions.DependencyInjection;
using Vega.Api.Attributes;
using Vega.Api.Interfaces.DependencyInjection;
using Vega.Api.Utils;

namespace Vega.Engine.Modules;

[ContainerModule]
public class ActionsModuleLoader : IModule
{
    public IServiceCollection Load(IServiceCollection services)
    {
        AssemblyUtils.GetAttribute<ActionExecutorAttribute>()
            .ForEach(
                s =>
                {
                    services.AddSingleton(s);
                }
            );
        return services;
    }
}

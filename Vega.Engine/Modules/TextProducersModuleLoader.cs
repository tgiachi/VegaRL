using Microsoft.Extensions.DependencyInjection;
using Vega.Api.Attributes;
using Vega.Api.Interfaces.DependencyInjection;
using Vega.Api.Utils;

namespace Vega.Engine.Modules;

[ContainerModule]
public class TextProducersModuleLoader : IModule
{
    public IServiceCollection Load(IServiceCollection services)
    {
        AssemblyUtils.GetAttribute<TextProducerAttribute>().ForEach(s => services.AddSingleton(s));
        return services;
    }
}

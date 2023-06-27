using Microsoft.Extensions.DependencyInjection;
using Vega.Framework.Attributes;
using Vega.Framework.Interfaces.DependencyInjection;
using Vega.Framework.Utils;

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

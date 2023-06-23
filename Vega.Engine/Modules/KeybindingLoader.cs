using Microsoft.Extensions.DependencyInjection;
using Vega.Api.Attributes;
using Vega.Api.Interfaces.DependencyInjection;
using Vega.Api.Utils;

namespace Vega.Engine.Modules;

[DiModule]
public class KeybindingLoader : IModule
{
    public IServiceCollection Load(IServiceCollection services)
    {
        AssemblyUtils.GetAttribute<KeybindingAttribute>()
            .ForEach(
                s =>
                {
                    services.AddSingleton(s);
                }
            );

        return services;
    }
}

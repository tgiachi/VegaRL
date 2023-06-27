using Microsoft.Extensions.DependencyInjection;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Attributes;
using Vega.Framework.Interfaces.DependencyInjection;
using Vega.Framework.Utils;

namespace Vega.Engine.Modules;

[ContainerModule]
public class ServicesModule : IModule
{
    public IServiceCollection Load(IServiceCollection services)
    {
        AssemblyUtils.GetAttribute<VegaServiceAttribute>()
            .ForEach(
                s =>
                {
                    var serviceInterf = AssemblyUtils.GetInterfacesOfType(s)
                        .FirstOrDefault(s => s != typeof(IVegaService) && s != typeof(IVegaReloadableService));

                    services.AddSingleton(serviceInterf, s);
                }
            );

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Vega.Api.Attributes;
using Vega.Api.Interfaces.DependencyInjection;
using Vega.Api.Utils;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Modules;

[DiModule]
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

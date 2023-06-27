using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Vega.Framework.Attributes;
using Vega.Framework.Interfaces.DependencyInjection;

namespace Vega.Gui.Modules;

[ContainerModule]
public class LoggerModule : IModule
{
    public IServiceCollection Load(IServiceCollection services)
    {
        services.AddLogging(
            builder =>
            {
                builder.AddSerilog();
            }
        );

        return services;
    }
}

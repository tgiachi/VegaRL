using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Vega.Api.Attributes;
using Vega.Api.Interfaces.DependencyInjection;

namespace Vega.Gui.Modules;

[DiModule]
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

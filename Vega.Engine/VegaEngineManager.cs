using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Vega.Api.Attributes;
using Vega.Api.Data.Config;
using Vega.Api.Interfaces.DependencyInjection;

namespace Vega.Engine;

public class VegaEngineManager
{
    private IServiceProvider _serviceProvider;

    private ServiceCollection _serviceCollection = new();
    private readonly VegaEngineOption _vegaEngineOptions;


    public VegaEngineManager(LoggerConfiguration configuration, VegaEngineOption options)
    {
        _vegaEngineOptions = options;
        Log.Logger = configuration.CreateLogger();
    }

    private void LoadModules()
    {
        Log.Logger.Information("Loading modules...");

        var modules = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<DiModuleAttribute>() != null)
            .OrderBy(t => t.GetCustomAttribute<DiModuleAttribute>())
            .ToList();

        foreach (var module in modules)
        {
            try
            {
                Log.Logger.Information("Loading module {Module}", module.Name);
                var moduleInstance = Activator.CreateInstance(module) as IModule;
                _serviceCollection = moduleInstance.Load(_serviceCollection);
            }
            catch (Exception ex)
            {
                Log.Error("Error loading module {Module}: {Message}", module.Name, ex.Message);
            }
        }
    }

    public Task Load()
    {
        LoadModules();
        
        _serviceProvider = _serviceCollection.BuildServiceProvider();

        return Task.CompletedTask;
    }
}

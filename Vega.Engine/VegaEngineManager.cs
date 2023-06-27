using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Vega.Engine.Events.Engine;
using Vega.Engine.Interfaces;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Config;
using Vega.Framework.Data.Directories;
using Vega.Framework.Data.Events;
using Vega.Framework.Interfaces.DependencyInjection;
using Vega.Framework.Utils;

namespace Vega.Engine;

public class VegaEngineManager
{
    private IServiceProvider _serviceProvider;

    private IServiceCollection _serviceCollection = new ServiceCollection();
    private readonly VegaEngineOption _vegaEngineOptions;

    private readonly SortedDictionary<int, List<Type>> _servicesTypes = new();
    private readonly SortedDictionary<int, List<IVegaService>> _services = new();


    public VegaEngineManager(LoggerConfiguration configuration, VegaEngineOption options)
    {
        _vegaEngineOptions = options;
        Log.Logger = configuration.CreateLogger();
    }

    private void LoadModules()
    {
        Log.Logger.Information("Loading modules...");

        var modules = AssemblyUtils.GetAttribute<ContainerModuleAttribute>();

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

    public void PublishEvent<TEvent>(TEvent @event) where TEvent : BaseEvent
    {
        Resolve<IMessageBusService>().Send(@event);
    }

    public TService Resolve<TService>() => _serviceProvider.GetRequiredService<TService>();

    public async Task Initialize()
    {
        ScanServices();
        LoadModules();

        var directoriesConfig = new DirectoriesConfig();
        directoriesConfig.Initialize(_vegaEngineOptions.RootDirectory);

        _serviceCollection = _serviceCollection
            .AddSingleton(_vegaEngineOptions)
            .AddSingleton(directoriesConfig);

        _serviceProvider = _serviceCollection.BuildServiceProvider();
        var eventBus = _serviceProvider.GetRequiredService<IMessageBusService>();
        eventBus.Send(new EngineLoadingEvent());
        await InitializeServices();
        eventBus.Send(new EngineReadyEvent());
    }

    private Task ScanServices()
    {
        AssemblyUtils.GetAttribute<VegaServiceAttribute>()
            .ForEach(
                s =>
                {
                    var attribute = s.GetCustomAttribute<VegaServiceAttribute>();
                    var serviceInterf = AssemblyUtils.GetInterfacesOfType(s)
                        .FirstOrDefault(s => s != typeof(IVegaService) && s != typeof(IVegaReloadableService))!;
                    if (!_servicesTypes.ContainsKey(attribute.Priority))
                    {
                        _servicesTypes.Add(attribute.Priority, new());
                    }

                    _servicesTypes[attribute.Priority].Add(serviceInterf);
                }
            );

        return Task.CompletedTask;
    }

    public void PreloadFonts(SadConsole.GameHost host)
    {
        Log.Logger.Information("Preloading fonts...");
        var fonts = Directory.GetFiles(
            Path.Join(Directory.GetCurrentDirectory(), "Assets"),
            "*.font",
            SearchOption.AllDirectories
        );
        foreach (var font in fonts)
        {
            Log.Logger.Information("Preloading font {Font}", font);
            host.LoadFont(font.Replace(Directory.GetCurrentDirectory() + "\\", ""));
        }
    }

    private Task InitializeServices()
    {
        foreach (var service in _servicesTypes)
        {
            foreach (var serviceType in service.Value)
            {
                Log.Logger.Debug("Initializing service {Service}", serviceType.Name);
                var serviceInstance = _serviceProvider.GetRequiredService(serviceType) as IVegaService;
                if (!_services.ContainsKey(service.Key))
                {
                    _services.Add(service.Key, new());
                }

                _services[service.Key].Add(serviceInstance);
            }
        }

        return Task.CompletedTask;
    }

    public async Task LoadServices()
    {
        foreach (var service in _services)
        {
            foreach (var serviceInstance in service.Value)
            {
                try
                {
                    Log.Logger.Debug("Loading service {Service}", serviceInstance.GetType().Name);
                    await serviceInstance.LoadAsync();
                }
                catch (Exception ex)
                {
                    Log.Error("Error loading service {Service}: {Message}", serviceInstance.GetType().Name, ex.Message);
                }
            }
        }
    }

    public async Task ReloadServices()
    {
        foreach (var service in _services)
        {
            foreach (var serviceInstance in service.Value)
            {
                if (serviceInstance is IVegaReloadableService reloadableService)
                {
                    Log.Logger.Debug("Reloading service {Service}", serviceInstance.GetType().Name);
                    await reloadableService.ReloadAsync();

                    await serviceInstance.LoadAsync();
                }
            }
        }
    }
}

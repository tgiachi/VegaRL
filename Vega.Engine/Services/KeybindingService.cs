using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;

namespace Vega.Engine.Services;

[VegaService(4)]
public class KeybindingService : BaseDataLoaderVegaService<KeybindingService>, IKeybindingService
{
    private readonly IServiceProvider _serviceProvider;

    public KeybindingService(
        ILogger<KeybindingService> logger, IDataService dataService, IServiceProvider serviceProvider, IMessageBusService messageBusService
    ) : base(logger, dataService, messageBusService)
    {
        _serviceProvider = serviceProvider;
    }
}

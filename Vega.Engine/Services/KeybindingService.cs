using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

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

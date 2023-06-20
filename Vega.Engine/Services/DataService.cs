using DaysOfDarkness.Engine.Data.Directories;
using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(1)]
public class DataService : BaseVegaReloadableService<DataService>, IDataService
{
    private readonly IMessageBusService _messageBusService;
    private readonly DirectoriesConfig _directoriesConfig;

    public DataService(ILogger<DataService> logger, IMessageBusService messageBusService, DirectoriesConfig directoriesConfig) : base(logger)
    {
        _messageBusService = messageBusService;
        _directoriesConfig = directoriesConfig;
    }

}

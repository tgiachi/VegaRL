using DaysOfDarkness.Engine.Data.Directories;
using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(10)]
public class SaveLoadService : BaseVegaService<SaveLoadService>, ISaveLoadService
{
    private readonly DirectoriesConfig _directoriesConfig;

    public SaveLoadService(ILogger<SaveLoadService> logger, DirectoriesConfig directoriesConfig, IMessageBusService messageBusService) : base(logger, messageBusService)
    {
        _directoriesConfig = directoriesConfig;
    }

    public override Task<bool> LoadAsync()
    {

        return Task.FromResult(true);
    }
}

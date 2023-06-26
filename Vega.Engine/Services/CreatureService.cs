using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(4)]
public class CreatureService : BaseDataLoaderVegaService<CreatureService>, ICreatureService
{
    public CreatureService(ILogger<CreatureService> logger, IDataService dataService, IMessageBusService messageBusService) : base(logger, dataService, messageBusService)
    {
    }
}

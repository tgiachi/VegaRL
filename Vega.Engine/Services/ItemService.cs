using Microsoft.Extensions.Logging;
using Vega.Api.Attributes;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;


[VegaService(5)]
public class ItemService : BaseDataLoaderVegaService<ItemService>, IItemService
{
    public ItemService(ILogger<ItemService> logger, IDataService dataService, IMessageBusService messageBusService) : base(logger, dataService, messageBusService)
    {
    }
}

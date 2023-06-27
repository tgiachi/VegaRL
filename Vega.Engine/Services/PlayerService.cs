using Microsoft.Extensions.Logging;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Items;
using Vega.Framework.Data.Entities.Player;

namespace Vega.Engine.Services;

[VegaService(10)]
public class PlayerService : BaseDataLoaderVegaService<PlayerService>, IPlayerService
{
    private readonly List<ItemEntity> _startingItems = new();
    private readonly IItemService _itemService;

    public PlayerService(
        ILogger<PlayerService> logger, IMessageBusService messageBusService, IDataService dataService,
        IItemService itemService
    ) : base(
        logger,
        dataService,
        messageBusService
    )
    {
        _itemService = itemService;
    }

    public override Task<bool> LoadAsync()
    {
        foreach (var playerInfo in LoadData<PlayerInfoEntity>())
        {
            if (playerInfo.ItemGroupId != null)
            {
                var itemGroup = _itemService.GetItemsFromItemGroupId(playerInfo.ItemGroupId);
                _startingItems.AddRange(itemGroup);
            }
        }

        return Task.FromResult(true);
    }
}

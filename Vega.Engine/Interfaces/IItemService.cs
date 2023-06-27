using System.Collections;
using SadRogue.Primitives;
using Vega.Api.Data.Entities.Items;
using Vega.Api.Map.GameObjects.Items;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface IItemService : IVegaService
{
    ItemGameObject CreateItemGameObject(string itemId, Point position);
    IEnumerable<ItemEntity> GetItemsFromDrop(List<ItemDropEntity> itemDrops);
    IEnumerable<ItemEntity> GetItemsFromItemGroupId(string itemGroupId);
    ItemEntity GetItem(string itemId);
    IEnumerable<ItemEntity> FindItemsByCategory(string category, string? subCategory = null);
}

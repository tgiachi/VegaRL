using System.Collections;
using SadRogue.Primitives;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Data.Entities.Items;
using Vega.Framework.Map.GameObjects.Items;

namespace Vega.Engine.Interfaces;

public interface IItemService : IVegaService
{
    ItemGameObject CreateItemGameObject(string itemId, Point position);
    IEnumerable<ItemGameObject> CreateItemGameObjects(IEnumerable<ItemEntity> items);
    IEnumerable<ItemEntity> GetItemsFromDrop(List<ItemDropEntity> itemDrops);
    IEnumerable<ItemEntity> GetItemsFromItemGroupId(string itemGroupId);
    ItemEntity GetItem(string itemId);
    IEnumerable<ItemEntity> FindItemsByCategory(string category, string? subCategory = null);
}

using SadRogue.Primitives;
using Vega.Api.Data.Entities.Items;
using Vega.Api.Map.GameObjects.Items;
using Vega.Engine.Interfaces.Services;

namespace Vega.Engine.Interfaces;

public interface IItemService : IVegaService
{
    ItemGameObject GetItemGameObject(string itemId, Point position);
    List<ItemEntity> GetItemsFromDrop(List<ItemDropEntity> itemDrops);
    List<ItemEntity> GetItemsFromItemGroupId(string itemGroupId);
}

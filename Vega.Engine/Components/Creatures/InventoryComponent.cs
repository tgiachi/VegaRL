using SadRogue.Integration.Components;

using Vega.Framework.Map.GameObjects.Creatures;
using Vega.Framework.Map.GameObjects.Items;

namespace Vega.Engine.Components.Creatures;

public class InventoryComponent : RogueLikeComponentBase<CreatureGameObject>
{
    public readonly List<ItemGameObject> Inventory = new();

    public InventoryComponent(List<ItemGameObject> items) : base(false, false, false, false)
    {
        Inventory = items;
    }
}

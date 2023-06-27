﻿using SadRogue.Integration.Components;
using Vega.Api.Data.Entities.Items;
using Vega.Api.Map.GameObjects.Creatures;

namespace Vega.Engine.Components.Creatures;

public class InventoryComponent : RogueLikeComponentBase<CreatureGameObject>
{
    public readonly List<ItemEntity> Inventory = new();

    public InventoryComponent(List<ItemEntity> items) : base(false, false, false, false)
    {
        Inventory = items;
    }
}

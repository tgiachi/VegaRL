using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Items;

[EntityData("item_group")]
public class ItemGroupEntity : BaseEntity
{
    public RandomBagEntity Items { get; set; } = new();
}

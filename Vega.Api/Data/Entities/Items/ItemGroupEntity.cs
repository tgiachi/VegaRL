using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Items;

[EntityData("item_group")]
public class ItemGroupEntity : BaseEntity
{
    public List<ItemDropEntity> Items { get; set; } = new();
}

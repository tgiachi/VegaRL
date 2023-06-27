using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Furniture;

[EntityData("furniture_group")]
public class FurnitureGroupEntity : BaseEntity
{
    public List<string> Ids { get; set; } = new();
}

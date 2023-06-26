using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Furniture;

[EntityData("furniture_group")]
public class FurnitureGroupEntity : BaseEntity
{
    public List<string> Ids { get; set; } = new();
}

using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Furniture;
[EntityData("furniture_class")]
public class FurnitureClassEntity : BaseEntity
{

    public Dictionary<string, PropEntity>? Container { get; set; }
}

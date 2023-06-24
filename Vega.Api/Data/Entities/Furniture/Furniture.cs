using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;
using Vega.Api.Data.Entities.Items;
using Vega.Api.Interfaces.Entities;

namespace Vega.Api.Data.Entities.Furniture;

[EntityData("furniture")]
public class FurnitureEntity : BaseEntity, IHasTile, IHasCategory
{
    public string? Sym { get; set; }
    public string? Background { get; set; }
    public string? Foreground { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsTransparent { get; set; }

    public string Category { get; set; }

    public string? SubCategory { get; set; }

    public double Weight { get; set; }

    public List<ItemDropEntity>? Container { get; set; }


}

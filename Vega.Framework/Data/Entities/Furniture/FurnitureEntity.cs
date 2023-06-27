using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;
using Vega.Framework.Interfaces.Entities;

namespace Vega.Framework.Data.Entities.Furniture;

[EntityData("furniture")]
public class FurnitureEntity : BaseEntity, IHasTile, IHasCategory
{
    public string? Sym { get; set; }
    public string? Background { get; set; }
    public string? Foreground { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsTransparent { get; set; }

    public string? FurnitureClassId { get; set; } = null!;

    public string Category { get; set; }

    public string? SubCategory { get; set; }

    public double Weight { get; set; }

    public RandomBagEntity? Container { get; set; }

}

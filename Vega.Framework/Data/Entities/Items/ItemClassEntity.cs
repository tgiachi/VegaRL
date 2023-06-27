using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;
using Vega.Framework.Interfaces.Entities;

namespace Vega.Framework.Data.Entities.Items;

[EntityData("item_class")]
public class ItemClassEntity : BaseEntity, IHasTile, IHasCategory
{
    public string? Sym { get; set; }
    public string? Background { get; set; }
    public string? Foreground { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsTransparent { get; set; }
    public string Category { get; set; }
    public string? SubCategory { get; set; }
}

using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;
using Vega.Framework.Data.Entities.Names;
using Vega.Framework.Interfaces.Entities;

namespace Vega.Framework.Data.Entities.Creatures;

[EntityData("creature")]
public class CreatureEntity : BaseEntity, IHasTile
{
    public string? Sym { get; set; }
    public string? Background { get; set; }
    public string? Foreground { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsTransparent { get; set; }
    public GenderTypeEnum GenderType { get; set; } = GenderTypeEnum.None;
    public string? CreatureClassId { get; set; }
    public Dictionary<string, PropEntity> Stats { get; set; } = new();
    public string? ItemGroupId { get; set; }
    public string? BehaviorTreeId { get; set; }
}

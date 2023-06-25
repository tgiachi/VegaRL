using Vega.Api.Data.Entities.Base;
using Vega.Api.Data.Entities.Names;
using Vega.Api.Interfaces.Entities;

namespace Vega.Api.Data.Entities.Creatures;

public class CreatureEntity : BaseEntity, IHasTile
{
    public string? Sym { get; set; }
    public string? Background { get; set; }
    public string? Foreground { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsTransparent { get; set; }

    public GenderTypeEnum GenderTypeEnum { get; set; } = GenderTypeEnum.Neutral;

    public string? CreatureClassId { get; set; }

    public Dictionary<string, PropEntity> Stats { get; set; } = new();

    public string ItemGroupId { get; set; }

    public string? BehaviorTreeId { get; set; }
}

using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Creatures;

[EntityData("creature_group")]
public class CreatureGroupEntity  : BaseEntity
{
    public RandomBagEntity Creatures { get; set; } = new();
}

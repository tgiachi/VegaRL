using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Creatures;

[EntityData("creature_group")]
public class CreatureGroupEntity  : BaseEntity
{
    public RandomBagEntity Creatures { get; set; } = new();
}

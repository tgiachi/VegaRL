using Vega.Api.Data.Entities.Skills;

namespace Vega.Api.Data.Entities.Stats;

public interface IHaveSkill
{
    List<SkillEntity> Skills { get; set; } 
}

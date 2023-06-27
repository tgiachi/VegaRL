using Vega.Framework.Data.Entities.Skills;

namespace Vega.Framework.Data.Entities.Stats;

public interface IHaveSkill
{
    List<SkillEntity> Skills { get; set; }
}

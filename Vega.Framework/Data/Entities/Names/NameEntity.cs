using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Names;
[EntityData("name_def")]
public class NameEntity : BaseEntity
{
    public NameTypeEnum Usage { get; set; }
    public GenderTypeEnum Gender { get; set; } = GenderTypeEnum.None;

    public List<string> Names { get; set; }
}

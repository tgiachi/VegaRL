using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Names;
[EntityData("name_def")]
public class NameEntity : BaseEntity
{
    public NameTypeEnum Usage { get; set; }
    public GenderTypeEnum Gender { get; set; } = GenderTypeEnum.None;

    public List<string> Names { get; set; }
}

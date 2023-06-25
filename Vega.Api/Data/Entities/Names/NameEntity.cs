using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Names;
[EntityData("names")]
public class NameEntity : BaseEntity
{
    public NameTypeEnum NameType { get; set; }
    public GenderTypeEnum Gender { get; set; }

    public List<string> Names { get; set; }
}

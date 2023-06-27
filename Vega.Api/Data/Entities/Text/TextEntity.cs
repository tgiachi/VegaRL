using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Text;


[EntityData("text_def")]
public class TextEntity : BaseEntity
{
    public List<string> Lines { get; set; } = new();
}

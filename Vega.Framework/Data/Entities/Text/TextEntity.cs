using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Text;


[EntityData("text_def")]
public class TextEntity : BaseEntity
{
    public List<string> Lines { get; set; } = new();
}

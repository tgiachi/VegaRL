using SadRogue.Primitives;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.ColorSchema;

[EntityData("color_def")]
public class ColorSchemaEntity : BaseEntity
{
    public Dictionary<string, int[]> Colors { get; set; } = new();
    public override string ToString() => $" {nameof(Colors)}: {Colors}, {base.ToString()}";

    public Dictionary<string, Color> GetColors()
    {
        var colors = new Dictionary<string, Color>();
        foreach (var c in Colors)
        {
            if (c.Value.Length == 3)
            {
                colors.Add(c.Key.ToUpper(), new Color(c.Value[0], c.Value[1], c.Value[2]));
            }
            else if (c.Value.Length == 4)
            {
                colors.Add(c.Key.ToUpper(), new Color(c.Value[0], c.Value[1], c.Value[2], c.Value[3]));
            }
        }

        return colors;
    }
}

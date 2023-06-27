using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;
using Vega.Framework.Interfaces.Entities;

namespace Vega.Framework.Data.Entities.Terrain;

[EntityData("terrain_def")]
public class TerrainEntity : BaseEntity, IHasTile
{
    public string? Sym { get; set; }
    public string? Background { get; set; }
    public string? Foreground { get; set; }

    public int? MoveCost { get; set; } = 1;
    public bool IsWalkable { get; set; }
    public bool IsTransparent { get; set; }

    public bool IsBreakable { get; set; }

    public TerrainSmashEntity Smash { get; set; } = new();

    public void SanitizeWorldFlags()
    {
        if (HasFlag("WORLDMAP"))
        {
            Flags?.Remove("WORLDMAP");
        }

        Flags.RemoveAll(e => e.StartsWith("VAL="));
    }
}

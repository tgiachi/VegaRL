using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;
using Vega.Api.Interfaces.Entities;
using Vega.Api.Interfaces.Entities.Base;

namespace Vega.Api.Data.Entities.Terrain;

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
}

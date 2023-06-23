using Vega.Api.Attributes;
using Vega.Api.Interfaces.Entities;
using Vega.Api.Interfaces.Entities.Base;

namespace Vega.Api.Data.Entities.Terrain;

[EntityData("terrain_def")]
public class TerrainEntity : IBaseEntity, IHasTileEntity
{
    public string Id { get; set; }

    public string? Description { get; set; }

    public string? Name { get; set; }

    public string? Comment { get; set; }

    public List<string>? Flags { get; set; } = new();
    public string? TileId { get; set; }
    public string? Sym { get; set; }
    public string? Background { get; set; }
    public string? Foreground { get; set; }

    public int? MoveCost { get; set; } = 1;

    public bool IsWalkable { get; set; }

    public bool IsTransparent { get; set; }

    public bool IsBreakable { get; set; }

    public TerrainSmashEntity Smash { get; set; } = new();
}

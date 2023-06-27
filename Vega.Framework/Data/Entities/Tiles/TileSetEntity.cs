using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Tiles;

[EntityData("tile_set_def")]
public class TileSetEntity : BaseEntity
{
    public string Font { get; set; }
    public List<TileMapEntity> Map { get; set; } = new();

    public override string ToString() =>
        $"{base.ToString()}, {nameof(Map)}: {Map.Count}";
}

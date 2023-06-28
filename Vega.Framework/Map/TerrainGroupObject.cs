using Vega.Framework.Map.GameObjects.World;

namespace Vega.Framework.Map;

public class TerrainGroupObject
{
    public string TileType { get; set; }
    public List<TerrainWorldGameObject> Tiles { get; set; } = new();
}

using Vega.Framework.Map.WorldMap.GameObjects;

namespace Vega.Framework.Map;

public class TerrainGroupObject
{
    public string TileType { get; set; }
    public List<TerrainWorldGameObject> Tiles { get; set; } = new();
}

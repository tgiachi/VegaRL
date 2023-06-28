using SadRogue.Primitives;
using Vega.Framework.Map.WorldMap.GameObjects;

namespace Vega.Framework.Map;

public class TerrainGroupObject
{
    public string TileType { get; set; }
    public List<TerrainWorldGameObject> Tiles { get; set; } = new();

    public Rectangle Rectangle => GetRectangle();

    private Rectangle GetRectangle()
    {
        var minX = Tiles.Min(t => t.Position.X);
        var minY = Tiles.Min(t => t.Position.Y);
        var maxX = Tiles.Max(t => t.Position.X);
        var maxY = Tiles.Max(t => t.Position.Y);
        return new Rectangle(new Point(minX, minY), new Point(maxX, maxY));
    }

    public override string ToString() => $"{TileType} {Tiles.Count} Area: {Rectangle.Area}";
}

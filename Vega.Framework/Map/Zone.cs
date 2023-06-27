using SadRogue.Primitives;

namespace Vega.Framework.Map;

public class Zone
{
    public Point StartPosition { get; set; }

    public Point EndPosition { get; set; }

    public List<Point> GetArea()
    {
        var points = new List<Point>();
        for (var x = StartPosition.X; x <= EndPosition.X; x++)
        {
            for (var y = StartPosition.Y; y <= EndPosition.Y; y++)
            {
                points.Add(new Point(x, y));
            }
        }

        return points;
    }

    public override string ToString() => $"Zone: {StartPosition} - {EndPosition}";
}

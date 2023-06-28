using System.Numerics;
using SadRogue.Primitives;

namespace Vega.Framework.Utils;

public static class PointUtils
{
    public static int ManhattanDistance(this Point p1, Point p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

    public static Point Min(this Point p1, Point p2)
    {
        int minX = Math.Min(p1.X, p2.X);
        int minY = Math.Min(p1.Y, p2.Y);
        return new Point(minX, minY);
    }

    public static Point Max(this Point p1, Point p2)
    {
        int maxX = Math.Max(p1.X, p2.X);
        int maxY = Math.Max(p1.Y, p2.Y);
        return new Point(maxX, maxY);
    }

    public static int Magnitude(this Point p) => (int)Math.Sqrt(p.X*p.X+p.Y*p.Y);

    public static int Magnitude(this Vector2 p) => (int)Math.Sqrt(p.X*p.X+p.Y*p.Y);
}

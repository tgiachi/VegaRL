using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using Vega.Framework.Map;

namespace Vega.Framework.Utils;

public class ZoneFinder
{
    private readonly RogueLikeMap _map;

    public ZoneFinder(RogueLikeMap map)
    {
        _map = map;
    }

    private static IEnumerable<Point> GetAdjacentPoints(Point point)
    {
        var adjacentPoints = new Point[]
        {
            new Point(point.X, point.Y - 1), // Up
            new Point(point.X, point.Y + 1), // Down
            new Point(point.X - 1, point.Y), // Left
            new Point(point.X + 1, point.Y)  // Right
        };

        return adjacentPoints;
    }

    public Zone FindZone(Point centralPoint, int radius)
    {
        HashSet<Point> visited = new HashSet<Point>();

        if (!IsValidPoint(centralPoint) || _map.GetTerrainAt(centralPoint).IsWalkable)
            return null;

        Queue<Point> queue = new Queue<Point>();
        queue.Enqueue(centralPoint);
        visited.Add(centralPoint);

        Point startPosition = centralPoint;
        Point endPosition = centralPoint;

        while (queue.Count > 0)
        {
            Point currentPoint = queue.Dequeue();

            if (!IsValidPoint(currentPoint) || !_map.GetTerrainAt(currentPoint).IsWalkable)
                continue;

            if (currentPoint.ManhattanDistance(centralPoint) <= radius)
            {
                startPosition = startPosition.Min(currentPoint);
                endPosition = endPosition.Max(currentPoint);

                var adjacentPoints = GetAdjacentPoints(currentPoint);
                foreach (Point adjacentPoint in adjacentPoints)
                {
                    if (!visited.Contains(adjacentPoint))
                    {
                        queue.Enqueue(adjacentPoint);
                        visited.Add(adjacentPoint);
                    }
                }
            }
        }

        return new Zone { StartPosition = startPosition, EndPosition = endPosition };
    }

    private Zone ExpandZone(Point startPoint, ISet<Point> visited)
    {
        Zone zone = new Zone
        {
            StartPosition = startPoint,
            EndPosition = startPoint
        };

        Queue<Point> queue = new Queue<Point>();
        queue.Enqueue(startPoint);

        while (queue.Count > 0)
        {
            Point currentPoint = queue.Dequeue();

            if (!IsValidPoint(currentPoint) || !_map.GetTerrainAt(currentPoint).IsWalkable || visited.Contains(currentPoint))
            {
                continue;
            }

            visited.Add(currentPoint);
            zone.EndPosition = currentPoint;

            // Add adjacent points to queue
            IEnumerable<Point> adjacentPoints = GetAdjacentPoints(currentPoint);
            foreach (Point adjacentPoint in adjacentPoints)
            {
                queue.Enqueue(adjacentPoint);
            }
        }

        return zone;
    }

    private bool IsValidPoint(Point point) => point.X >= 0 && point.X < _map.Width && point.Y >= 0 && point.Y < _map.Height;
}

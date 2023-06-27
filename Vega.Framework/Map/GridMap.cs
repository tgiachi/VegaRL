using SadRogue.Integration.Maps;
using SadRogue.Primitives;

namespace Vega.Framework.Map;

public class GridMap : RogueLikeMap
{
    public Point GridPosition { get; set; }

    public GridMap(
        int width, int height,
        Point gridPosition
    ) : base(
        width,
        height,
        null,
        Enum.GetValues<WorldMapLayerType>().Length,
        DefaultMapParams.Measurement,
        useCachedGridViews: true
    )
    {
        GridPosition = gridPosition;
    }
}

using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace Vega.Framework.Map.WorldMap.GameObjects;

public class CloudWorldGameObject : RogueLikeEntity
{
    public CloudWorldGameObject(Point position,
        ColoredGlyph appearance, bool walkable = true, bool transparent = true
    ) : base(appearance, walkable, transparent, (int)WorldMapLayerType.Weather)
    {
        Position = position;
    }
}

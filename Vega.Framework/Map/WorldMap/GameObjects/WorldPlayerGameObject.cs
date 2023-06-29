using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace Vega.Framework.Map.WorldMap.GameObjects;

public class WorldPlayerGameObject : RogueLikeEntity
{
    public WorldPlayerGameObject(
        string landId,
        Point position,
        ColoredGlyph appearance, bool walkable = true, bool transparent = true
    ) : base(appearance, walkable, transparent, (int)WorldMapLayerType.Player)
    {
        Position = position;
    }
}

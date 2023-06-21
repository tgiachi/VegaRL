using SadConsole;
using SadRogue.Integration;

namespace Vega.Api.Map.GameObjects;

public class BaseTerrainGameObject : RogueLikeCell
{
    public BaseTerrainGameObject(
        ColoredGlyph appearance, int layer, bool walkable = true, bool transparent = true
    ) : base(appearance, (int)MapLayerType.Terrain, walkable, transparent)
    {
    }
}

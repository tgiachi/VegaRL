using SadConsole;
using Vega.Framework.Map.GameObjects.Terrain.Base;

namespace Vega.Framework.Map.GameObjects.Vegetation;

public class VegetationGameObject : BaseTerrainGameObject
{
    public VegetationGameObject(string terrainId, ColoredGlyph appearance, bool walkable = true, bool transparent = true) :
        base(terrainId, appearance, (int)MapLayerType.Vegetation, walkable, transparent)
    {
    }
}

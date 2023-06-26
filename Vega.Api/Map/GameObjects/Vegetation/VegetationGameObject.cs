using SadConsole;

using Vega.Api.Map.GameObjects.Terrain.Base;

namespace Vega.Api.Map.GameObjects.Vegetation;

public class VegetationGameObject : BaseTerrainGameObject
{
    public VegetationGameObject(string terrainId, ColoredGlyph appearance, bool walkable = true, bool transparent = true) :
        base(terrainId, appearance, (int)MapLayerType.Vegetation, walkable, transparent)
    {
    }
}

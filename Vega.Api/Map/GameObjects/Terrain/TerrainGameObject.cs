using SadConsole;
using Vega.Api.Map.GameObjects.Terrain.Base;

namespace Vega.Api.Map.GameObjects.Terrain;

public class TerrainGameObject : BaseTerrainGameObject
{
    public TerrainGameObject(string terrainId, ColoredGlyph appearance, bool walkable = true, bool transparent = true) :
        base(terrainId, appearance, (int)MapLayerType.Terrain, walkable, transparent)
    {
    }
}

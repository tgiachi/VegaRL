using SadRogue.Primitives;
using Vega.Framework.Map;
using Vega.Framework.Map.GameObjects.World;


namespace Vega.Framework.Utils;

public static class WorldMapUtils
{
    public static TerrainWorldGameObject GetTop(this WorldMap map, TerrainWorldGameObject tile, Point mapSize) =>
        map.GetTerrainAt<TerrainWorldGameObject>(
            new Point(
                tile.Position.X,
                MathsHelper.Mod(tile.Position.Y - 1, mapSize.Y)
            )
        );

    public static TerrainWorldGameObject GetBottom(this WorldMap map, TerrainWorldGameObject tile, Point mapSize) =>
        map.GetTerrainAt<TerrainWorldGameObject>(
            new Point(
                tile.Position.X,
                MathsHelper.Mod(tile.Position.Y + 1, mapSize.Y)
            )
        );

    public static TerrainWorldGameObject GetLeft(this WorldMap map, TerrainWorldGameObject tile, Point mapSize) =>
        map.GetTerrainAt<TerrainWorldGameObject>(
            new Point(
                MathsHelper.Mod(tile.Position.X - 1, mapSize.X),
                tile.Position.Y
            )
        );

    public static TerrainWorldGameObject GetRight(this WorldMap map, TerrainWorldGameObject tile, Point mapSize) =>
        map.GetTerrainAt<TerrainWorldGameObject>(
            new Point(
                MathsHelper.Mod(tile.Position.X + 1, mapSize.X),
                tile.Position.Y
            )
        );
}

using SadRogue.Primitives;
using Vega.Framework.Map.GameObjects.World;

namespace Vega.Framework.Map.Rivers;

public class River
{
    public int Length;
    public List<TerrainWorldGameObject> Tiles { get; set; } = new();
    public int Id { get; set; }

    public int Intersections;
    public float TurnCount;
    public Direction CurrentDirection;

    public River(int id)
    {
        Id = id;

    }

    public void AddTile(TerrainWorldGameObject tile)
    {
        tile.SetRiverPath (this);
        Tiles.Add (tile);
    }
}

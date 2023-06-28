using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;
using Vega.Framework.Map.Biomes;
using Vega.Framework.Map.HeatMap;
using Vega.Framework.Map.Rivers;

namespace Vega.Framework.Map.GameObjects.World;

public class TerrainWorldGameObject : RogueLikeCell
{
    public List<string> Flags { get; set; } = new();

    public List<River> Rivers { get; set; } = new();

    public int RiverSize { get; set; }
    public HeatType HeatType { get; set; }
    public MoistureType MoistureType { get; set; }

    public BiomeType BiomeType { get; set; }

    public float MoistureValue { get; set; }

    public int Bitmask { get; set; }

    public bool FloodFilled { get; set; }
    public TerrainWorldGameObject Left { get; set; }
    public TerrainWorldGameObject Right { get; set; }
    public TerrainWorldGameObject Top { get; set; }
    public TerrainWorldGameObject Bottom { get; set; }

    public float HeightValue { get; set; }
    public string HeightType { get; set; }

    public TerrainWorldGameObject(
        Point position,
        ColoredGlyph appearance, bool walkable, bool transparent, List<string> flags
    ) : base(appearance, (int)WorldMapLayerType.Terrain, walkable, transparent)
    {
        Position = position;
        Flags = flags;
        HeightType = Flags.First();
    }

    public void UpdateBitmask()
    {
        int count = 0;

        if (Top.HeightType == HeightType)
            count += 1;
        if (Right.HeightType == HeightType)
            count += 2;
        if (Bottom.HeightType == HeightType)
            count += 4;
        if (Left.HeightType == HeightType)
            count += 8;

        Bitmask = count;
    }

    public int GetRiverNeighborCount(River river)
    {
        int count = 0;
        if (Left.Rivers.Count > 0 && Left.Rivers.Contains(river))
        {
            count++;
        }

        if (Right.Rivers.Count > 0 && Right.Rivers.Contains(river))
        {
            count++;
        }

        if (Top.Rivers.Count > 0 && Top.Rivers.Contains(river))
        {
            count++;
        }

        if (Bottom.Rivers.Count > 0 && Bottom.Rivers.Contains(river))
        {
            count++;
        }

        return count;
    }

    public Direction GetLowestNeighbor()
    {
        if (Left.HeightValue < Right.HeightValue && Left.HeightValue < Top.HeightValue &&
            Left.HeightValue < Bottom.HeightValue)
        {
            return Direction.Left;
        }

        if (Right.HeightValue < Left.HeightValue && Right.HeightValue < Top.HeightValue &&
            Right.HeightValue < Bottom.HeightValue)
        {
            return Direction.Right;
        }

        if (Top.HeightValue < Left.HeightValue && Top.HeightValue < Right.HeightValue &&
            Top.HeightValue < Bottom.HeightValue)
        {
            return Direction.Right;
        }

        if (Bottom.HeightValue < Left.HeightValue && Bottom.HeightValue < Top.HeightValue &&
            Bottom.HeightValue < Right.HeightValue)
        {
            return Direction.Right;
        }

        return Direction.Down;
    }

    public void SetRiverPath(River river)
    {
        if (!IsWalkable)
            return;

        if (!Rivers.Contains(river))
        {
            Rivers.Add(river);
        }
    }

    private void SetRiverTile(River river)
    {
        SetRiverPath(river);
        HeightType = "RIVER";
        HeightValue = 0;
        IsWalkable = false;
    }

    public void DigRiver(River river, int size)
    {
        SetRiverTile(river);
        RiverSize = size;

        if (size == 1)
        {
            Bottom.SetRiverTile(river);
            Right.SetRiverTile(river);
            Bottom.Right.SetRiverTile(river);
        }

        if (size == 2)
        {
            Bottom.SetRiverTile(river);
            Right.SetRiverTile(river);
            Bottom.Right.SetRiverTile(river);
            Top.SetRiverTile(river);
            Top.Left.SetRiverTile(river);
            Top.Right.SetRiverTile(river);
            Left.SetRiverTile(river);
            Left.Bottom.SetRiverTile(river);
        }

        if (size == 3)
        {
            Bottom.SetRiverTile(river);
            Right.SetRiverTile(river);
            Bottom.Right.SetRiverTile(river);
            Top.SetRiverTile(river);
            Top.Left.SetRiverTile(river);
            Top.Right.SetRiverTile(river);
            Left.SetRiverTile(river);
            Left.Bottom.SetRiverTile(river);
            Right.Right.SetRiverTile(river);
            Right.Right.Bottom.SetRiverTile(river);
            Bottom.Bottom.SetRiverTile(river);
            Bottom.Bottom.Right.SetRiverTile(river);
        }

        if (size == 4)
        {
            Bottom.SetRiverTile(river);
            Right.SetRiverTile(river);
            Bottom.Right.SetRiverTile(river);
            Top.SetRiverTile(river);
            Top.Right.SetRiverTile(river);
            Left.SetRiverTile(river);
            Left.Bottom.SetRiverTile(river);
            Right.Right.SetRiverTile(river);
            Right.Right.Bottom.SetRiverTile(river);
            Bottom.Bottom.SetRiverTile(river);
            Bottom.Bottom.Right.SetRiverTile(river);
            Left.Bottom.Bottom.SetRiverTile(river);
            Left.Left.Bottom.SetRiverTile(river);
            Left.Left.SetRiverTile(river);
            Left.Left.Top.SetRiverTile(river);
            Left.Top.SetRiverTile(river);
            Left.Top.Top.SetRiverTile(river);
            Top.Top.SetRiverTile(river);
            Top.Top.Right.SetRiverTile(river);
            Top.Right.Right.SetRiverTile(river);
        }
    }

    public override string ToString() =>
        $" {nameof(Position)}: {Position}, {nameof(Appearance)}: {Appearance}, {nameof(IsWalkable)}: {IsWalkable}, {nameof(IsTransparent)}: {IsTransparent}, {nameof(Flags)}: {string.Join(',', Flags)} -- {MoistureType} - {HeightType}";
}

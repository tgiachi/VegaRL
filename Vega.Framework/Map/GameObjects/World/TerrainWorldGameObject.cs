using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace Vega.Framework.Map.GameObjects.World;

public class TerrainWorldGameObject : RogueLikeCell
{
    public List<string> Flags { get; set; } = new();

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

    public override string ToString() =>
        $" {nameof(Position)}: {Position}, {nameof(Appearance)}: {Appearance}, {nameof(IsWalkable)}: {IsWalkable}, {nameof(IsTransparent)}: {IsTransparent}, {nameof(Flags)}: {string.Join(',', Flags)}";
}

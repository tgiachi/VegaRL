using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace Vega.Framework.Map.GameObjects.World;

public class TerrainWorldGameObject : RogueLikeCell
{
    public List<string> Flags { get; set; } = new List<string>();

    public TerrainWorldGameObject(
        Point position,
        ColoredGlyph appearance, bool walkable, bool transparent, List<string> flags
    ) : base(appearance, (int)WorldMapLayerType.Terrain, walkable, transparent)
    {
        Position = position;
        Flags = flags;
    }

    public override string ToString() =>
        $" {nameof(Position)}: {Position}, {nameof(Appearance)}: {Appearance}, {nameof(IsWalkable)}: {IsWalkable}, {nameof(IsTransparent)}: {IsTransparent}, {nameof(Flags)}: {string.Join(',', Flags)}";
}

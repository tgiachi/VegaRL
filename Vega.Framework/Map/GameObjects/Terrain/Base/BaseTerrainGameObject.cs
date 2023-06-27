using SadConsole;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Primitives;

namespace Vega.Framework.Map.GameObjects.Terrain.Base;

public class BaseTerrainGameObject : MemoryAwareRogueLikeCell
{
    public TerrainAppearanceDefinition Appearance { get; }
    public string TerrainId { get; set; }

    public BaseTerrainGameObject(string terrainId,
        ColoredGlyph appearance, int layer, bool walkable = true, bool transparent = true
    ) : base(appearance, layer, walkable, transparent)
    {
        TerrainId = terrainId;
        Appearance = new TerrainAppearanceDefinition(appearance, MakeColorDark(appearance));
    }

    protected ColoredGlyph MakeColorDark(ColoredGlyph coloredGlyph) => new(
        DarkColor(coloredGlyph.Foreground),
        DarkColor(coloredGlyph.Background),
        coloredGlyph.Glyph
    );

    public static Color DarkColor(Color color, float factor = 0.70f)
    {
        byte r = (byte)(color.R * factor);
        byte g = (byte)(color.G * factor);
        byte b = (byte)(color.B * factor);
        byte a = color.A;

        return new Color(r, g, b, a);
    }
}

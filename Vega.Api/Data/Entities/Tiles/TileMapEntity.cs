using Vega.Api.Attributes;
using Vega.Api.Data.Entities.ColorSchema;

namespace Vega.Api.Data.Entities.Tiles;

public class TileMapEntity
{
    public string TileId { get; set; }

    public string Glyph { get; set; }

    [ValidateEntity(typeof(ColorSchemaEntity))]
    public string Foreground { get; set; } = string.Empty;

    [ValidateEntity(typeof(ColorSchemaEntity))]
    public string Background { get; set; } = string.Empty;

    public bool IsAnimated { get; set; }
    public List<int> Frames { get; set; } = new();

    public override string ToString() =>
        $" {nameof(TileId)}: {TileId}, {nameof(Glyph)}: {Glyph}, {nameof(Foreground)}: {Foreground}, {nameof(Background)}: {Background}, {nameof(IsAnimated)}: {IsAnimated}, {nameof(Frames)}: {Frames}";
}

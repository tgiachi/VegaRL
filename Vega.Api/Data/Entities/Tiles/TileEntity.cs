using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;
using Vega.Api.Data.Entities.ColorSchema;

namespace Vega.Api.Data.Entities.Tiles;

[EntityData("tile_def")]
public class TileEntity : BaseEntity
{
    public string Symbol { get; set; } = string.Empty;

    [ValidateEntity(typeof(ColorSchemaEntity))]
    public string Background { get; set; } = string.Empty;

    [ValidateEntity(typeof(ColorSchemaEntity))]
    public string Foreground { get; set; } = string.Empty;

    public bool IsWalkable { get; set; }

    public bool IsTransparent { get; set; }
}

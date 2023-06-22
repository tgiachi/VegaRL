namespace Vega.Api.Interfaces.Entities;

public interface IHasTileEntity
{
    string? TileId { get; set; }
    string? Sym { get; set; }
    string? Background { get; set; }
    string? Foreground { get; set; }
}

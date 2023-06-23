namespace Vega.Api.Interfaces.Entities;

public interface IHasTileEntity
{
    string Id { get; set; }
    string? Sym { get; set; }
    string? Background { get; set; }
    string? Foreground { get; set; }

    bool IsWalkable { get; set; }
    bool IsTransparent { get; set; }

}

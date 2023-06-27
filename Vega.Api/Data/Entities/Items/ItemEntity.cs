using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Base;
using Vega.Api.Interfaces.Entities;

namespace Vega.Api.Data.Entities.Items;

[EntityData("item")]
public class ItemEntity : BaseEntity, IHasTile, IHasCategory
{
    public string? ItemClassId { get; set; } = null!;
    public string? Sym { get; set; }
    public string? Background { get; set; }
    public string? Foreground { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsTransparent { get; set; }
    public string Category { get; set; }
    public string? SubCategory { get; set; }
    public double Weight { get; set; }
    public double Price { get; set; }

    public ItemEquipLocationType EquipLocation { get; set; }

    public override string ToString() =>
        $" {Name} ({Id})  {Sym}  {Background}  {Foreground}  {IsWalkable}  {IsTransparent}  {Category}  {SubCategory}  {Weight}  {Price}  {EquipLocation}  {Comment}  {Flags} ";
}

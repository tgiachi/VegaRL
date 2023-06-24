
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Data.Entities.Items;

public class ItemDropEntity : PropEntity
{
    public string? ItemId { get; set; }
    public string? ItemClassId { get; set; }


    public override string ToString() => $" Item: {ItemId} ({ItemClassId}) {Count} [{Range}]";
}

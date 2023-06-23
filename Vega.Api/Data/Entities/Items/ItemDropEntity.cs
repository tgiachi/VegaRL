using Newtonsoft.Json;

namespace Vega.Api.Data.Entities.Items;

public class ItemDropEntity
{
    public string? ItemId { get; set; }
    public string? ItemClassId { get; set; }
    public int? Count { get; set; }
    public ItemDropRangeEntity? Range { get; set; }

    [JsonProperty("prob")]
    public double? Probability { get; set; }

    public override string ToString() => $" Item: {ItemId} ({ItemClassId}) {Count} [{Range}]";
}

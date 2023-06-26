

using System.Text.Json.Serialization;

namespace Vega.Api.Data.Entities.Base;

public class PropEntity
{
    public int? Count { get; set; }

    [JsonPropertyName("prob")] public double? Probability { get; set; }

    public PropRangeEntity? Range { get; set; }

    public string? Dice { get; set; } = null!;

    public override string ToString() => $" {Count} [{Range}] {Probability} % {Dice}";
}



using System.Text.Json.Serialization;

namespace Vega.Framework.Data.Entities.Base;

public class PropEntity
{
    public int Count { get; set; } = 1;

    [JsonPropertyName("prob")] public double? Probability { get; set; }

    public PropRangeEntity? Range { get; set; }

    public string? Dice { get; set; } = null!;

    public int? Value { get; set; }

    public override string ToString() => $" {Count} [{Range}] {Probability} % {Dice} {Value}";
}

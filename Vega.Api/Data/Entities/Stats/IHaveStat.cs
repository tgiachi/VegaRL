using System.Text.Json.Serialization;

namespace Vega.Api.Data.Entities.Stats;

public interface IHaveStat
{
    [JsonPropertyName("hp")] int Health { get; set; }
    [JsonPropertyName("hp_max")] int MaxHealth { get; set; }
    [JsonPropertyName("str")] int Strength { get; set; }
    [JsonPropertyName("dex")] int Dexterity { get; set; }
    [JsonPropertyName("con")] int Constitution { get; set; }
    [JsonPropertyName("int")] int Intelligence { get; set; }
    [JsonPropertyName("cha")] int Charisma { get; set; }
}

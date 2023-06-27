using System.Text.Json.Serialization;

namespace Vega.Framework.Data.Entities.Stats;

public class BaseStatEntity : IHaveStat
{
    [JsonPropertyName("hp")] public int Health { get; set; }
    [JsonPropertyName("hp_max")] public int MaxHealth { get; set; }
    [JsonPropertyName("str")] public int Strength { get; set; }
    [JsonPropertyName("dex")] public int Dexterity { get; set; }
    [JsonPropertyName("con")] public int Constitution { get; set; }
    [JsonPropertyName("int")] public int Intelligence { get; set; }
    [JsonPropertyName("cha")] public int Charisma { get; set; }
    [JsonPropertyName("att")] public int Attitude { get; set; }
}

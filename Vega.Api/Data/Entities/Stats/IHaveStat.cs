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

    /// <summary>
    ///   The creature's attitude towards the player and other creatures.
    ///   where -100 is hostile, 0 is neutral, and 100 is friendly.
    /// </summary>
    [JsonPropertyName("att")] public int Attitude { get; set; }
}

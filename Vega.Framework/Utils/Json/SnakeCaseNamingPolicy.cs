using System.Text.Json;

namespace Vega.Framework.Utils.Json;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static SnakeCaseNamingPolicy Instance { get; } = new();

    public override string ConvertName(string name) => name.ToUnderscoreCase();
}

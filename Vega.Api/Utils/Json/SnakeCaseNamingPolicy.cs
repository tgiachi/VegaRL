using System.Text.Json;

namespace Vega.Api.Utils.Json;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static SnakeCaseNamingPolicy Instance { get; } = new();

    public override string ConvertName(string name) => name.ToUnderscoreCase();
}

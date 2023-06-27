using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vega.Framework.Utils.Json;

public class JsonSerializerUtility
{
    public static JsonSerializerOptions DefaultOptions => new()
    {
        PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
        WriteIndented = true
    };
}

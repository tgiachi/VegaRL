using System.Text.Json;
using System.Text.Json.Serialization;
using Vega.Api.Utils.Json;

namespace Vega.Api.Utils;

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

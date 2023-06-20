using System.Text.Json;
using System.Text.Json.Nodes;


namespace Vega.Api.Utils.Json;

public class JsonSerializerInstance
{
    private const string _typePropertyName = "$type";

    public static TEntity Deserialize<TEntity>(string json) =>
        JsonSerializer.Deserialize<TEntity>(json, JsonSerializerUtility.DefaultOptions);

    public static Dictionary<string, List<string>> ParseDataType(string json, Dictionary<string, Type> mappedTypes)
    {
        var results = new Dictionary<string, List<string>>();
        var node = JsonSerializer.Deserialize<JsonNode>(json, JsonSerializerUtility.DefaultOptions);
        foreach (var item in node.AsArray())
        {
            if (item != null)
            {
                var itemType = item[_typePropertyName].ToString();
                if (!results.ContainsKey(itemType))
                {
                    results.Add(itemType, new List<string>());
                }

                results[itemType].Add(item.ToString());
            }
        }


        return results;
    }
}

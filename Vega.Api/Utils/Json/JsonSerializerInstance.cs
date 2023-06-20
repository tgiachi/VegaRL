using System.Text.Json;


namespace Vega.Api.Utils.Json;

public class JsonSerializerInstance
{
    private const string _typePropertyName = "$type";

    public TEntity Deserialize<TEntity>(string json) =>
        JsonSerializer.Deserialize<TEntity>(json, JsonSerializerUtility.DefaultOptions);

    public Dictionary<Type, List<string>> ParseDataType(string json, Dictionary<string, Type> mappedTypes)
    {
        var results = new Dictionary<Type, List<string>>();
        var node = JsonSerializer.SerializeToNode(json, JsonSerializerUtility.DefaultOptions);
        var jsonArray = node.AsArray();

        if (json == null)
        {
            throw new Exception("Json is not an array");
        }

        foreach (var item in jsonArray)
        {
            if (item[_typePropertyName] == null)
            {
                throw new Exception($"Json item does not contain {_typePropertyName} property");
            }

            var type = mappedTypes[item[_typePropertyName].ToJsonString()];

            if (!results.ContainsKey(type))
            {
                results[type] = new List<string>();
            }

            results[type].Add(item.ToJsonString());
        }

        return results;
    }
}

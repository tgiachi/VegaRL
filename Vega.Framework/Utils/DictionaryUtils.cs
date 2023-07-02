namespace Vega.Framework.Utils;

public static class DictionaryUtils
{
    public static TValue CheckValueInDictionary<TKey, TValue>(this IDictionary<double, TValue> dictionary, double value)
    {
        foreach (var val in dictionary.Keys)
        {
            if (value <= val )
            {
                return dictionary[val];
            }

        }

        var closestKey = dictionary.Keys.Aggregate((x, y) => Math.Abs(x - value) < Math.Abs(y - value) ? x : y);
        return dictionary[closestKey];
    }
}

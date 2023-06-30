namespace Vega.Framework.Utils;

public static class DictionaryUtils
{
    public static TValue CheckValueInDictionary<TKey, TValue>(this IDictionary<double, TValue> dictionary, double value)
    {
        var closestKey = dictionary.Keys.First();
        var smallestDifference = Math.Abs(value - closestKey);

        foreach (var key in dictionary.Keys)
        {
            var currentDifference = Math.Abs(value - key);
            if (currentDifference < smallestDifference)
            {
                smallestDifference = currentDifference;
                closestKey = key;
            }
        }

        return dictionary[closestKey];
    }
}

using GoRogue.Random;
using Vega.Api.Data.Entities.Base;

namespace Vega.Api.Utils.Random;

public static class RandomUtils
{
    public static int Range(int min, int max) => GlobalRandom.DefaultRNG.NextInt(min, max);
    public static TEntity RandomElement<TEntity>(this IEnumerable<TEntity> enumerable) => enumerable.ElementAt(Range(0, enumerable.Count()));

    public static List<TEntity> RandomWeightedElements<TEntity>(int count, params TEntity[] entities) where TEntity : PropEntity
    {
        var bag = new WeightedRandomBag<TEntity>();
        foreach (var item in entities)
        {
            bag.AddEntry(item, item.Probability ?? 0);
        }

        var result = new List<TEntity>();
        for (var i = 0; i < count; i++)
        {
            result.Add(bag.GetRandom());
        }

        return result;
    }

}

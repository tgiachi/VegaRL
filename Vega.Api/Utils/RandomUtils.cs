using GoRogue.Random;


namespace Vega.Api.Utils;

public static class RandomUtils
{
    public static int Range(int min, int max) => GlobalRandom.DefaultRNG.NextInt(min, max);
    public static TEntity RandomElement<TEntity>(this IEnumerable<TEntity> enumerable) => enumerable.ElementAt(Range(0, enumerable.Count()));

}

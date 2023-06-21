using ShaiRandom.Generators;

namespace Vega.Api.Utils;

public static class RandomUtils
{

    public static int Range(int min, int max) => MaxRandom.Instance.NextInt(min, max);
}

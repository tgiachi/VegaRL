using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoRogue.DiceNotation;
using GoRogue.Random;
using Vega.Api.Data.Entities.Base;
using Vega.Api.Data.Entities.Stats;

namespace Vega.Api.Utils.Random;

public static class RandomUtils
{
    public static int Range(int min, int max) => GlobalRandom.DefaultRNG.NextInt(min, max);

    public static TEntity RandomElement<TEntity>(this IEnumerable<TEntity> enumerable) =>
        enumerable.ElementAt(Range(0, enumerable.Count()));

    public static List<TEntity> RandomWeightedElements<TEntity>(int count, params TEntity[] entities)
        where TEntity : PropEntity
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

    public static List<string> BuildPropEntries(this RandomBagEntity bag)
    {
        var result = new List<string>();
        foreach (var _ in Enumerable.Range(0, bag.Count))
        {
            foreach (var item in bag.Items)
            {
                if (item.Value.Value != null)
                {
                    var value = item.Value.Value.Value;
                    result.AddRange(Enumerable.Range(0, value).Select(_ => item.Key));
                }
                else if (item.Value.Range != null)
                {
                    var range = item.Value.Range;
                    var count = RandomUtils.Range(range.Min, range.Max);
                    result.AddRange(Enumerable.Range(0, count).Select(_ => item.Key));
                }
                else if (item.Value.Dice != null)
                {
                    var dice = item.Value.Dice;
                    var count = Dice.Roll(dice);
                    result.AddRange(Enumerable.Range(0, count).Select(_ => item.Key));
                }
                else if (item.Value.Probability != null)
                {
                    var probability = item.Value.Probability.Value;
                    var count = RandomUtils.Range(0, 100);
                    if (count <= probability)
                    {
                        result.Add(item.Key);
                    }
                }
                else
                {
                    result.Add(item.Key);
                }
            }
        }

        return result;
    }

    public static int RandomRange(this PropRangeEntity range) => Range(range.Min, range.Max);

    public static Dictionary<string, int> BuildPropStatEntities(this Dictionary<string, PropEntity> entities)
    {
        var result = new Dictionary<string, int>();
        foreach (var entity in entities)
        {
            if (entity.Value.Value != null)
            {
                result.Add(entity.Key, entity.Value.Value.Value);
            }
            else if (entity.Value.Dice != null)
            {
                result.Add(entity.Key, Dice.Roll(entity.Value.Dice));
            }
            else if (entity.Value.Range != null)
            {
                result.Add(entity.Key, RandomRange(entity.Value.Range));
            }
            else
            {
                throw new Exception("Invalid prop entity.");
            }
        }

        return result;
    }

    public static BaseStatEntity BuildRandomCreatureStats(this Dictionary<string, PropEntity> entities)
    {
        var stats = entities.BuildPropStatEntities();
        var baseStatEntity = new BaseStatEntity();
        var fields = typeof(BaseStatEntity).GetProperties();
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<JsonPropertyNameAttribute>(true) != null)
            {
                var attr = field.GetCustomAttribute<JsonPropertyNameAttribute>();
                if (stats.TryGetValue(attr.Name.ToLower(), out var value))
                {
                    field.SetValue(baseStatEntity, value);
                }
            }
        }

        baseStatEntity.MaxHealth = baseStatEntity.Health;

        return baseStatEntity;
    }
}

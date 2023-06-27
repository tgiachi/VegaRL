using GoRogue.Random;

namespace Vega.Framework.Utils.Random;

public class WeightedRandomBag<T>
{
    private struct Entry
    {
        public double accumulatedWeight;
        public T item;
    }

    private readonly List<Entry> entries = new();
    private double accumulatedWeight;


    public void AddEntry(T item, double weight)
    {
        accumulatedWeight += weight;
        entries.Add(new Entry { item = item, accumulatedWeight = accumulatedWeight });
    }

    public T GetRandom()
    {
        double r = GlobalRandom.DefaultRNG.NextDouble() * accumulatedWeight;

        foreach (Entry entry in entries)
        {
            if (entry.accumulatedWeight >= r)
            {
                return entry.item;
            }
        }

        return default(T); //should only happen when there are no entries
    }
}

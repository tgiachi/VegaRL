namespace Vega.Api.Data.Entities.Base;

public class RandomBagEntity
{
    public int Count { get; set; }
    public Dictionary<string, PropEntity> Items { get; set; } = new();
}

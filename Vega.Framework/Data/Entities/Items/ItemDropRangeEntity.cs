namespace Vega.Framework.Data.Entities.Items;

public class ItemDropRangeEntity
{
    public int Min { get; set; }
    public int Max { get; set; }

    public override string ToString() => $"Range: {Min} - {Max}";
}

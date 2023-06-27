namespace Vega.Framework.Data.Entities.Base;

public class PropRangeEntity
{
    public int Min { get; set; }
    public int Max { get; set; }

    public override string ToString() => $"Range: {Min} - {Max}";
}

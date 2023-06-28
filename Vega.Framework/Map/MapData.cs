namespace Vega.Framework.Map;

public class MapData
{
    public float[,] Data { get; set; }
    public float Min { get; set; }
    public float Max { get; set; }

    public MapData(int width, int height)
    {
        Data = new float[width, height];
        Min = float.MaxValue;
        Max = float.MinValue;
    }
}

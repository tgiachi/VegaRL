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

    public void AddData(int x, int y, float value)
    {
        Data[x, y] += value;
        if (Data[x, y] < Min)
        {
            Min = Data[x, y];
        }

        if (Data[x, y] > Max)
        {
            Max = Data[x, y];
        }
    }

    public float GetData(int x, int y) => Data[x, y];
}

namespace Vega.Framework.Map.WorldMap.HeatMap;

public class HeatMapUtils
{
    private static readonly float ColdestValue = 0.05f;
    private static readonly float ColderValue = 0.18f;
    private static readonly float ColdValue = 0.4f;
    private static readonly float WarmValue = 0.6f;
    private static readonly float WarmerValue = 0.8f;

    public static HeatType ValueToHeat(float heatValue)
    {
        if (heatValue < ColdestValue)
        {
            return HeatType.Coldest;
        }

        if (heatValue < ColderValue)
        {
            return HeatType.Colder;
        }

        if (heatValue < ColdValue)
        {
            return HeatType.Cold;
        }

        if (heatValue < WarmValue)
        {
            return HeatType.Warm;
        }

        if (heatValue < WarmerValue)
        {
            return HeatType.Warmer;
        }

        return HeatType.Warmest;
    }
}

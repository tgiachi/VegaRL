namespace Vega.Framework.Map.HeatMap;

public class HeatMapUtils
{
    private static readonly float ColdestValue = 0.05f;
    private static readonly float ColderValue = 0.18f;
    private static readonly float ColdValue = 0.4f;
    private static readonly float WarmValue = 0.6f;
    private static readonly float WarmerValue = 0.8f;

    public static HeatMapType ValueToHeat(float heatValue)
    {
        if (heatValue < ColdestValue)
        {
            return HeatMapType.Coldest;
        }

        if (heatValue < ColderValue)
        {
            return HeatMapType.Colder;
        }

        if (heatValue < ColdValue)
        {
            return HeatMapType.Cold;
        }

        if (heatValue < WarmValue)
        {
            return HeatMapType.Warm;
        }

        if (heatValue < WarmerValue)
        {
            return HeatMapType.Warmer;
        }

        return HeatMapType.Warmest;
    }
}

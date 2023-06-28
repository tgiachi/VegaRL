namespace Vega.Framework.Map.WorldMap.HeatMap;

public class MoistureUtils
{
    public const float DryerValue = 0.27f;
    public  const float DryValue = 0.4f;
    public  const float WetValue = 0.6f;
    public  const float WetterValue = 0.8f;
    public  const float WettestValue = 0.9f;

    public static MoistureType GetMoistureType(double moistureValue)
    {
        if (moistureValue < DryerValue)
        {
            return MoistureType.Dryest;
        }

        if (moistureValue < DryValue)
        {
            return MoistureType.Dryer;
        }

        if (moistureValue < WetValue)
        {
            return MoistureType.Dry;
        }

        if (moistureValue < WetterValue)
        {
            return MoistureType.Wet;
        }

        if (moistureValue < WettestValue)
        {
            return MoistureType.Wetter;
        }


        return MoistureType.Wettest;
    }
}

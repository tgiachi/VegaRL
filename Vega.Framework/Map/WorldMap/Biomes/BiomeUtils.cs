using Vega.Framework.Map.GameObjects.World;

namespace Vega.Framework.Map.Biomes;

public class BiomeUtils
{
    public static BiomeType[,] BiomeTable = new BiomeType[6, 6]
    {
        //COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST
        {
            BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland, BiomeType.Desert, BiomeType.Desert, BiomeType.Desert
        }, //DRYEST
        {
            BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland, BiomeType.Desert, BiomeType.Desert, BiomeType.Desert
        }, //DRYER
        {
            BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland, BiomeType.Woodland, BiomeType.Savanna, BiomeType.Savanna
        }, //DRY
        {
            BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland, BiomeType.Savanna, BiomeType.Savanna
        }, //WET
        {
            BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest, BiomeType.TropicalRainforest,
            BiomeType.TropicalRainforest
        }, //WETTER
        {
            BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest,
            BiomeType.TropicalRainforest, BiomeType.TropicalRainforest
        } //WETTEST
    };

    public static BiomeType GetBiomeType(TerrainWorldGameObject tile) => BiomeTable [(int)tile.MoistureType, (int)tile.HeatType];
}

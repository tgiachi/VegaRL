using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Config.WorldMap;

public class WorldMapConfig
{
    public PropEntity NumCities { get; set; } = new () {Range = new(1, 3)};

    public WorldMapNoiseType NoiseType { get; set; } = WorldMapNoiseType.Perlin;
}



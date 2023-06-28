using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Config.WorldMap;

public class WorldMapConfig
{
    public PropEntity NumCities { get; set; } = new () {Range = new(1, 3)};
    
    public PropEntity NumRivers { get; set; } = new() { Range = new(40, 40) };

    public float MinRiverHeight { get; set; } = 0.6f;

    public int MaxRiverAttempts { get; set; } = 10000;

    public int MinRiverLength { get; set; } = 20;

    public int MaxRiverIntersections { get; set; } = 2;

    public int MinRiverTurns { get; set; } = 18;
}



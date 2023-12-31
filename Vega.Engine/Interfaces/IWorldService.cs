﻿using SadRogue.Primitives;
using Vega.Engine.Interfaces.Services;
using Vega.Framework.Data.Config.WorldMap;
using Vega.Framework.Map;
using Vega.Framework.Map.WorldMap;

namespace Vega.Engine.Interfaces;

public interface IWorldService : IVegaService
{
    Point WorldMapSize { get; }
    Point GridMapSize { get; }
    WorldMap CurrentWorldMap { get; set; }
    GridMap CurrentGridMap { get; set; }

    Task<WorldMap> GenerateWorldMapAsync(WorldMapConfig config);
}

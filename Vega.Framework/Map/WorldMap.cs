﻿using SadRogue.Integration.Maps;

namespace Vega.Framework.Map;

public class WorldMap : RogueLikeMap
{
    public WorldMap(
        int width, int height
    ) : base(
        width,
        height,
        null,
        Enum.GetValues<WorldMapLayerType>().Length,
        DefaultMapParams.Measurement,
        useCachedGridViews: true
    )
    {
    }
}
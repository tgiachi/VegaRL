using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.WorldMap;


[EntityData("map_spawn_location")]
public class WorldMapSpawnLocationEntity : BaseEntity
{
    public string Group { get; set; }

    public string TerrainSpawnGroup { get; set; }
}

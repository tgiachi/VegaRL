using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.WorldMap;


[EntityData("land_spawn_group")]
public class LandSpawnGroupEntity : BaseEntity
{
    public List<string> Lands { get; set; } = new();

}

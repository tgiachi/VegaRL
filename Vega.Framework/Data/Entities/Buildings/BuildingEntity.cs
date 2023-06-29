using Vega.Framework.Data.Entities.Base;

namespace Vega.Framework.Data.Entities.Buildings;

public class BuildingEntity : IHasCategory
{
    public string Category { get; set; }

    public string? SubCategory { get; set; }
    public List<string> Rows { get; set; } = new();

    public Dictionary<string, string> Terrain { get; set; }

    public Dictionary<string, IdOrGroupEntity> Furniture { get; set; }

    public PlayerStartLocationEntity? PlayerStartLocation { get; set; }
}

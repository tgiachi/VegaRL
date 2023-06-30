using Vega.Framework.Map.WorldMap.GameObjects;

namespace Vega.Framework.Map.WorldMap;

public class LandZoneObject
{
    public List<LandGameObject> LandGameObjects { get; set; } = new();

    public string Name { get; set; }

    public string Type { get; set; }

    public LandZoneObject(List<LandGameObject> landGameObjects, string name = "", string type = "")
    {
        LandGameObjects = landGameObjects;
        Name = name;
        Type = type;
    }
}

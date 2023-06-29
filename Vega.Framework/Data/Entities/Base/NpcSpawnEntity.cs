namespace Vega.Framework.Data.Entities.Base;

public class NpcSpawnEntity: IIdOrGroupEntity
{
    public int X { get; set; }
    public int Y { get; set; }

    public string? Id { get; set; }
    public string? Group { get; set; }
}

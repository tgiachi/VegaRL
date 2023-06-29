namespace Vega.Framework.Data.Entities.Base;

public class IdOrGroupEntity : IIdOrGroupEntity
{
    public string? Id { get; set; }
    public string? Group { get; set; }

    public override string ToString() => $" {Id} / {Group}";
}

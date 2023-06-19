using Vega.Api.Interfaces.Entities;

namespace Vega.Api.Data.Entities.Base;

public class BaseEntity : IBaseEntity
{
    public string Id { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
    public string? Comment { get; set; }

    public override string ToString() =>
        $" {nameof(Id)}: {Id}, {nameof(Description)}: {Description}, {nameof(Name)}: {Name}, {nameof(Comment)}: {Comment ?? "N/A"}";
}

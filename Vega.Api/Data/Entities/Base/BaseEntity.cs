using Vega.Api.Interfaces.Entities;

namespace Vega.Api.Data.Entities.Base;

/// <summary>
/// <inheritdoc cref="IBaseEntity"/>
/// </summary>
public class BaseEntity : IBaseEntity
{
    public string Id { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
    public string? Comment { get; set; }
    public List<string>? Flags { get; set; }

    public List<string> SearchFlags(params string[] flags)
    {
        if (Flags == null || Flags.Count == 0)
        {
            return new List<string>();
        }

        return flags.Where(flag => Flags.Any(f => string.Equals(f, flag, StringComparison.OrdinalIgnoreCase))).ToList();
    }

    public override string ToString() =>
        $" {nameof(Id)}: {Id}, {nameof(Description)}: {Description}, {nameof(Name)}: {Name}, {nameof(Comment)}: {Comment ?? "N/A"}";
}

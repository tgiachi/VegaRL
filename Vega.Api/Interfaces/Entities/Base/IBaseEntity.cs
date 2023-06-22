using System.Text.Json.Serialization;

namespace Vega.Api.Interfaces.Entities.Base;


/// <summary>
///  Base interface for all serializable entities.
/// </summary>
public interface IBaseEntity
{
    string Id { get; set; }
    string? Description { get; set; }
    string? Name { get; set; }
    [JsonPropertyName("//")] string? Comment { get; set; }
    List<string>? Flags { get; set; }
}

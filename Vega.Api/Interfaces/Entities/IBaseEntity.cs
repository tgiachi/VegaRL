using System.Text.Json.Serialization;

namespace Vega.Api.Interfaces.Entities;

public interface IBaseEntity
{
    string Id { get; set; }
    string? Description { get; set; }
    string? Name { get; set; }
    [JsonPropertyName("//")] string? Comment { get; set; }
}

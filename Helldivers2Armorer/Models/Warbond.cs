using System.Text.Json.Serialization;

namespace Helldivers2Armorer.Models;

public record Warbond
{
    [JsonPropertyName("internalName")]
    public string InternalName { get; init; } = "";

    [JsonPropertyName("displayName")]
    public string DisplayName { get; init; } = "";

    [JsonPropertyName("wikiUrl")]
    public string WikiUrl { get; init; } = "";
}

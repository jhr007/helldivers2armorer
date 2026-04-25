using System.Text.Json.Serialization;

namespace Helldivers2Armorer.Models;

public record ArmorPassive
{
    [JsonPropertyName("displayName")] public string DisplayName { get; init; } = "";
    [JsonPropertyName("internalName")] public string InternalName { get; init; } = "";
    [JsonPropertyName("imageURL")] public string ImageURL { get; init; } = "";
}

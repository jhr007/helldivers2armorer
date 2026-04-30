using System.Text.Json.Serialization;

namespace Helldivers2Armorer.Models;

public record ArmorSet
{
    [JsonPropertyName("displayName")]
    public string DisplayName { get; init; } = "";

    [JsonPropertyName("tags")]
    public string[] Tags { get; init; } = [];

    [JsonPropertyName("armorRating")]
    public int ArmorRating { get; init; }

    [JsonPropertyName("speed")]
    public int Speed { get; init; }

    [JsonPropertyName("stamina")]
    public int Stamina { get; init; }

    [JsonPropertyName("passive")]
    public string Passive { get; init; } = "";

    [JsonPropertyName("internalName")]
    public string InternalName { get; init; } = "";

    [JsonPropertyName("imageURL")]
    public string ImageURL { get; init; } = "";

    [JsonPropertyName("sortOrder")]
    public int SortOrder { get; init; }

    public string WeightClass => Tags.FirstOrDefault(t => t is "Light" or "Medium" or "Heavy") ?? "Unknown";

    [JsonIgnore]
    public ArmorPassive? PassiveInfo { get; init; }
}

using System.Text.Json.Serialization;

namespace Helldivers2Armorer.Models;

public record ArmorSet
{
    [JsonPropertyName("displayName")]
    public string DisplayName { get; init; } = "";

    [JsonPropertyName("weightClass")]
    public string WeightClass { get; init; } = "";

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

    [JsonPropertyName("warbondCode")]
    public string WarbondCode { get; init; } = "";

    [JsonIgnore]
    public ArmorPassive? PassiveInfo { get; init; }

    [JsonIgnore]
    public Warbond? WarbondInfo { get; init; }
}

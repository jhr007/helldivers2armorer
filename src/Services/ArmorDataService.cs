using System.Net.Http.Json;
using System.Text.Json;
using Helldivers2Armorer.Models;

namespace Helldivers2Armorer.Services;

public class ArmorDataService(HttpClient http)
{
    private static readonly JsonSerializerOptions Opts = new() { PropertyNameCaseInsensitive = true };

    public List<ArmorSet> ArmorSets { get; private set; } = [];
    public List<ArmorPassive> Passives { get; private set; } = [];
    public Dictionary<string, List<string>> FeatureTags { get; private set; } = [];
    public IReadOnlyList<string> AllFeatureTags { get; private set; } = [];
    public Dictionary<string, string> PassiveIconMap { get; private set; } = [];
    public bool IsLoaded { get; private set; }

    public async Task LoadAsync()
    {
        if (IsLoaded) return;
        ArmorSets = await http.GetFromJsonAsync<List<ArmorSet>>("data/armorsets.json", Opts) ?? [];
        Passives = await http.GetFromJsonAsync<List<ArmorPassive>>("data/armorpassives.json", Opts) ?? [];
        FeatureTags = await http.GetFromJsonAsync<Dictionary<string, List<string>>>("data/feature-tags.json", Opts) ?? [];
        AllFeatureTags = FeatureTags.Values.SelectMany(x => x).Distinct().OrderBy(x => x).ToList();
        PassiveIconMap = Passives.ToDictionary(p => p.DisplayName, p => p.ImageURL);
        IsLoaded = true;
    }

    public List<ArmorSet> Filter(HashSet<string> passives, HashSet<string> featureTags)
    {
        return ArmorSets.Where(a =>
            (passives.Count == 0 || passives.Contains(a.Passive)) &&
            (featureTags.Count == 0 || featureTags.All(tag =>
                FeatureTags.TryGetValue(a.Passive, out var tags) && tags.Contains(tag)))
        ).ToList();
    }
}

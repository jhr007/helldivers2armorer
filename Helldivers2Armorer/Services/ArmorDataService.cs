using System.Net.Http.Json;
using System.Text.Json;
using Helldivers2Armorer.Models;

namespace Helldivers2Armorer.Services;

public class ArmorDataService(HttpClient http)
{
    private static readonly JsonSerializerOptions Opts = new() { PropertyNameCaseInsensitive = true };

    public List<ArmorSet> ArmorSets { get; private set; } = [];
    public List<ArmorPassive> Passives { get; private set; } = [];
    public Dictionary<string, ArmorPassive> PassiveMap { get; private set; } = [];
    public IReadOnlyList<string> AllFeatureTags { get; private set; } = [];
    public bool IsLoaded { get; private set; }

    public async Task LoadAsync()
    {
        if (IsLoaded) return;
        ArmorSets = await http.GetFromJsonAsync<List<ArmorSet>>("data/armorsets.json", Opts) ?? [];
        Passives = await http.GetFromJsonAsync<List<ArmorPassive>>("data/armorpassives.json", Opts) ?? [];
        PassiveMap = Passives.ToDictionary(p => p.DisplayName);
        AllFeatureTags = Passives.SelectMany(p => p.AbilityTags).Distinct().OrderBy(x => x).ToList();
        ArmorSets = ArmorSets.Select(a => a with { PassiveInfo = PassiveMap.GetValueOrDefault(a.Passive) }).ToList();
        IsLoaded = true;
    }

    public List<ArmorSet> Filter(HashSet<string> passives, HashSet<string> featureTags)
    {
        return ArmorSets.Where(a =>
            (passives.Count == 0 || passives.Contains(a.Passive)) &&
            (featureTags.Count == 0 || featureTags.All(tag =>
                a.PassiveInfo?.AbilityTags.Contains(tag) == true))
        ).ToList();
    }
}

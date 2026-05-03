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
    public Dictionary<string, Warbond> WarbondMap { get; private set; } = [];
    public IReadOnlyList<string> AllFeatureTags { get; private set; } = [];
    public bool IsLoaded { get; private set; }

    public async Task LoadAsync()
    {
        if (IsLoaded)
            return;

        var armorTask   = http.GetFromJsonAsync<List<ArmorSet>>("data/armorsets.json", Opts);
        var passiveTask = http.GetFromJsonAsync<List<ArmorPassive>>("data/armorpassives.json", Opts);
        var warbondTask = http.GetFromJsonAsync<List<Warbond>>("data/warbonds.json", Opts);

        await Task.WhenAll(armorTask, passiveTask, warbondTask);

        Passives   = passiveTask.Result ?? [];
        PassiveMap = Passives.ToDictionary(p => p.DisplayName);

        var warbonds = warbondTask.Result ?? [];
        WarbondMap = warbonds.ToDictionary(w => w.InternalName);

        AllFeatureTags = Passives.SelectMany(p => p.AbilityTags).Distinct().OrderBy(x => x).ToList();

        ArmorSets = (armorTask.Result ?? [])
            .Select(a => a with
            {
                PassiveInfo = PassiveMap.GetValueOrDefault(a.Passive),
                WarbondInfo = WarbondMap.GetValueOrDefault(a.WarbondCode),
            })
            .ToList();

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

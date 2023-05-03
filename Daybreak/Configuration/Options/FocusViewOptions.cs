using Daybreak.Attributes;
using Daybreak.Configuration.FocusView;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Focus View")]
public sealed class FocusViewOptions
{
    [JsonProperty(nameof(Enabled))]
    public bool Enabled { get; set; }

    [JsonProperty(nameof(MemoryReaderFrequency))]
    public double MemoryReaderFrequency { get; set; } = 16;

    [JsonProperty(nameof(EnablePathfinding))]
    public bool EnablePathfinding { get; set; } = true;

    [JsonProperty(nameof(ExperienceDisplay))]
    public ExperienceDisplay ExperienceDisplay { get; set; }

    [JsonProperty(nameof(KurzickPointsDisplay))]
    public PointsDisplay KurzickPointsDisplay { get; set; }

    [JsonProperty(nameof(LuxonPointsDisplay))]
    public PointsDisplay LuxonPointsDisplay { get; set; }

    [JsonProperty(nameof(BalthazarPointsDisplay))]
    public PointsDisplay BalthazarPointsDisplay { get; set; }

    [JsonProperty(nameof(ImperialPointsDisplay))]
    public PointsDisplay ImperialPointsDisplay { get; set; }

    [JsonProperty(nameof(VanquishingDisplay))]
    public PointsDisplay VanquishingDisplay { get; set; }

    [JsonProperty(nameof(HealthDisplay))]
    public PointsDisplay HealthDisplay { get; set; }

    [JsonProperty(nameof(EnergyDisplay))]
    public PointsDisplay EnergyDisplay { get; set; }

    [JsonProperty(nameof(BrowserUrl))]
    public string? BrowserUrl { get; set; } = "https://wiki.guildwars.com/wiki/Main_Page";
}

using Newtonsoft.Json;

namespace Daybreak.Configuration.FocusView;

public sealed class FocusViewOptions
{
    [JsonProperty("ExperienceDisplay")]
    public ExperienceDisplay ExperienceDisplay { get; set; }
    [JsonProperty("KurzickPointsDisplay")]
    public PointsDisplay KurzickPointsDisplay { get; set; }
    [JsonProperty("LuxonPointsDisplay")]
    public PointsDisplay LuxonPointsDisplay { get; set; }
    [JsonProperty("BalthazarPointsDisplay")]
    public PointsDisplay BalthazarPointsDisplay { get; set; }
    [JsonProperty("ImperialPointsDisplay")]
    public PointsDisplay ImperialPointsDisplay { get; set; }
    [JsonProperty("VanquishingDisplay")]
    public PointsDisplay VanquishingDisplay { get; set; }
    [JsonProperty("HealthDisplay")]
    public PointsDisplay HealthDisplay { get; set; }
    [JsonProperty("EnergyDisplay")]
    public PointsDisplay EnergyDisplay { get; set; }
    [JsonProperty("BrowserUrl")]
    public string? BrowserUrl { get; set; } = string.Empty;
}

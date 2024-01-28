using Daybreak.Attributes;
using Daybreak.Configuration.FocusView;
using Daybreak.Models.Browser;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Focus View")]
public sealed class FocusViewOptions
{
    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the focus view is enabled, showing live information from the game")]
    public bool Enabled { get; set; } = true;

    [OptionName(Name = "Inventory Component Enabled", Description = "If true, the focus view will show a component with the inventory contents")]
    public bool InventoryComponentVisible { get; set; }

    [OptionName(Name = "Minimap Component Enabled", Description = "If true, the focus view will show a minimap component")]
    public bool MinimapComponentVisible { get; set; }

    [JsonProperty(nameof(ExperienceDisplay))]
    [OptionName(Name = "Experience Display Mode", Description = "Sets how should the experience display show the information")]
    public ExperienceDisplay ExperienceDisplay { get; set; }

    [JsonProperty(nameof(KurzickPointsDisplay))]
    [OptionName(Name = "Kurzick Points Display Mode", Description = "Sets how should the kurzick points display show the information")]
    public PointsDisplay KurzickPointsDisplay { get; set; }

    [JsonProperty(nameof(LuxonPointsDisplay))]
    [OptionName(Name = "Luxon Points Display Mode", Description = "Sets how should the luxon points display show the information")]
    public PointsDisplay LuxonPointsDisplay { get; set; }

    [JsonProperty(nameof(BalthazarPointsDisplay))]
    [OptionName(Name = "Balthazar Points Display Mode", Description = "Sets how should the balthazar points display show the information")]
    public PointsDisplay BalthazarPointsDisplay { get; set; }

    [JsonProperty(nameof(ImperialPointsDisplay))]
    [OptionName(Name = "Imperial Points Display Mode", Description = "Sets how should the imperial points display show the information")]
    public PointsDisplay ImperialPointsDisplay { get; set; }

    [JsonProperty(nameof(VanquishingDisplay))]
    [OptionName(Name = "Vanquishing Display Mode", Description = "Sets how should the vanquishing display show the information")]
    public PointsDisplay VanquishingDisplay { get; set; }

    [JsonProperty(nameof(HealthDisplay))]
    [OptionName(Name = "Health Display Mode", Description = "Sets how should the health display show the information")]
    public PointsDisplay HealthDisplay { get; set; }

    [JsonProperty(nameof(EnergyDisplay))]
    [OptionName(Name = "Energy Display Mode", Description = "Sets how should the energy display show the information")]
    public PointsDisplay EnergyDisplay { get; set; }

    [JsonProperty(nameof(MinimapRotationEnabled))]
    [OptionName(Name = "Minimap Rotation", Description = "When enabled, the minimap will rotate according to the player camera")]
    public bool MinimapRotationEnabled { get; set; } = true;

    [JsonProperty(nameof(BrowserHistory))]
    [OptionIgnore]
    public BrowserHistory BrowserHistory { get; set; } = new();
}

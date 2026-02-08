using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models.FocusView;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Focus View")]
[OptionsIgnore]
public sealed class FocusViewOptions
{
    [JsonPropertyName(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the focus view is enabled, showing live information from the game")]
    public bool Enabled { get; set; } = false;

    [JsonPropertyName(nameof(ExperienceDisplay))]
    [OptionName(Name = "Experience Display Mode", Description = "Sets how should the experience display show the information")]
    public ExperienceDisplay ExperienceDisplay { get; set; }

    [JsonPropertyName(nameof(KurzickPointsDisplay))]
    [OptionName(Name = "Kurzick Points Display Mode", Description = "Sets how should the kurzick points display show the information")]
    public PointsDisplay KurzickPointsDisplay { get; set; }

    [JsonPropertyName(nameof(LuxonPointsDisplay))]
    [OptionName(Name = "Luxon Points Display Mode", Description = "Sets how should the luxon points display show the information")]
    public PointsDisplay LuxonPointsDisplay { get; set; }

    [JsonPropertyName(nameof(BalthazarPointsDisplay))]
    [OptionName(Name = "Balthazar Points Display Mode", Description = "Sets how should the balthazar points display show the information")]
    public PointsDisplay BalthazarPointsDisplay { get; set; }

    [JsonPropertyName(nameof(ImperialPointsDisplay))]
    [OptionName(Name = "Imperial Points Display Mode", Description = "Sets how should the imperial points display show the information")]
    public PointsDisplay ImperialPointsDisplay { get; set; }

    [JsonPropertyName(nameof(VanquishingDisplay))]
    [OptionName(Name = "Vanquishing Display Mode", Description = "Sets how should the vanquishing display show the information")]
    public PointsDisplay VanquishingDisplay { get; set; }

    [JsonPropertyName(nameof(HealthDisplay))]
    [OptionName(Name = "Health Display Mode", Description = "Sets how should the health display show the information")]
    public PointsDisplay HealthDisplay { get; set; }

    [JsonPropertyName(nameof(EnergyDisplay))]
    [OptionName(Name = "Energy Display Mode", Description = "Sets how should the energy display show the information")]
    public PointsDisplay EnergyDisplay { get; set; }
}

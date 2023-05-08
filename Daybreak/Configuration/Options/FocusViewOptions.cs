﻿using Daybreak.Attributes;
using Daybreak.Configuration.FocusView;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Focus View")]
public sealed class FocusViewOptions
{
    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the focus view is enabled, showing live information from the game")]
    public bool Enabled { get; set; }

    [JsonProperty(nameof(MemoryReaderFrequency))]
    [OptionRange<double>(MinValue = 16, MaxValue = 1000)]
    [OptionName(Name = "Memory Reader Frequency", Description = "Measured in ms. Sets how often should the launcher poll information from the game")]
    public double MemoryReaderFrequency { get; set; } = 16;

    [JsonProperty(nameof(EnablePathfinding))]
    [OptionName(Name = "Enable Pathfinding", Description = "If true, the mini-map will attempt to produce paths from the player position to objectives")]
    public bool EnablePathfinding { get; set; } = true;

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

    [JsonProperty(nameof(BrowserUrl))]
    [OptionIgnore]
    public string? BrowserUrl { get; set; } = "https://wiki.guildwars.com/wiki/Main_Page";
}

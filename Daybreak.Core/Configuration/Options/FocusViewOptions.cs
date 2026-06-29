using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models.FocusView;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Focus View")]
[OptionsIgnore]
public sealed class FocusViewOptions
{
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

    [JsonPropertyName(nameof(GridColumns))]
    [OptionName(Name = "Layout Grid Columns", Description = "Number of columns in the focus view layout grid")]
    [OptionIgnore]
    public int GridColumns { get; set; } = DefaultGridColumns;

    [JsonPropertyName(nameof(GridRows))]
    [OptionName(Name = "Layout Grid Rows", Description = "Number of rows in the focus view layout grid")]
    [OptionIgnore]
    public int GridRows { get; set; } = DefaultGridRows;

    [JsonPropertyName(nameof(Layout))]
    [OptionName(Name = "Layout", Description = "Tile-based layout of the focus view components")]
    public List<FocusViewTile> Layout { get; set; } = DefaultLayout();

    public const int DefaultGridColumns = 20;
    public const int DefaultGridRows = 16;

    public static List<FocusViewTile> DefaultLayout() =>
    [
        new() { Component = FocusViewComponent.Character, Column = 1, Row = 1, ColumnSpan = 10, RowSpan = 3 },
        new() { Component = FocusViewComponent.PlayerResources, Column = 1, Row = 4, ColumnSpan = 10, RowSpan = 3 },
        new() { Component = FocusViewComponent.Vanquish, Column = 1, Row = 7, ColumnSpan = 10, RowSpan = 1 },
        new() { Component = FocusViewComponent.Title, Column = 1, Row = 8, ColumnSpan = 10, RowSpan = 1 },
        new() { Component = FocusViewComponent.CurrentMap, Column = 1, Row = 9, ColumnSpan = 10, RowSpan = 1 },
        new() { Component = FocusViewComponent.QuestLog, Column = 1, Row = 10, ColumnSpan = 10, RowSpan = 7 },
        new() { Component = FocusViewComponent.Build, Column = 11, Row = 1, ColumnSpan = 10, RowSpan = 5 },
        new() { Component = FocusViewComponent.Browser, Column = 11, Row = 6, ColumnSpan = 10, RowSpan = 11 },
    ];
}

using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.FocusView;

public sealed class FocusViewTile
{
    [JsonPropertyName(nameof(Component))]
    public FocusViewComponent Component { get; set; }

    [JsonPropertyName(nameof(Visible))]
    public bool Visible { get; set; } = true;

    [JsonPropertyName(nameof(Column))]
    public int Column { get; set; } = 1;

    [JsonPropertyName(nameof(Row))]
    public int Row { get; set; } = 1;

    [JsonPropertyName(nameof(ColumnSpan))]
    public int ColumnSpan { get; set; } = 1;

    [JsonPropertyName(nameof(RowSpan))]
    public int RowSpan { get; set; } = 1;
}

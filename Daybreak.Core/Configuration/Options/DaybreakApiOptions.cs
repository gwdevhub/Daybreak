using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Daybreak API")]
[OptionsIgnore]
public sealed class DaybreakApiOptions
{
    [JsonPropertyName(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the Daybreak API is injected into Guild Wars, enabling extended functionality such as loading builds, character switching, and the focus view")]
    public bool Enabled { get; set; } = false;
}

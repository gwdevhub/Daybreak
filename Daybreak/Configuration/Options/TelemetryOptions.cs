using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Telemetry")]
[OptionsIgnore]
public sealed class TelemetryOptions
{
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will send application telemetry (scrubbed logs, metrics, errors) to a managed instance")]
    public bool Enabled { get; set; } = false;
}

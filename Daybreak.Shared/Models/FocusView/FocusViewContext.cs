namespace Daybreak.Shared.Models.FocusView;

public sealed class FocusViewContext
{
    public required uint ProcessId { get; init; }
    public required string ConfigurationId { get; init; }
}

namespace Daybreak.Shared.Models.Options;

public sealed class OptionSetter
{
    public bool HasCustomSetter { get; set; }
    public string? CustomSetterAction { get; set; } = default!;
    public Type? CustomSetterViewType { get; set;} = default!;
}

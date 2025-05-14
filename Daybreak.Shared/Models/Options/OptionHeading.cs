namespace Daybreak.Shared.Models.Options;

public sealed class OptionHeading
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public double DesiredHeight { get; init; }
}

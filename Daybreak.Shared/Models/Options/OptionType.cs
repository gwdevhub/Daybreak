namespace Daybreak.Shared.Models.Options;
public sealed class OptionType
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required bool IsVisible { get; init; }
    public required bool IsSynchronized { get; init; }
    public required Type Type { get; init; }
    public required List<OptionProperty> Properties { get; init; }
}

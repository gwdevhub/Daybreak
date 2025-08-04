namespace Daybreak.Shared.Models.Options;
public sealed class OptionInstance
{
    public required object Reference { get; init; }
    public required OptionType Type { get; init; }
}

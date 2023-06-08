namespace Daybreak.Models.Guildwars;

public readonly struct UnknownBagItem : IBagContent
{
    public uint ItemId { get; init; }
    public uint Slot { get; init; }
    public uint Count { get; init; }
}

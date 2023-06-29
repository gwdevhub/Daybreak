using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class BagItem : IBagContent
{
    public ItemBase Item { get; init; }
    public uint Slot { get; init; }
    public uint Count { get; init; }
    public IEnumerable<ItemModifier> Modifiers { get; init; }
}

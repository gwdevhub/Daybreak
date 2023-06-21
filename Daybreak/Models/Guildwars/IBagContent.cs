using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public interface IBagContent
{
    uint Slot { get; }
    uint Count { get; }
    IEnumerable<ItemModifier> Modifiers { get; }
}

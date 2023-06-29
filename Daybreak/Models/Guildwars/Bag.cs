using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class Bag
{
    public List<IBagContent> Items { get; init; }
    public int Capacity { get; init; }
}

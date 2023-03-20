using Daybreak.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;

public sealed class DebouncePositionalEntityCache<T> where T : IEntity
{
    public const int CacheCapacity = 1000;
    public const int CacheStep = 100;

    public T? Entity { get; set; } = default!;
    public HashSet<Position> PositionCache { get; } = new HashSet<Position>(CacheCapacity);
    public LinkedList<Position> PositionList { get; } = new LinkedList<Position>();
}

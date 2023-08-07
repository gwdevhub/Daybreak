using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class PreGameData
{
    public int ChosenCharacterIndex { get; init; }
    public List<string>? Characters { get; init; }
}

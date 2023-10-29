using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;
public sealed class GameState
{
    public List<EntityGameState>? States { get; init; }
}

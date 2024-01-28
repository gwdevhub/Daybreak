using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;
public sealed class GameState
{
    public Camera Camera { get; init; }
    public List<EntityGameState>? States { get; init; }
}

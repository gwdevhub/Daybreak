using Daybreak.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Models;

public sealed class GameData
{
    public MainPlayerInformation? MainPlayer { get; init; }
    public List<PlayerInformation>? Party { get; init; }
    public UserInformation? User { get; init; }
    public SessionInformation? Session { get; init; }
    public List<WorldPlayerInformation>? WorldPlayers { get; init; }
}

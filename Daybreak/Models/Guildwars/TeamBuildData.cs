using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;
public sealed class TeamBuildData
{
    public required List<TeamBuildPlayerData> TeamBuildPlayers { get; init; }
}

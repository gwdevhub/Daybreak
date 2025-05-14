using System.Collections.Generic;

namespace Daybreak.Shared.Models.Guildwars;
public sealed class TeamBuildData
{
    public required List<TeamBuildPlayerData> TeamBuildPlayers { get; init; }
}

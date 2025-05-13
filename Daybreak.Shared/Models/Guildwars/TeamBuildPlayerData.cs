namespace Daybreak.Models.Guildwars;
public abstract class TeamBuildPlayerData
{
    public required Build Build { get; init; }
}

public sealed class TeamBuildMainPlayerData : TeamBuildPlayerData
{
}

public sealed class TeamBuildHeroData : TeamBuildPlayerData
{
    public required Hero Hero { get; init; }
}

public sealed class TeamBuildPartyMemberData : TeamBuildPlayerData
{
}

public sealed class TeamBuildHenchmanData : TeamBuildPlayerData
{
}

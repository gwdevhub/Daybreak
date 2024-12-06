using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;
public sealed class TeamBuildData
{
    public Build? PlayerBuild { get; set; }
    public List<Build>? TeamMemberBuilds { get; set; }
}

using Daybreak.Models.Guildwars;

namespace Daybreak.Models;
public sealed class DownloadedBuild
{
    public string? PreferredName { get; set; }
    public Build? Build { get; set; }
}

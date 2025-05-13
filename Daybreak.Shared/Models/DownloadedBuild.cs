using Daybreak.Models.Builds;

namespace Daybreak.Models;
public sealed class DownloadedBuild
{
    public string? PreferredName { get; set; }
    public IBuildEntry? Build { get; set; }
}

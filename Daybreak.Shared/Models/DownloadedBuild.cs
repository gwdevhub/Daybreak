using Daybreak.Shared.Models.Builds;

namespace Daybreak.Shared.Models;
public sealed class DownloadedBuild
{
    public string? PreferredName { get; set; }
    public IBuildEntry? Build { get; set; }
}

using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.Toolbox;
public interface IToolboxService : IModService
{
    Task NotifyUserIfUpdateAvailable(CancellationToken cancellationToken);

    Task<bool> SetupToolbox(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);

    IAsyncEnumerable<TeamBuildEntry> GetToolboxBuilds(CancellationToken cancellationToken);

    Task<bool> SaveToolboxBuild(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken);

    Task<bool> DeleteToolboxBuild(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken);

    Task<bool> ExportBuildToToolbox(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken);
}

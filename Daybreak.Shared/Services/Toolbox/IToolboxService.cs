using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Mods;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.Toolbox;
public interface IToolboxService : IModService
{
    bool LoadToolboxFromDisk();

    bool LoadToolboxFromUsualLocation();

    Task NotifyUserIfUpdateAvailable(CancellationToken cancellationToken);

    Task<bool> SetupToolbox(ToolboxInstallationStatus toolboxInstallationStatus);

    IAsyncEnumerable<TeamBuildEntry> GetToolboxBuilds(CancellationToken cancellationToken);

    Task<bool> SaveToolboxBuild(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken);

    Task<bool> DeleteToolboxBuild(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken);

    Task<bool> ExportBuildToToolbox(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken);
}

using Daybreak.Models.Builds;
using Daybreak.Models.Progress;
using Daybreak.Services.Mods;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;
public interface IToolboxService : IModService
{
    bool LoadToolboxFromDisk();

    bool LoadToolboxFromUsualLocation();

    Task NotifyUserIfUpdateAvailable(CancellationToken cancellationToken);

    Task<bool> SetupToolbox(ToolboxInstallationStatus toolboxInstallationStatus);

    IAsyncEnumerable<TeamBuildEntry> GetToolboxBuilds(CancellationToken cancellationToken);
}

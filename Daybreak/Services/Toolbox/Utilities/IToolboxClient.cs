using Daybreak.Services.Toolbox.Models;
using Daybreak.Shared.Models.Progress;
using Version = Daybreak.Shared.Models.Versioning.Version;

namespace Daybreak.Services.Toolbox.Utilities;
internal interface IToolboxClient
{
    Task<DownloadLatestOperation> DownloadLatestDll(ToolboxInstallationStatus toolboxInstallationStatus, string destinationFolder, CancellationToken cancellationToken);
    Task<Version?> GetLatestVersion(CancellationToken cancellationToken);
}

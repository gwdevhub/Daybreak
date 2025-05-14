using Daybreak.Services.Toolbox.Models;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Models.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox.Utilities;
internal interface IToolboxClient
{
    Task<DownloadLatestOperation> DownloadLatestDll(ToolboxInstallationStatus toolboxInstallationStatus, string destinationFolder, CancellationToken cancellationToken);
    Task<Version?> GetLatestVersion(CancellationToken cancellationToken);
}

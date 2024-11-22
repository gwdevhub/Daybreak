using Daybreak.Models.Progress;
using Daybreak.Models.Versioning;
using Daybreak.Services.Toolbox.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox.Utilities;
internal interface IToolboxClient
{
    Task<DownloadLatestOperation> DownloadLatestDll(ToolboxInstallationStatus toolboxInstallationStatus, string destinationFolder, CancellationToken cancellationToken);
    Task<Version?> GetLatestVersion(CancellationToken cancellationToken);
}

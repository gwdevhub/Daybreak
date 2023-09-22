using Daybreak.Models.Progress;
using Daybreak.Services.Toolbox.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox.Utilities;
internal interface IToolboxClient
{
    Task<DownloadLatestOperation> DownloadLatestDll(ToolboxInstallationStatus toolboxInstallationStatus, CancellationToken cancellationToken);
}

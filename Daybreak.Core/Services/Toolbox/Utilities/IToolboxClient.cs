using Daybreak.Services.Toolbox.Models;
using Daybreak.Shared.Models.Async;

namespace Daybreak.Services.Toolbox.Utilities;
internal interface IToolboxClient
{
    Task<DownloadLatestOperation> DownloadLatestDll(IProgress<ProgressUpdate> progress, string destinationFolder, CancellationToken cancellationToken);
    Task<Version?> GetLatestVersion(CancellationToken cancellationToken);
}

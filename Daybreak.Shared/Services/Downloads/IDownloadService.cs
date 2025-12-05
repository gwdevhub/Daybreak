using Daybreak.Shared.Models.Async;

namespace Daybreak.Shared.Services.Downloads;

public interface IDownloadService
{
    Task<bool> DownloadFile(string downloadUri, string destinationPath, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);
}

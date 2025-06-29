using Daybreak.Shared.Models.Progress;

namespace Daybreak.Shared.Services.Downloads;

public interface IDownloadService
{
    Task<bool> DownloadFile(string downloadUri, string destinationPath, DownloadStatus downloadStatus, CancellationToken cancellationToken = default);
}

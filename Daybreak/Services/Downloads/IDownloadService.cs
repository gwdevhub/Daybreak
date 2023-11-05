using Daybreak.Models.Progress;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Downloads;

public interface IDownloadService
{
    Task<bool> DownloadFile(string downloadUri, string destinationPath, DownloadStatus downloadStatus, CancellationToken cancellationToken = default);
}

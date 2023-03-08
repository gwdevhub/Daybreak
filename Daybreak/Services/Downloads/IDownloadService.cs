using Daybreak.Models.Progress;
using System.Threading.Tasks;

namespace Daybreak.Services.Downloads;

public interface IDownloadService
{
    Task<bool> DownloadGuildwars(string destinationPath, DownloadStatus downloadStatus);
}

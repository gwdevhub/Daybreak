using Daybreak.Models;
using System.Threading.Tasks;

namespace Daybreak.Services.Updater
{
    public interface IApplicationUpdater
    {
        string CurrentVersion { get; }
        void FinalizeUpdate();
        Task<bool> UpdateAvailable();
        Task<bool> DownloadUpdate(UpdateStatus updateStatus);
    }
}

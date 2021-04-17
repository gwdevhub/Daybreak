using Daybreak.Models;
using Daybreak.Services.ApplicationLifetime;
using System.Threading.Tasks;

namespace Daybreak.Services.Updater
{
    public interface IApplicationUpdater : IApplicationLifetimeService
    {
        string CurrentVersion { get; }
        void FinalizeUpdate();
        void PeriodicallyCheckForUpdates();
        Task<bool> UpdateAvailable();
        Task<bool> DownloadUpdate(UpdateStatus updateStatus);
    }
}

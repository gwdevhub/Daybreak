using Daybreak.Models;
using Daybreak.Models.Versioning;
using Daybreak.Services.ApplicationLifetime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Daybreak.Services.Updater
{
    public interface IApplicationUpdater : IApplicationLifetimeService
    {
        Version CurrentVersion { get; }
        void FinalizeUpdate();
        void PeriodicallyCheckForUpdates();
        Task<IEnumerable<Version>> GetVersions();
        Task<bool> UpdateAvailable();
        Task<bool> DownloadUpdate(Version version, UpdateStatus updateStatus);
        Task<bool> DownloadLatestUpdate(UpdateStatus updateStatus);
    }
}

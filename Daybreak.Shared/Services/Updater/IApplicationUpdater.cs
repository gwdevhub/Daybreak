using Daybreak.Shared.Models.Progress;
using Version = Daybreak.Shared.Models.Versioning.Version;

namespace Daybreak.Shared.Services.Updater;

public interface IApplicationUpdater
{
    Version CurrentVersion { get; }
    void FinalizeUpdate();
    void PeriodicallyCheckForUpdates();
    Task<IEnumerable<Version>> GetVersions();
    Task<bool> UpdateAvailable();
    Task<bool> DownloadUpdate(Version version, UpdateStatus updateStatus);
    Task<bool> DownloadLatestUpdate(UpdateStatus updateStatus);
    Task<string?> GetChangelog(Version version);
}

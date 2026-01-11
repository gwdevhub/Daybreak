using Daybreak.Shared.Models.Async;

namespace Daybreak.Shared.Services.Updater;

public interface IApplicationUpdater
{
    void FinalizeUpdate();
    Version CurrentVersion { get; }
    Task<IEnumerable<Version>> GetVersions(CancellationToken cancellationToken);
    Task<bool> UpdateAvailable(CancellationToken cancellationToken);
    IProgressAsyncOperation<bool> DownloadUpdate(Version version, CancellationToken cancellationToken);
    IProgressAsyncOperation<bool> DownloadLatestUpdate(CancellationToken cancellationToken);
    Task<string?> GetChangelog(Version version, CancellationToken cancellationToken);
}

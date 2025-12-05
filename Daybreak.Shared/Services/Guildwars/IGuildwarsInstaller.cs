using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Async;

namespace Daybreak.Shared.Services.Guildwars;
public interface IGuildWarsInstaller
{
    Task<bool> UpdateGuildwars(string exePath, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);
    Task<bool> InstallGuildwars(string destinationPath, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);
    Task<int?> GetLatestVersionId(CancellationToken cancellationToken);
    Task<int?> GetVersionId(string executablePath, CancellationToken cancellationToken);
    IAsyncEnumerable<GuildWarsUpdateResponse> CheckAndUpdateGuildWarsExecutables(List<GuildWarsUpdateRequest> requests, CancellationToken cancellationToken);
}

using Daybreak.Models.Progress;
using Daybreak.Services.Guildwars.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Guildwars;
public interface IGuildWarsInstaller
{
    Task<bool> UpdateGuildwars(string exePath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken);
    Task<bool> InstallGuildwars(string destinationPath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken);
    Task<int?> GetLatestVersionId(CancellationToken cancellationToken);
    Task<int?> GetVersionId(string executablePath, CancellationToken cancellationToken);
    IAsyncEnumerable<GuildWarsUpdateResponse> CheckAndUpdateGuildWarsExecutables(List<GuildWarsUpdateRequest> requests, CancellationToken cancellationToken);
}

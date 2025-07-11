﻿using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Progress;

namespace Daybreak.Shared.Services.Guildwars;
public interface IGuildWarsInstaller
{
    Task<bool> UpdateGuildwars(string exePath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken);
    Task<bool> InstallGuildwars(string destinationPath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken);
    Task<int?> GetLatestVersionId(CancellationToken cancellationToken);
    Task<int?> GetVersionId(string executablePath, CancellationToken cancellationToken);
    IAsyncEnumerable<GuildWarsUpdateResponse> CheckAndUpdateGuildWarsExecutables(List<GuildWarsUpdateRequest> requests, CancellationToken cancellationToken);
}

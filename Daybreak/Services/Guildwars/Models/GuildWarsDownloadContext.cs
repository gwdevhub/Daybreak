using Daybreak.Shared.Models.Progress;

namespace Daybreak.Services.Guildwars.Models;
internal sealed class GuildWarsDownloadContext
{
    public GuildwarsInstallationStatus? GuildwarsInstallationStatus { get; init; }
    public CancellationTokenSource? CancellationTokenSource { get; init; }
}

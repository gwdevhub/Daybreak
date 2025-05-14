using Daybreak.Shared.Models.Progress;
using System.Threading;

namespace Daybreak.Services.Guildwars.Models;
internal sealed class GuildWarsDownloadContext
{
    public GuildwarsInstallationStatus? GuildwarsInstallationStatus { get; init; }
    public CancellationTokenSource? CancellationTokenSource { get; init; }
}

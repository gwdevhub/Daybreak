using Daybreak.Models.Progress;
using System.Threading;

namespace Daybreak.Services.GuildWars.Models;
internal sealed class GuildWarsDownloadContext
{
    public GuildwarsInstallationStatus? GuildwarsInstallationStatus { get; init; }
    public CancellationTokenSource? CancellationTokenSource { get; init; }
}

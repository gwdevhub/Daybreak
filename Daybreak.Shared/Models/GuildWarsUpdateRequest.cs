using Daybreak.Shared.Models.Progress;
using System.Threading;

namespace Daybreak.Shared.Models;
public sealed class GuildWarsUpdateRequest
{
    public string? ExecutablePath { get; init; }
    public GuildwarsInstallationStatus? Status { get; init; }
    public CancellationToken CancellationToken { get; init; }
}

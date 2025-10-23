using Daybreak.Shared.Models.Async;

namespace Daybreak.Shared.Models;
public sealed class GuildWarsUpdateRequest
{
    public string? ExecutablePath { get; init; }
    public Progress<ProgressUpdate> Progress { get; } = new();
    public CancellationToken CancellationToken { get; init; }
}

using System.Text.Json;

namespace Daybreak.Shared.Services.Options;

public interface IOptionsSynchronizationService
{
    Task<Dictionary<string, JsonDocument>> GetLocalOptions(CancellationToken cancellationToken);

    Task<Dictionary<string, JsonDocument>?> GetRemoteOptions(CancellationToken cancellationToken);

    /// <summary>
    /// Back up options on remote
    /// </summary>
    /// <returns></returns>
    Task BackupOptions(CancellationToken cancellationToken);

    /// <summary>
    /// Restore options from remote
    /// </summary>
    /// <returns></returns>
    Task RestoreOptions(CancellationToken cancellationToken);
}

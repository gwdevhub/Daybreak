using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Options;

public interface IOptionsSynchronizationService
{
    Task<Dictionary<string, JObject>> GetLocalOptions(CancellationToken cancellationToken);

    Task<Dictionary<string, JObject>?> GetRemoteOptions(CancellationToken cancellationToken);

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

using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.DXVK;

public interface IDXVKService : IModService
{
    Version Version { get; }

    Task<bool> SetupDXVK(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);

    Task CheckAndUpdateDXVK(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);
}

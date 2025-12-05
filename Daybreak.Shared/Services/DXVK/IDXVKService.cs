using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.DXVK;

public interface IDXVKService : IModService
{
    Task<bool> SetupDXVK(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);
}

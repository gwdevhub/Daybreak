using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.DXVK;

public interface IDXVKService : IModService
{
    Task<bool> SetupDXVK(DXVKInstallationStatus dXVKInstallationStatus, CancellationToken cancellationToken);
}

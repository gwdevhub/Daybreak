using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Mods;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.DXVK;

public interface IDXVKService : IModService
{
    Task<bool> SetupDXVK(DXVKInstallationStatus dXVKInstallationStatus, CancellationToken cancellationToken);
}

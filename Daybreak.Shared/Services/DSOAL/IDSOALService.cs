using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.DSOAL;

public interface IDSOALService : IModService
{
    void EnsureDSOALSymbolicLinkExists();
    Task<bool> SetupDSOAL(DSOALInstallationStatus dSOALInstallationStatus);
}

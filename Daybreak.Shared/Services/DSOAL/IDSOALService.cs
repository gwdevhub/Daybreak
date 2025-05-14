using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Mods;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.DSOAL;

public interface IDSOALService : IModService
{
    void EnsureDSOALSymbolicLinkExists();
    Task<bool> SetupDSOAL(DSOALInstallationStatus dSOALInstallationStatus);
}

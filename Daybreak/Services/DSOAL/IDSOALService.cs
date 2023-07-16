using Daybreak.Models.Progress;
using Daybreak.Services.Mods;
using System.Threading.Tasks;

namespace Daybreak.Services.DSOAL;

public interface IDSOALService : IModService
{
    void EnsureDSOALSymbolicLinkExists();
    Task<bool> SetupDSOAL(DSOALInstallationStatus dSOALInstallationStatus);
}

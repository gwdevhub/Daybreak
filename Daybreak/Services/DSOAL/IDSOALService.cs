using Daybreak.Models.Progress;
using Daybreak.Services.Mods;
using System.Threading.Tasks;

namespace Daybreak.Services.DSOAL;

public interface IDSOALService : IModService
{
    Task<bool> SetupDSOAL(DSOALInstallationStatus dSOALInstallationStatus);
}

using Daybreak.Models.Progress;
using Daybreak.Services.Mods;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod;
public interface IUModService : IModService
{
    bool LoadUModFromDisk();

    Task<bool> SetupUMod(UModInstallationStatus uModInstallationStatus);

    Task<bool> AddMod(string pathToTpf);
}

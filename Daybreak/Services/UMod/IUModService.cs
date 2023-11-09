using Daybreak.Models.Progress;
using Daybreak.Models.UMod;
using Daybreak.Services.Mods;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod;
public interface IUModService : IModService
{
    bool LoadUModFromDisk();

    Task<bool> SetupUMod(UModInstallationStatus uModInstallationStatus);

    Task CheckAndUpdateUMod(CancellationToken cancellationToken);

    bool AddMod(string pathToTpf, bool? imported = default);

    bool RemoveMod(string pathToTpf);

    List<UModEntry> GetMods();

    void SaveMods(List<UModEntry> list);
}

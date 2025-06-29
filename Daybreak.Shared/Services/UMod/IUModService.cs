using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.Mods;
using Version = Daybreak.Shared.Models.Versioning.Version;

namespace Daybreak.Shared.Services.UMod;
public interface IUModService : IModService
{
    Version Version { get; }

    Task<bool> SetupUMod(UModInstallationStatus uModInstallationStatus);

    Task CheckAndUpdateUMod(CancellationToken cancellationToken);

    bool AddMod(string pathToTpf, bool? imported = default);

    bool RemoveMod(string pathToTpf);

    List<UModEntry> GetMods();

    void SaveMods(List<UModEntry> list);
}

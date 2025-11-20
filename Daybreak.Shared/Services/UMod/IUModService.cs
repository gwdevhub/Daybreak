using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.UMod;
public interface IUModService : IModService
{
    Version Version { get; }

    Task<bool> SetupUMod(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);

    Task CheckAndUpdateUMod(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);

    bool AddMod(string pathToTpf, bool? imported = default);

    bool RemoveMod(string pathToTpf);

    List<UModEntry> GetMods();

    void SaveMods(List<UModEntry> list);

    Task<IReadOnlyCollection<string>> LoadModsFromDisk(CancellationToken cancellationToken);
}

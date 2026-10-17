using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.UMod;
public interface IUModService : IModService
{
    Version Version { get; }

    Task<bool> SetupUMod(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);

    Task CheckAndUpdateUMod(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);

    bool AddMod(string pathToTpf, bool? imported = default);

    bool AddMod(string pathToTpf, bool? imported, LaunchConfigurationWithCredentials? configuration) =>
        this.AddMod(pathToTpf, imported);

    bool RemoveMod(string pathToTpf);

    bool RemoveMod(string pathToTpf, LaunchConfigurationWithCredentials? configuration) => this.RemoveMod(pathToTpf);

    List<UModEntry> GetMods();

    List<UModEntry> GetMods(LaunchConfigurationWithCredentials? configuration) => this.GetMods();

    void SaveMods(List<UModEntry> list);

    void SaveMods(List<UModEntry> list, LaunchConfigurationWithCredentials? configuration) => this.SaveMods(list);

    Task<IReadOnlyCollection<string>> LoadModsFromDisk(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<string>> LoadModsFromDisk(CancellationToken cancellationToken, LaunchConfigurationWithCredentials? configuration) =>
        this.LoadModsFromDisk(cancellationToken);
}

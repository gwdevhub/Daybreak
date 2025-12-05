using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.ReShade;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.ReShade;
public interface IReShadeService : IModService
{
    bool AutoUpdate { get; set; }
    Task<bool> LoadReShadeFromDisk(CancellationToken cancellationToken);
    Task<bool> InstallPackage(ShaderPackage package, CancellationToken cancellationToken);
    Task<bool> InstallPackage(string pathToZip, CancellationToken cancellationToken);
    Task<IEnumerable<ShaderPackage>> GetStockPackages(CancellationToken cancellationToken);
    Task<bool> SetupReShade(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);
    Task<string> GetConfig(CancellationToken cancellationToken);
    Task<bool> SaveConfig(string config, CancellationToken cancellationToken);
    Task<string> GetPreset(CancellationToken cancellationToken);
    Task<bool> SavePreset(string config, CancellationToken cancellationToken);
    Task<bool> UpdateIniFromPath(string pathToIni, CancellationToken cancellationToken);
}

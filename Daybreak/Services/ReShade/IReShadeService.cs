using Daybreak.Models.Progress;
using Daybreak.Models.ReShade;
using Daybreak.Services.Mods;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.ReShade;
public interface IReShadeService : IModService
{
    bool AutoUpdate { get; set; }
    Task<bool> LoadReShadeFromDisk(CancellationToken cancellationToken);
    Task<bool> InstallPackage(ShaderPackage package, CancellationToken cancellationToken);
    Task<bool> InstallPackage(string pathToZip, CancellationToken cancellationToken);
    Task<IEnumerable<ShaderPackage>> GetStockPackages(CancellationToken cancellationToken);
    Task<bool> SetupReShade(ReShadeInstallationStatus reShadeInstallationStatus, CancellationToken cancellationToken);
    Task<string> GetConfig(CancellationToken cancellationToken);
    Task<bool> SaveConfig(string config, CancellationToken cancellationToken);
    Task<string> GetPreset(CancellationToken cancellationToken);
    Task<bool> SavePreset(string config, CancellationToken cancellationToken);
}

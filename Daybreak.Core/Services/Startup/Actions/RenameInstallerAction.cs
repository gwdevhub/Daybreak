using Daybreak.Shared.Models;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Startup.Actions;

internal sealed class RenameInstallerAction(
    ILogger<RenameInstallerAction> logger) : StartupActionBase
{
    private static readonly string TemporaryInstallerFileNameSubPath = OperatingSystem.IsWindows()
        ? "Installer/Daybreak.Installer.Temp.exe"
        : "Installer/Daybreak.Installer.Temp";
    private static readonly string InstallerFileNameSubPath = OperatingSystem.IsWindows()
        ? "Installer/Daybreak.Installer.exe"
        : "Installer/Daybreak.Installer";

    private static readonly string TemporaryInstallerFileName = PathUtils.GetAbsolutePathFromRoot(TemporaryInstallerFileNameSubPath);
    private static readonly string InstallerFileName = PathUtils.GetAbsolutePathFromRoot(InstallerFileNameSubPath);

    private readonly ILogger<RenameInstallerAction> logger = logger.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Checking for new installer version to rename");
        if (File.Exists(TemporaryInstallerFileName))
        {
            scopedLogger.LogDebug("Detected new installer version. Overwriting old installer with new one");
            File.Copy(TemporaryInstallerFileName, InstallerFileName, true);

            scopedLogger.LogDebug("Deleting new installer temporary file");
            File.Delete(TemporaryInstallerFileName);
        }
    }
}

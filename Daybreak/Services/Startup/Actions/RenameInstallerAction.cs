using Daybreak.Shared.Models;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.IO;

namespace Daybreak.Services.Startup.Actions;

internal sealed class RenameInstallerAction : StartupActionBase
{
    private const string TemporaryInstallerFileNameSubPath = "Daybreak.Installer.Temp.exe";
    private const string InstallerFileNameSubPath = "Daybreak.Installer.exe";

    private static readonly string TemporaryInstallerFileName = PathUtils.GetAbsolutePathFromRoot(TemporaryInstallerFileNameSubPath);
    private static readonly string InstallerFileName = PathUtils.GetAbsolutePathFromRoot(InstallerFileNameSubPath);

    private readonly ILogger<RenameInstallerAction> logger;

    public RenameInstallerAction(
        ILogger<RenameInstallerAction> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public override void ExecuteOnStartup()
    {
        if (File.Exists(TemporaryInstallerFileName))
        {
            this.logger.LogDebug("Detected new installer version. Overwriting old installer with new one");
            File.Copy(TemporaryInstallerFileName, InstallerFileName, true);

            this.logger.LogDebug("Deleting new installer temporary file");
            File.Delete(TemporaryInstallerFileName);
        }
    }
}

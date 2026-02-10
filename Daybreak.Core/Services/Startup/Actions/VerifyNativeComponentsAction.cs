using System.Extensions.Core;
using Daybreak.Shared;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Daybreak.Services.Startup.Actions;

/// <summary>
/// Startup action that verifies all required native components are present.
/// Checks for Injector, API, and Installer in their respective directories.
/// </summary>
public sealed class VerifyNativeComponentsAction(
    INotificationService notificationService,
    ILogger<VerifyNativeComponentsAction> logger
) : StartupActionBase
{
    private const string InjectorFolder = "Injector";
    private const string ApiFolder = "Api";
    private const string InstallerFolder = "Installer";

    private static readonly string[] InjectorFiles = OperatingSystem.IsWindows()
        ? ["Daybreak.Injector.exe", "FASM.DLL"]
        : ["Daybreak.Injector.exe", "FASM.DLL"];
    private static readonly string[] ApiFiles = ["Daybreak.API.dll"];
    private static readonly string[] InstallerFiles = OperatingSystem.IsWindows()
        ? ["Daybreak.Installer.exe"]
        : ["Daybreak.Installer"];
    private static readonly string[] InstallerFallbackFiles = OperatingSystem.IsWindows()
        ? ["Daybreak.Installer.Temp.exe"]
        : ["Daybreak.Installer.Temp"];

    private readonly INotificationService notificationService = notificationService;
    private readonly ILogger<VerifyNativeComponentsAction> logger = logger;

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var injectorMissing = this.CheckComponentFiles(InjectorFolder, InjectorFiles);
        if (!injectorMissing)
        {
            scopedLogger.LogWarning("Missing Injector files");
        }

        var apiMissing = this.CheckComponentFiles(ApiFolder, ApiFiles);
        if (!apiMissing)
        {
            scopedLogger.LogWarning("Missing API files");
        }

        var installerMissing = this.CheckComponentFilesWithFallback(
            InstallerFolder,
            InstallerFiles,
            InstallerFallbackFiles
        );
        if (!installerMissing)
        {
            scopedLogger.LogWarning("Missing Installer files");
        }

        if (!injectorMissing || !apiMissing || !installerMissing)
        {
            scopedLogger.LogError("Missing native components detected");
            this.notificationService.NotifyError(
                title: "Missing native components",
                description: "Daybreak detected missing native components and some features may not work.",
                expirationTime: Global.NotificationLongExpiration
            );
        }
        else
        {
            scopedLogger.LogDebug("All native components verified successfully");
        }
    }

    private bool CheckComponentFiles(string folder, string[] requiredFiles)
    {
        var folderPath = PathUtils.GetAbsolutePathFromRoot(folder);

        if (!Directory.Exists(folderPath))
        {
            this.logger.LogDebug("Component folder does not exist: {Folder}", folderPath);
            return false;
        }

        foreach (var file in requiredFiles)
        {
            var filePath = Path.Combine(folderPath, file);
            if (!File.Exists(filePath))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckComponentFilesWithFallback(
        string folder,
        string[] primaryFiles,
        string[] fallbackFiles
    )
    {
        var folderPath = PathUtils.GetAbsolutePathFromRoot(folder);

        if (!Directory.Exists(folderPath))
        {
            this.logger.LogDebug("Component folder does not exist: {Folder}", folderPath);
            return false;
        }

        var anyPrimaryExists = primaryFiles.Any(f => File.Exists(Path.Combine(folderPath, f)));
        if (anyPrimaryExists)
        {
            return this.CheckComponentFiles(folder, primaryFiles);
        }

        var anyFallbackExists = fallbackFiles.Any(f => File.Exists(Path.Combine(folderPath, f)));
        if (anyFallbackExists)
        {
            return this.CheckComponentFiles(folder, fallbackFiles);
        }

        return false;
    }
}

using System.Extensions.Core;
using Daybreak.Linux.Services.Startup.Notifications;
using Daybreak.Linux.Services.Wine;
using Daybreak.Shared;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Startup.Actions;

/// <summary>
/// Startup action that checks if Wine is available and the prefix is initialized.
/// If Wine is available but the prefix is not set up, notifies the user to initialize it.
/// </summary>
public sealed class SetupWinePrefixAction(
    IWinePrefixManager winePrefixManager,
    INotificationService notificationService,
    ILogger<SetupWinePrefixAction> logger
) : StartupActionBase
{
    private readonly IWinePrefixManager winePrefixManager = winePrefixManager;
    private readonly INotificationService notificationService = notificationService;
    private readonly ILogger<SetupWinePrefixAction> logger = logger;

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!this.winePrefixManager.IsAvailable())
        {
            scopedLogger.LogWarning("Wine is not installed. Game launching will not be available.");
            this.notificationService.NotifyError(
                title: "Wine not installed",
                description: "Wine is required to launch Guild Wars on Linux. Please install Wine and restart Daybreak.",
                expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(30)
            );
            return;
        }

        if (this.winePrefixManager.IsInitialized())
        {
            scopedLogger.LogDebug(
                "Wine prefix already initialized at {PrefixPath}",
                this.winePrefixManager.GetWinePrefixPath()
            );
            return;
        }

        scopedLogger.LogInformation("Wine prefix not initialized. Prompting user to set it up.");
        this.notificationService.NotifyInformation<WinePrefixSetupHandler>(
            title: "Wine prefix setup required",
            description: "Click here to initialize the Wine prefix for Guild Wars launching.",
            expirationTime: Global.NotificationLongExpiration
        );
    }

    public override async Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        if (
            !this.winePrefixManager.IsAvailable()
            || !this.winePrefixManager.IsInitialized()
        )
        {
            return;
        }

        var scopedLogger = this.logger.CreateScopedLogger();
        if (await this.winePrefixManager.ConfigureComputerName(cancellationToken))
        {
            scopedLogger.LogDebug("Wine computer name configuration is up to date");
            return;
        }

        scopedLogger.LogError(
            "Failed to configure the Linux host name as the Wine computer name"
        );
        this.notificationService.NotifyError(
            title: "Wine prefix configuration failed",
            description: "Daybreak could not configure the Linux host name as the Wine computer name. Check the logs for details.",
            expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(30)
        );
    }
}

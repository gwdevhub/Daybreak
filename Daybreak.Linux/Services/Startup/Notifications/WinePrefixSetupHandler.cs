using System.Extensions.Core;
using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Startup.Notifications;

/// <summary>
/// Notification handler that triggers Wine prefix installation when the user clicks the notification.
/// </summary>
public sealed class WinePrefixSetupHandler(
    IWinePrefixManager winePrefixManager,
    INotificationService notificationService,
    ILogger<WinePrefixSetupHandler> logger
) : INotificationHandler
{
    private readonly IWinePrefixManager winePrefixManager = winePrefixManager;
    private readonly INotificationService notificationService = notificationService;
    private readonly ILogger<WinePrefixSetupHandler> logger = logger;

    public void OpenNotification(Notification notification)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation(
            "User clicked Wine prefix setup notification. Starting installation..."
        );

        // Start the installation in the background
        _ = Task.Factory.StartNew(this.PerformInstallation);
    }

    private async Task PerformInstallation()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var installOperation = this.winePrefixManager.Install(CancellationToken.None);

            // Subscribe to progress updates
            installOperation.ProgressChanged += (sender, progress) =>
            {
                scopedLogger.LogDebug(
                    "Wine prefix setup progress: {Progress:P0} - {Message}",
                    progress.Percentage.Value,
                    progress.StatusMessage ?? string.Empty
                );
            };

            var result = await installOperation;

            if (result)
            {
                scopedLogger.LogInformation("Wine prefix setup completed successfully");
                this.notificationService.NotifyInformation(
                    title: "Wine prefix ready",
                    description: "Wine prefix has been initialized successfully. You can now launch Guild Wars.",
                    expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(5)
                );
            }
            else
            {
                scopedLogger.LogError("Wine prefix setup failed");
                this.notificationService.NotifyError(
                    title: "Wine prefix setup failed",
                    description: "Failed to initialize the Wine prefix. Check the logs for more details.",
                    expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(5)
                );
            }
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Exception during Wine prefix setup");
            this.notificationService.NotifyError(
                title: "Wine prefix setup error",
                description: $"An error occurred: {ex.Message}",
                expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(5)
            );
        }
    }
}

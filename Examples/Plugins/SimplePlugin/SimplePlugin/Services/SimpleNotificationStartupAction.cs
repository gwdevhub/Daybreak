using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace SimplePlugin.Services;

public sealed class SimpleNotificationStartupAction(
    INotificationService notificationService,
    ILogger<SimpleNotificationStartupAction> logger)
    : StartupActionBase
{
    private readonly INotificationService notificationService = notificationService;
    private readonly ILogger<SimpleNotificationStartupAction> logger = logger;

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        Task.Factory.StartNew(async () =>
        {
            scopedLogger.LogInformation("SimpleNotificationStartupAction: Waiting for 3 seconds before sending notification.");
            await Task.Delay(3000);

            scopedLogger.LogInformation("SimpleNotificationStartupAction: Sending startup notification.");
            this.notificationService.NotifyInformation(
                title: "Simple Plugin",
                description: "Reporting for duty!");
        });
    }
}

using Daybreak.Services.Notifications;
using System.Core.Extensions;
using System.Windows.Extensions.Services;

namespace SimplePlugin.Services;
public sealed class EmitLoadedNotificationService : IApplicationLifetimeService
{
    private readonly INotificationService notificationService;

    public EmitLoadedNotificationService(
        INotificationService notificationService)
    {
        this.notificationService = notificationService.ThrowIfNull();
    }

    public void OnClosing()
    {
    }

    public async void OnStartup()
    {
        await Task.Delay(2000);
        this.notificationService.NotifyInformation("SimplePlugin", "Successfully loaded!");
    }
}

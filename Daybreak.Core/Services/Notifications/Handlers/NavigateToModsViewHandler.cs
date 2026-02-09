using System.Core.Extensions;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Views;
using TrailBlazr.Services;

namespace Daybreak.Services.Notifications.Handlers;

public sealed class NavigateToModsViewHandler(IViewManager viewManager) : INotificationHandler
{
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public void OpenNotification(Notification notification)
    {
        this.viewManager.ShowView<ModsView>();
    }
}

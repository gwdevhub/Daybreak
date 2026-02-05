using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Views;
using System.Core.Extensions;
using TrailBlazr.Services;

namespace Daybreak.Services.Updater;
internal sealed class UpdateNotificationHandler(IViewManager viewManager)
    : INotificationHandler
{
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public void OpenNotification(Notification notification)
    {
        var maybeVersion = notification.Metadata;
        if (!Version.TryParse(maybeVersion, out var version))
        {
            return;
        }

        this.viewManager.ShowView<UpdateConfirmationView>((nameof(UpdateConfirmationView.Version), version.ToString()));
    }
}

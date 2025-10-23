using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Models.Trade;
using Newtonsoft.Json;
using System.Extensions;

namespace Daybreak.Services.TradeChat.Notifications;

//TODO: Handle trade message notifications
internal sealed class TradeMessageNotificationHandler : INotificationHandler
{
    //private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public void OpenNotification(Notification notification)
    {
        if (notification.Metadata.IsNullOrWhiteSpace())
        {
            return;
        }

        var trade = JsonConvert.DeserializeObject<TraderMessage>(notification.Metadata);
        //this.viewManager.ShowView<TradeNotificationView>(trade!);
    }
}

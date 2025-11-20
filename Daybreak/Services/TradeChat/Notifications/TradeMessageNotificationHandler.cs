using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Models.Trade;
using Daybreak.Views.Trade;
using Newtonsoft.Json;
using System.Core.Extensions;
using System.Extensions;
using TrailBlazr.Services;

namespace Daybreak.Services.TradeChat.Notifications;

internal sealed class TradeMessageNotificationHandler(IViewManager viewManager)
    : INotificationHandler
{
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public void OpenNotification(Notification notification)
    {
        if (notification.Metadata.IsNullOrWhiteSpace())
        {
            return;
        }

        var trade = JsonConvert.DeserializeObject<TraderMessage>(notification.Metadata);
        if (trade is null)
        {
            return;
        }

        this.viewManager.ShowView<TradeNotificationView>(
            (nameof(TradeNotificationView.Message), trade.Message),
            (nameof(TradeNotificationView.Sender), trade.Sender),
            (nameof(TradeNotificationView.Timestamp), trade.Timestamp.ToString("g")));
    }
}

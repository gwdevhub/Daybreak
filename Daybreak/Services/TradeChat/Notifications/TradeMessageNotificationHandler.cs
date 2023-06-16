using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;
using Daybreak.Models.Trade;
using Daybreak.Services.Navigation;
using Daybreak.Views.Trade;
using Newtonsoft.Json;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.TradeChat.Notifications;

public sealed class TradeMessageNotificationHandler : INotificationHandler
{
    private readonly IViewManager viewManager;

    public TradeMessageNotificationHandler(IViewManager viewManager)
    {
        this.viewManager = viewManager.ThrowIfNull();
    }

    public void OpenNotification(Notification notification)
    {
        if (notification.Metadata.IsNullOrWhiteSpace())
        {
            return;
        }

        var trade = JsonConvert.DeserializeObject<TraderMessage>(notification.Metadata);
        this.viewManager.ShowView<TradeNotificationView>(trade!);
    }
}

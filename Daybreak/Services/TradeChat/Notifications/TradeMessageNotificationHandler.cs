﻿using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Views.Trade;
using Newtonsoft.Json;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.TradeChat.Notifications;

internal sealed class TradeMessageNotificationHandler(IViewManager viewManager) : INotificationHandler
{
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

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

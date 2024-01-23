﻿using Daybreak.Models.Notifications;
using Daybreak.Models.Notifications.Handling;
using System.Core.Extensions;
using System.IO;
using System.Threading;

namespace Daybreak.Services.ReShade.Notifications;
internal sealed class ReShadeConfigChangedHandler : INotificationHandler
{
    private readonly IReShadeService reShadeService;

    public ReShadeConfigChangedHandler(
        IReShadeService reShadeService)
    {
        this.reShadeService = reShadeService.ThrowIfNull();
    }

    public async void OpenNotification(Notification notification)
    {
        var presetPath = Path.GetFullPath(notification.Metadata);
        if (!File.Exists(presetPath))
        {
            return;
        }

        await this.reShadeService.UpdateIniFromPath(presetPath, CancellationToken.None);
    }
}

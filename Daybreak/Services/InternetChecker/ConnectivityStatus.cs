using Daybreak.Models;
using Daybreak.Models.Notifications;
using Daybreak.Services.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.InternetChecker;

internal sealed class ConnectivityStatus : IConnectivityStatus, IApplicationLifetimeService
{
    private static readonly TimeSpan ConnectivityBackoff = TimeSpan.FromSeconds(5);

    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly IInternetCheckingService internetCheckingService;
    private readonly INotificationService notificationService;

    public bool IsInternetAvailable { get; private set; } = false;

    public ConnectivityStatus(
        IInternetCheckingService internetCheckingService,
        INotificationService notificationService)
    {
        this.internetCheckingService = internetCheckingService.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
    }

    public async void OnStartup()
    {
        var checkingConnectionNotification = this.notificationService.NotifyInformation(
            "Checking connection",
            "Checking internet connection status",
            expirationTime: DateTime.MaxValue,
            clickClosable: false);
        NotificationToken? internetUnavailableNotification = default;
        while (!this.cancellationTokenSource.IsCancellationRequested)
        {
            var connectionStatus = await this.internetCheckingService.CheckConnectionAvailable(this.cancellationTokenSource.Token);
            if (checkingConnectionNotification.Closed is not true)
            {
                checkingConnectionNotification.Cancel();
            }

            switch (connectionStatus)
            {
                case InternetConnectionState.Available:
                case InternetConnectionState.PartialOutage:
                    this.IsInternetAvailable = true;
                    break;
                case InternetConnectionState.GuildwarsOutage:
                case InternetConnectionState.Unavailable:
                    this.IsInternetAvailable = false;
                    break;
                case InternetConnectionState.Undefined:
                default:
                    throw new InvalidOperationException($"Unexpected connection status {connectionStatus}");
            }

            // Only show Connection Restored message when a previous internet unavailable notification has been shown
            if (connectionStatus is InternetConnectionState.PartialOutage or InternetConnectionState.Available &&
                internetUnavailableNotification.HasValue)
            {
                internetUnavailableNotification?.Cancel();
                internetUnavailableNotification = default;
                this.notificationService.NotifyInformation("Connection restored", "Internet connection has been restored", clickClosable: true);
            }

            // Only show Connection Unavailable message when no other connection unavailable notification is being shown
            if (connectionStatus is InternetConnectionState.GuildwarsOutage or InternetConnectionState.Unavailable &&
                !internetUnavailableNotification.HasValue)
            {
                internetUnavailableNotification = this.notificationService.NotifyError(
                    "Connection unavailable",
                    "Internet connection is not available",
                    expirationTime: DateTime.MaxValue,
                    clickClosable: false);
            }

            await Task.Delay(ConnectivityBackoff, this.cancellationTokenSource.Token);
        }
    }

    public void OnClosing()
    {
        this.cancellationTokenSource.Cancel();
        this.cancellationTokenSource.Dispose();
    }
}

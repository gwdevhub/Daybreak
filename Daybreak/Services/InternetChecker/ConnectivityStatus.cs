using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Services.InternetChecker;
using Daybreak.Shared.Services.Notifications;
using System.Core.Extensions;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.InternetChecker;

internal sealed class ConnectivityStatus(
    IInternetCheckingService internetCheckingService,
    INotificationService notificationService) : IConnectivityStatus, IApplicationLifetimeService
{
    private static readonly TimeSpan ConnectivityBackoff = TimeSpan.FromSeconds(30);

    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly IInternetCheckingService internetCheckingService = internetCheckingService.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();

    public bool IsInternetAvailable { get; private set; } = false;

    public async void OnStartup()
    {
        var checkingConnectionNotification = this.notificationService.NotifyInformation(
            "Checking connection",
            "Checking internet connection status",
            expirationTime: DateTime.MaxValue,
            clickClosable: false,
            persistent: false);
        NotificationToken? internetUnavailableNotification = default;
        while (!this.cancellationTokenSource.IsCancellationRequested)
        {
            var connectionStatus = await this.internetCheckingService.CheckConnectionAvailable(this.cancellationTokenSource.Token);
            if (checkingConnectionNotification.Closed is not true)
            {
                checkingConnectionNotification.Cancel();
            }

            this.IsInternetAvailable = connectionStatus switch
            {
                InternetConnectionState.Available or InternetConnectionState.PartialOutage => true,
                InternetConnectionState.GuildwarsOutage or InternetConnectionState.Unavailable => false,
                _ => throw new InvalidOperationException($"Unexpected connection status {connectionStatus}"),
            };

            // Only show Connection Restored message when a previous internet unavailable notification has been shown
            if (connectionStatus is InternetConnectionState.PartialOutage or InternetConnectionState.Available &&
                internetUnavailableNotification.HasValue)
            {
                internetUnavailableNotification?.Cancel();
                internetUnavailableNotification = default;
                this.notificationService.NotifyInformation("Connection restored", "Internet connection has been restored", clickClosable: true, persistent: false);
            }

            // Only show Connection Unavailable message when no other connection unavailable notification is being shown
            if (connectionStatus is InternetConnectionState.GuildwarsOutage or InternetConnectionState.Unavailable &&
                !internetUnavailableNotification.HasValue)
            {
                internetUnavailableNotification = this.notificationService.NotifyError(
                    "Connection unavailable",
                    "Internet connection is not available",
                    expirationTime: DateTime.MaxValue,
                    clickClosable: false,
                    persistent: false);
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

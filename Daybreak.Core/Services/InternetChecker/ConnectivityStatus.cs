using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Services.InternetChecker;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Hosting;
using System.Core.Extensions;

namespace Daybreak.Services.InternetChecker;

internal sealed class ConnectivityStatus(
    IInternetCheckingService internetCheckingService,
    INotificationService notificationService) : IConnectivityStatus, IHostedService
{
    private static readonly TimeSpan ConnectivityBackoff = TimeSpan.FromSeconds(30);

    private readonly IInternetCheckingService internetCheckingService = internetCheckingService.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();

    public bool IsInternetAvailable { get; private set; } = false;

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        var checkingConnectionNotification = this.notificationService.NotifyInformation(
            "Checking connection",
            "Checking internet connection status",
            expirationTime: DateTime.MaxValue,
            clickClosable: false,
            persistent: false);
        NotificationToken? internetUnavailableNotification = default;
        while (!cancellationToken.IsCancellationRequested)
        {
            var connectionStatus = await this.internetCheckingService.CheckConnectionAvailable(cancellationToken);
            if (checkingConnectionNotification.Closed is not true)
            {
                checkingConnectionNotification.Cancel();
            }

            this.IsInternetAvailable = connectionStatus switch
            {
                InternetConnectionState.Available => true,
                InternetConnectionState.Unavailable => false,
                _ => throw new InvalidOperationException($"Unexpected connection status {connectionStatus}"),
            };

            // Only show Connection Restored message when a previous internet unavailable notification has been shown
            if (connectionStatus is InternetConnectionState.Available &&
                internetUnavailableNotification.HasValue)
            {
                internetUnavailableNotification?.Cancel();
                internetUnavailableNotification = default;
                this.notificationService.NotifyInformation("Connection restored", "Internet connection has been restored", clickClosable: true, persistent: false);
            }

            // Only show Connection Unavailable message when no other connection unavailable notification is being shown
            if (connectionStatus is InternetConnectionState.Unavailable &&
                !internetUnavailableNotification.HasValue)
            {
                internetUnavailableNotification = this.notificationService.NotifyError(
                    "Connection unavailable",
                    "Internet connection is not available",
                    expirationTime: DateTime.MaxValue,
                    clickClosable: false,
                    persistent: false);
            }

            await Task.Delay(ConnectivityBackoff, cancellationToken);
        }
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

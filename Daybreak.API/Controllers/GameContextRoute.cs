using Daybreak.API.Interop;
using Daybreak.API.Services;
using Net.Sdk.Web.Websockets;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Net.WebSockets;
using System.Text;

namespace Daybreak.API.Controllers;

public sealed class GameContextRoute(
    GameThreadService gameThreadService,
    ILogger<GameContextRoute> logger)
    : WebSocketRouteBase
{
    private readonly GameThreadService gameThreadService = gameThreadService.ThrowIfNull();
    private readonly ILogger<GameContextRoute> logger = logger.ThrowIfNull();

    private CallbackRegistration? gameCallbackRegistration;

    public override Task ExecuteAsync(WebSocketMessageType type, byte[] data, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public override Task SocketAccepted(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("WebSocket accepted: {id}", this.Context?.Connection.Id ?? string.Empty);
        this.gameCallbackRegistration = this.gameThreadService.RegisterCallback(this.OnGameThreadProc);
        return base.SocketAccepted(cancellationToken);
    }

    public override Task SocketClosed()
    {
        this.gameCallbackRegistration?.Dispose();
        this.gameCallbackRegistration = default;
        return base.SocketClosed();
    }

    private async void OnGameThreadProc()
    {
        if (this.WebSocket is null)
        {
            return;
        }

        var message = Encoding.Unicode.GetBytes("Procced game thread");
        await this.WebSocket.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}

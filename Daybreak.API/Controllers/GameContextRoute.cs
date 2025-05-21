using Daybreak.API.Services;
using Net.Sdk.Web.Websockets;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Net.WebSockets;

namespace Daybreak.API.Controllers;

public sealed class GameContextRoute(
    ChatService chatService,
    ILogger<GameContextRoute> logger)
    : WebSocketRouteBase
{
    private readonly ChatService chatService = chatService.ThrowIfNull();
    private readonly ILogger<GameContextRoute> logger = logger.ThrowIfNull();

    public override Task ExecuteAsync(WebSocketMessageType type, byte[] data, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public override async Task SocketAccepted(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("WebSocket accepted: {id}", this.Context?.Connection.Id ?? string.Empty);
        await this.chatService.AddMessageAsync($"GameContext subscriber added: {this.Context?.Connection.RemoteIpAddress?.ToString()}:{this.Context?.Connection.RemotePort}", "Daybreak.API", Models.Channel.Whisper, cancellationToken);
    }

    public override async Task SocketClosed()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("WebSocket closed: {id}", this.Context?.Connection.Id ?? string.Empty);
        await this.chatService.AddMessageAsync($"GameContext subscriber removed: {this.Context?.Connection.RemoteIpAddress?.ToString()}:{this.Context?.Connection.RemotePort}", "Daybreak.API", Models.Channel.Whisper, CancellationToken.None);
    }
}

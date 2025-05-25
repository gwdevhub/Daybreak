using Daybreak.API.Models;
using Daybreak.API.Services;
using Net.Sdk.Web.Websockets;
using System.Buffers;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Net.WebSockets;

namespace Daybreak.API.Controllers.WebSocket.GameState;

public sealed class MainPlayerStateRoute(
    MainPlayerStateService mainPlayerStateService,
    ChatService chatService,
    ILogger<MainPlayerStateRoute> logger)
    : WebSocketRouteBase
{
    private const int DefaultFrequency = 1000;

    private readonly byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
    private readonly MainPlayerStateService mainPlayerStateService = mainPlayerStateService.ThrowIfNull();
    private readonly ChatService chatService = chatService.ThrowIfNull();
    private readonly ILogger<MainPlayerStateRoute> logger = logger.ThrowIfNull();

    private CallbackRegistration? mainPlayerStateRegistration;

    public override Task ExecuteAsync(WebSocketMessageType type, ReadOnlySequence<byte> data, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public override async Task SocketAccepted(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!int.TryParse(this.Context?.Request.Query["f"].FirstOrDefault(), out var freq))
        {
            freq = DefaultFrequency;
        }

        var freqTimeSpan = TimeSpan.FromMilliseconds(freq);
        scopedLogger.LogInformation("WebSocket accepted {id} with frequency {frequency}", this.Context?.Connection.Id ?? string.Empty, freqTimeSpan);

        this.mainPlayerStateRegistration = this.mainPlayerStateService.RegisterConsumer(freqTimeSpan, this.SendState);

        await this.chatService.AddMessageAsync(
            message: $"{nameof(MainPlayerState)} subscriber added: {this.Context?.Connection.RemoteIpAddress?.ToString()}:{this.Context?.Connection.RemotePort} with frequency {freq}ms",
            sender: "Daybreak.API",
            channel: Channel.Whisper,
            cancellationToken: cancellationToken);
    }

    public override async Task SocketClosed()
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        this.mainPlayerStateRegistration?.Dispose();
        this.mainPlayerStateRegistration = default;
        ArrayPool<byte>.Shared.Return(this.buffer);

        scopedLogger.LogInformation("WebSocket closed {id}", this.Context?.Connection.Id ?? string.Empty);
        await this.chatService.AddMessageAsync(
            message: $"{nameof(MainPlayerState)} subscriber removed: {this.Context?.Connection.RemoteIpAddress?.ToString()}:{this.Context?.Connection.RemotePort}",
            sender: "Daybreak.API",
            channel: Channel.Whisper,
            cancellationToken: CancellationToken.None);
    }

    private void SendState(ReadOnlySpan<byte> state)
    {
        // state lives on the stack, so we need to copy it to a buffer
        state.CopyTo(this.buffer);
        Task.Run(async () =>
        {
            if (this.WebSocket is null)
            {
                return;
            }

            await this.WebSocket.SendAsync(this.buffer, WebSocketMessageType.Binary, true, this.Context?.RequestAborted ?? CancellationToken.None);
        });
    }
}

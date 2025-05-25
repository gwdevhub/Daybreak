using Daybreak.API.Interop;
using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.API.Services;

public sealed class ChatService(
    GameThreadService gameThreadService,
    UIHandlingService uIHandlingService)
{
    private readonly SemaphoreSlim semaphoreSlim = new(1);
    private readonly GameThreadService gameThreadService = gameThreadService.ThrowIfNull();
    private readonly UIHandlingService uIHandlingService = uIHandlingService.ThrowIfNull();

    /// <summary>
    /// Adds a message to the chat. It does not send the message, it only shows it to the player.
    /// </summary>
    public async ValueTask AddMessageAsync(string message, string? sender, Channel channel, CancellationToken cancellationToken)
    {
        using var ctx = await this.semaphoreSlim.Acquire(cancellationToken);
        await this.gameThreadService.QueueOnGameThread(() =>
        {
            var encodedMessage = string.Create(
                3 + message.Length,
                message,
                static (span, msg) =>
                {
                    span[0] = '\u0108';
                    span[1] = '\u0107';
                    msg.AsSpan().CopyTo(span[2..]);
                    span[^1] = '\u0001';
                });

            var encodedSender = sender is not null
            ? string.Create(
                3 + sender.Length,
                sender,
                static (span, msg) =>
                {
                    span[0] = '\u0108';
                    span[1] = '\u0107';
                    msg.AsSpan().CopyTo(span[2..]);
                    span[^1] = '\u0001';
                })
            : default;

            var encoded = encodedSender is null
            ? encodedMessage
            : string.Create(
                encodedMessage.Length + encodedSender.Length + 6,
                (encodedMessage, encodedSender),
                static (span, state) =>
                {
                    span[0] = '\u076b';
                    span[1] = '\u010a';
                    state.encodedSender.AsSpan().CopyTo(span[2..]);
                    span[2 + state.encodedSender.Length] = '\u0001';
                    span[3 + state.encodedSender.Length] = '\u010b';
                    state.encodedMessage.AsSpan().CopyTo(span[(4 + state.encodedSender.Length)..]);
                    span[^2] = '\u0001';
                });

            using var packet = new UnmanagedStruct<UIPackets.UIChatMessage>(new UIPackets.UIChatMessage(channel, encoded, channel));
            this.uIHandlingService.SendMessage(UIMessage.WriteToChatLog, (nuint)packet.Address, 0x0);
        }, cancellationToken);
    }
}

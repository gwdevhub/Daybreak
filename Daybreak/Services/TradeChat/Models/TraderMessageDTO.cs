using System;

namespace Daybreak.Services.TradeChat.Models;

public sealed class TraderMessageDTO
{
    public TraderSource TraderSource { get; init; }

    public string Message { get; init; } = string.Empty;

    public string Sender { get; init; } = string.Empty;

    public DateTime Timestamp { get; init; }

    public long Id { get; init; }
}

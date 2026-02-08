namespace Daybreak.Services.TradeChat.Models;

public sealed class TraderMessageDTO
{
    public required long Id { get; init; }

    public int TraderSource { get; init; }

    public string Message { get; init; } = string.Empty;

    public string Sender { get; init; } = string.Empty;

    public DateTime Timestamp { get; init; }
}

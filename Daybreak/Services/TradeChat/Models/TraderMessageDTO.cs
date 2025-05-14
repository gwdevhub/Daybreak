using Squealify;
using System;

namespace Daybreak.Services.TradeChat.Models;

[Table("traderMessages")]
public partial class TraderMessageDTO
{
    [PrimaryKey]
    public required long Id { get; init; }

    public int TraderSource { get; init; }

    public string Message { get; init; } = string.Empty;

    public string Sender { get; init; } = string.Empty;

    public DateTimeOffset Timestamp { get; init; }
}

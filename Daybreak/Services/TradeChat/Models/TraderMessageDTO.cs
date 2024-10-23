using Realms;
using System;

namespace Daybreak.Services.TradeChat.Models;

public sealed class TraderMessageDTO : RealmObject
{
    public int TraderSource { get; init; }

    public string Message { get; init; } = string.Empty;

    public string Sender { get; init; } = string.Empty;

    public DateTimeOffset Timestamp { get; init; }
    [PrimaryKey]
    public long Id { get; init; }
}

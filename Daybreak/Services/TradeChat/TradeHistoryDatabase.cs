using Daybreak.Services.Database;
using Daybreak.Services.TradeChat.Models;
using System;
using System.Collections.Generic;
using System.Core.Extensions;

namespace Daybreak.Services.TradeChat;

internal sealed class TradeHistoryDatabase : ITradeHistoryDatabase
{
    private readonly IDatabaseCollection<TraderMessageDTO> liteCollection;

    public TradeHistoryDatabase(
        IDatabaseCollection<TraderMessageDTO> liteCollection)
    {
        this.liteCollection = liteCollection.ThrowIfNull();
    }

    public IEnumerable<TraderMessageDTO> GetTraderMessagesSinceTime(DateTimeOffset since)
    {
        return this.liteCollection.FindAll(t => t.Timestamp > since);
    }

    public bool StoreTraderMessage(TraderMessageDTO message)
    {
        return this.liteCollection.Update(message);
    }
}

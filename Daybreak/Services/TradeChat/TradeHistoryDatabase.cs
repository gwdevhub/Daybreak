using Daybreak.Services.TradeChat.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Core.Extensions;

namespace Daybreak.Services.TradeChat;

internal sealed class TradeHistoryDatabase : ITradeHistoryDatabase
{
    private readonly ILiteCollection<TraderMessageDTO> liteCollection;

    public TradeHistoryDatabase(
        ILiteCollection<TraderMessageDTO> liteCollection)
    {
        this.liteCollection = liteCollection.ThrowIfNull();
    }

    public IEnumerable<TraderMessageDTO> GetTraderMessagesSinceTime(DateTime since)
    {
        return this.liteCollection.Find(t => t.Timestamp > since);
    }

    public void StoreTraderMessage(TraderMessageDTO message)
    {
        this.liteCollection.Upsert(message.Id, message);
    }
}

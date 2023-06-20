using Daybreak.Models.Guildwars;
using Daybreak.Services.TradeChat.Models;
using LiteDB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.TradeChat;
public sealed class PriceHistoryDatabase : IPriceHistoryDatabase
{
    private readonly IItemHashService itemHashService;
    private readonly ILiteCollection<TraderQuoteDTO> collection;
    private readonly ILogger<PriceHistoryDatabase> logger;

    public PriceHistoryDatabase(
        IItemHashService itemHashService,
        ILiteCollection<TraderQuoteDTO> collection,
        ILogger<PriceHistoryDatabase> logger)
    {
        this.itemHashService = itemHashService.ThrowIfNull();
        this.collection = collection.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public void AddTraderQuotes(IEnumerable<TraderQuoteDTO> traderQuotes)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.AddTraderQuotes), string.Empty);
        scopedLogger.LogDebug("Inserting quotes");
        foreach(var quote in traderQuotes)
        {
            this.collection.Upsert(quote.Id, quote);
        }

        scopedLogger.LogDebug("Inserted quotes");
    }

    public IEnumerable<TraderQuoteDTO> GetLatestQuotes(TraderQuoteType traderQuoteType)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestQuotes), string.Empty);
        scopedLogger.LogDebug($"Retrieving latest quotes");
        var items = this.collection.Find(t => t.IsLatest == true && t.TraderQuoteType == traderQuoteType);
        scopedLogger.LogDebug($"Retrieved latest quotes");
        return items;
    }

    public IEnumerable<TraderQuoteDTO> GetQuoteHistory(ItemBase item, DateTime? fromTimestamp, DateTime? toTimestamp)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetQuoteHistory), item.Id.ToString());
        fromTimestamp ??= DateTime.MinValue;
        toTimestamp ??= DateTime.MaxValue;
        scopedLogger.LogDebug($"Retrieving quotes for item {item.Id} with timestamp between [{fromTimestamp}] and [{toTimestamp}]");
        var modifiersHash = item.Modifiers is not null ? this.itemHashService.ComputeHash(item) : default;
        var items = this.collection.Find(t =>
            t.ItemId == item.Id &&
            t.ModifiersHash == modifiersHash &&
            t.TimeStamp >= fromTimestamp &&
            t.TimeStamp <= toTimestamp);
        scopedLogger.LogDebug($"Retrieved quotes for item {item.Id}");
        return items;
    }

    public IEnumerable<TraderQuoteDTO> GetQuotesByTimestamp(TraderQuoteType type, DateTime? from = default, DateTime? to = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetQuotesByTimestamp), string.Empty);
        from ??= DateTime.MinValue;
        to ??= DateTime.Now;

        scopedLogger.LogDebug($"Retrieving all quotes by timestamp between [{from}] and [{to}]");
        var items = this.collection.Find(t => t.TimeStamp >= from && t.TimeStamp <= to && t.TraderQuoteType == type);
        scopedLogger.LogDebug("Retrieved quotes by timestamp");
        return items;
    }

    public IEnumerable<TraderQuoteDTO> GetQuotesByInsertionTime(TraderQuoteType type, DateTime? from = default, DateTime? to = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetQuotesByInsertionTime), string.Empty);
        from ??= DateTime.MinValue;
        to ??= DateTime.Now;

        scopedLogger.LogDebug($"Retrieving all quotes by insertion time between [{from}] and [{to}]");
        var items = this.collection.Find(t => t.InsertionTime >= from && t.InsertionTime <= to && t.TraderQuoteType == type);
        scopedLogger.LogDebug("Retrieved quotes by insertion time");
        return items;
    }
}

using Daybreak.Models.Guildwars;
using Daybreak.Services.Database;
using Daybreak.Services.TradeChat.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;

namespace Daybreak.Services.TradeChat;
internal sealed class PriceHistoryDatabase : IPriceHistoryDatabase
{
    private readonly IItemHashService itemHashService;
    private readonly IDatabaseCollection<TraderQuoteDTO> collection;
    private readonly ILogger<PriceHistoryDatabase> logger;

    public PriceHistoryDatabase(
        IItemHashService itemHashService,
        IDatabaseCollection<TraderQuoteDTO> collection,
        ILogger<PriceHistoryDatabase> logger)
    {
        this.itemHashService = itemHashService.ThrowIfNull();
        this.collection = collection.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public bool AddTraderQuotes(IEnumerable<TraderQuoteDTO> traderQuotes)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.AddTraderQuotes), string.Empty);
        scopedLogger.LogDebug("Inserting quotes");
        this.collection.AddBulk(traderQuotes);
        scopedLogger.LogDebug("Inserted quotes");
        return true;
    }

    public IEnumerable<TraderQuoteDTO> GetLatestQuotes(TraderQuoteType traderQuoteType)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestQuotes), string.Empty);
        scopedLogger.LogDebug($"Retrieving latest quotes");
        var items = this.collection.FindAll(t => t.TraderQuoteType == (int)traderQuoteType).OrderByDescending(q => q.InsertionTime).ToList();
        var latestItems = items.GroupBy(q => q.ItemId).Select(g => g.First()).ToList();
        scopedLogger.LogDebug($"Retrieved latest quotes");
        return latestItems;
    }

    public IEnumerable<TraderQuoteDTO> GetQuoteHistory(ItemBase item, DateTime? fromTimestamp, DateTime? toTimestamp)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetQuoteHistory), item.Id.ToString());
        var fromO = fromTimestamp.HasValue ?
            fromTimestamp.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.MinValue;
        var toO = toTimestamp.HasValue ?
            toTimestamp.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.MaxValue;
        scopedLogger.LogDebug($"Retrieving quotes for item {item.Id} with timestamp between [{fromO}] and [{toO}]");
        var modifiersHash = item.Modifiers is not null ? this.itemHashService.ComputeHash(item) : default;
        var items = this.collection.FindAll(t =>
            t.ItemId == item.Id &&
            t.ModifiersHash == modifiersHash &&
            t.TimeStamp >= fromO &&
            t.TimeStamp <= toO &&
            t.TraderQuoteType == (int)TraderQuoteType.Sell);
        scopedLogger.LogDebug($"Retrieved quotes for item {item.Id}");
        return items;
    }

    public IEnumerable<TraderQuoteDTO> GetQuotesByTimestamp(TraderQuoteType type, DateTime? from = default, DateTime? to = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetQuotesByTimestamp), string.Empty);
        var fromO = from.HasValue ?
            from.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.MinValue;
        var toO = to.HasValue ?
            to.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.Now;
        scopedLogger.LogDebug($"Retrieving all quotes by timestamp between [{fromO}] and [{toO}]");
        var items = this.collection.FindAll(t => t.TimeStamp >= fromO && t.TimeStamp <= toO && t.TraderQuoteType == (int)type);
        scopedLogger.LogDebug("Retrieved quotes by timestamp");
        return items;
    }

    public IEnumerable<TraderQuoteDTO> GetQuotesByInsertionTime(TraderQuoteType type, DateTime? from = default, DateTime? to = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetQuotesByInsertionTime), string.Empty);
        var fromO = from.HasValue ?
            from.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.MinValue;
        var toO = to.HasValue ?
            to.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.Now;
        scopedLogger.LogDebug($"Retrieving all quotes by insertion time between [{fromO}] and [{toO}]");
        var items = this.collection.FindAll(t => t.InsertionTime >= fromO && t.InsertionTime <= toO && t.TraderQuoteType == (int)type);
        scopedLogger.LogDebug("Retrieved quotes by insertion time");
        return items;
    }
}

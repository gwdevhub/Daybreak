using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using Daybreak.Services.TradeChat.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.TradeChat;
internal sealed class PriceHistoryDatabase : IPriceHistoryDatabase
{
    private readonly IItemHashService itemHashService;
    private readonly TradeQuoteDbContext collection;
    private readonly ILogger<PriceHistoryDatabase> logger;

    public PriceHistoryDatabase(
        IItemHashService itemHashService,
        TradeQuoteDbContext collection,
        ILogger<PriceHistoryDatabase> logger)
    {
        this.itemHashService = itemHashService.ThrowIfNull();
        this.collection = collection.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async ValueTask<bool> AddTraderQuotes(IEnumerable<TraderQuoteDTO> traderQuotes, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.AddTraderQuotes), string.Empty);
        scopedLogger.LogDebug("Inserting quotes");
        using var transaction = await this.collection.CreateTransaction(cancellationToken);
        foreach(var quote in traderQuotes)
        {
            await this.collection.Insert(quote, cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        scopedLogger.LogDebug("Inserted quotes");
        return true;
    }

    public async ValueTask<IEnumerable<TraderQuoteDTO>> GetLatestQuotes(TraderQuoteType traderQuoteType, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestQuotes), string.Empty);
        scopedLogger.LogDebug($"Retrieving latest quotes");
        var items = await this.collection
            .FindAll(cancellationToken)
            .Where(t => t.TraderQuoteType == (int)traderQuoteType).OrderByDescending(q => q.InsertionTime)
            .GroupBy(q => q.ItemId)
            .SelectAwait(async g => await g.FirstAsync(cancellationToken))
            .ToListAsync(cancellationToken);
        scopedLogger.LogDebug($"Retrieved latest quotes");
        return items;
    }

    public async ValueTask<IEnumerable<TraderQuoteDTO>> GetQuoteHistory(ItemBase item, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken)
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
        var items = await this.collection.FindAll(cancellationToken)
            .Where(t =>
                t.ItemId == item.Id &&
                t.ModifiersHash == modifiersHash &&
                t.TimeStamp >= fromO &&
                t.TimeStamp <= toO &&
                t.TraderQuoteType == (int)TraderQuoteType.Sell)
            .ToListAsync(cancellationToken);
        scopedLogger.LogDebug($"Retrieved quotes for item {item.Id}");
        return items;
    }

    public async ValueTask<IEnumerable<TraderQuoteDTO>> GetQuotesByTimestamp(TraderQuoteType type, DateTime? from = default, DateTime? to = default, CancellationToken cancellationToken = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetQuotesByTimestamp), string.Empty);
        var fromO = from.HasValue ?
            from.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.MinValue;
        var toO = to.HasValue ?
            to.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.Now;
        scopedLogger.LogDebug($"Retrieving all quotes by timestamp between [{fromO}] and [{toO}]");
        var items = await this.collection
            .FindAll(cancellationToken)
            .Where(t => t.TimeStamp >= fromO && t.TimeStamp <= toO && t.TraderQuoteType == (int)type)
            .ToListAsync(cancellationToken);
        scopedLogger.LogDebug("Retrieved quotes by timestamp");
        return items;
    }

    public async ValueTask<IEnumerable<TraderQuoteDTO>> GetQuotesByInsertionTime(TraderQuoteType type, DateTime? from = default, DateTime? to = default, CancellationToken cancellationToken = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetQuotesByInsertionTime), string.Empty);
        var fromO = from.HasValue ?
            from.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.MinValue;
        var toO = to.HasValue ?
            to.Value.ToSafeDateTimeOffset() :
            DateTimeOffset.Now;
        scopedLogger.LogDebug($"Retrieving all quotes by insertion time between [{fromO}] and [{toO}]");
        var items = await this.collection
            .FindAll(cancellationToken)
            .Where(t => t.InsertionTime >= fromO && t.InsertionTime <= toO && t.TraderQuoteType == (int)type)
            .ToListAsync(cancellationToken);
        scopedLogger.LogDebug("Retrieved quotes by insertion time");
        return items;
    }
}

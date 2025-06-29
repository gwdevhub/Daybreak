using Daybreak.Services.TradeChat.Models;
using System.Core.Extensions;

namespace Daybreak.Services.TradeChat;

internal sealed class TradeHistoryDatabase : ITradeHistoryDatabase
{
    private readonly TradeMessagesDbContext liteCollection;

    public TradeHistoryDatabase(
        TradeMessagesDbContext liteCollection)
    {
        this.liteCollection = liteCollection.ThrowIfNull();
    }

    public async ValueTask<IEnumerable<TraderMessageDTO>> GetTraderMessagesSinceTime(DateTimeOffset since, CancellationToken cancellationToken)
    {
        return await this.liteCollection
            .FindAll(cancellationToken)
            .Where(t => t.Timestamp > since)
            .ToListAsync(cancellationToken);
    }

    public async ValueTask<bool> StoreTraderMessage(TraderMessageDTO message, CancellationToken cancellationToken)
    {
        await this.liteCollection.Update(message, cancellationToken);
        return true;
    }
}

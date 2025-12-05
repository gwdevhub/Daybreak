using Daybreak.Services.TradeChat.Models;
using System.Core.Extensions;

namespace Daybreak.Services.TradeChat;

internal sealed class TradeHistoryDatabase(
    TradeMessagesDbContext liteCollection) : ITradeHistoryDatabase
{
    private readonly TradeMessagesDbContext liteCollection = liteCollection.ThrowIfNull();

    public async ValueTask<IEnumerable<TraderMessageDTO>> GetTraderMessagesSinceTime(DateTimeOffset since, CancellationToken cancellationToken)
    {
        return await this.liteCollection
            .FindAll(cancellationToken)
            .Where(async (t, ct) => await Task.FromResult(t.Timestamp > since))
            .ToListAsync(cancellationToken);
    }

    public async ValueTask<bool> StoreTraderMessage(TraderMessageDTO message, CancellationToken cancellationToken)
    {
        await this.liteCollection.Update(message, cancellationToken);
        return true;
    }
}

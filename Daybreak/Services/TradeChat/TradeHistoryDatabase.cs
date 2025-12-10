using System.Extensions;
using Daybreak.Services.TradeChat.Models;

namespace Daybreak.Services.TradeChat;

internal sealed class TradeHistoryDatabase : ITradeHistoryDatabase
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly List<TraderMessageDTO> inMemoryStore = [];

    public async ValueTask<IEnumerable<TraderMessageDTO>> GetTraderMessagesSinceTime(DateTimeOffset since, CancellationToken cancellationToken)
    {
        using var ctx = await this.semaphore.Acquire(cancellationToken);
        return [.. this.inMemoryStore.Where(t => t.Timestamp > since)];
    }

    public async ValueTask<bool> StoreTraderMessage(TraderMessageDTO message, CancellationToken cancellationToken)
    {
        using var ctx = await this.semaphore.Acquire(cancellationToken);
        this.inMemoryStore.Add(message);
        return true;
    }
}

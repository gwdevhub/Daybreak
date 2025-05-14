using Daybreak.Services.TradeChat.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.TradeChat;

public interface ITradeHistoryDatabase
{
    ValueTask<IEnumerable<TraderMessageDTO>> GetTraderMessagesSinceTime(DateTimeOffset since, CancellationToken cancellationToken);
    ValueTask<bool> StoreTraderMessage(TraderMessageDTO message, CancellationToken cancellationToken);
}

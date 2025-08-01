﻿using Daybreak.Services.TradeChat.Models;

namespace Daybreak.Services.TradeChat;

public interface ITradeHistoryDatabase
{
    ValueTask<IEnumerable<TraderMessageDTO>> GetTraderMessagesSinceTime(DateTimeOffset since, CancellationToken cancellationToken);
    ValueTask<bool> StoreTraderMessage(TraderMessageDTO message, CancellationToken cancellationToken);
}

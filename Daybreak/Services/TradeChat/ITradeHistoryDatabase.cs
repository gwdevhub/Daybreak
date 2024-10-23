using Daybreak.Services.TradeChat.Models;
using System;
using System.Collections.Generic;

namespace Daybreak.Services.TradeChat;

public interface ITradeHistoryDatabase
{
    IEnumerable<TraderMessageDTO> GetTraderMessagesSinceTime(DateTimeOffset since);
    bool StoreTraderMessage(TraderMessageDTO message);
}

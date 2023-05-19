using Daybreak.Services.TradeChat.Models;
using System;
using System.Collections.Generic;

namespace Daybreak.Services.TradeChat;

public interface IPriceHistoryDatabase
{
    IEnumerable<TraderQuoteDTO> GetQuoteHistory(int itemId, DateTime? fromTimestamp, DateTime? toTimestamp);

    void AddTraderQuotes(IEnumerable<TraderQuoteDTO> traderQuotes);

    IEnumerable<TraderQuoteDTO> GetQuotesByTimestamp(TraderQuoteType type, DateTime? from = default, DateTime? to = default);

    IEnumerable<TraderQuoteDTO> GetQuotesByInsertionTime(TraderQuoteType type, DateTime? from = default, DateTime? to = default);
}

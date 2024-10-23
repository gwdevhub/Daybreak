using Daybreak.Models.Guildwars;
using Daybreak.Services.TradeChat.Models;
using System;
using System.Collections.Generic;

namespace Daybreak.Services.TradeChat;

public interface IPriceHistoryDatabase
{
    IEnumerable<TraderQuoteDTO> GetLatestQuotes(TraderQuoteType traderQuoteType);

    IEnumerable<TraderQuoteDTO> GetQuoteHistory(ItemBase item, DateTime? fromTimestamp, DateTime? toTimestamp);

    bool AddTraderQuotes(IEnumerable<TraderQuoteDTO> traderQuotes);

    IEnumerable<TraderQuoteDTO> GetQuotesByTimestamp(TraderQuoteType type, DateTime? from = default, DateTime? to = default);

    IEnumerable<TraderQuoteDTO> GetQuotesByInsertionTime(TraderQuoteType type, DateTime? from = default, DateTime? to = default);
}

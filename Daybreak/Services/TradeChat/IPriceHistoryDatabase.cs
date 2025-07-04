﻿using Daybreak.Services.TradeChat.Models;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Trade;

namespace Daybreak.Services.TradeChat;

public interface IPriceHistoryDatabase
{
    ValueTask<IEnumerable<TraderQuoteDTO>> GetLatestQuotes(TraderQuoteType traderQuoteType, CancellationToken cancellationToken);

    ValueTask<IEnumerable<TraderQuoteDTO>> GetQuoteHistory(ItemBase item, DateTime? fromTimestamp, DateTime? toTimestamp, CancellationToken cancellationToken);

    ValueTask<bool> AddTraderQuotes(IEnumerable<TraderQuoteDTO> traderQuotes, CancellationToken cancellationToken);

    ValueTask<IEnumerable<TraderQuoteDTO>> GetQuotesByTimestamp(TraderQuoteType type, DateTime? from = default, DateTime? to = default, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<TraderQuoteDTO>> GetQuotesByInsertionTime(TraderQuoteType type, DateTime? from = default, DateTime? to = default, CancellationToken cancellationToken = default);
}

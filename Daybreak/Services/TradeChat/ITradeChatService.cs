﻿using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.TradeChat;

public interface ITradeChatService<TChannelOptions> : ITradeChatService
    where TChannelOptions : class, ITradeChatOptions, new()
{
}

public interface ITradeChatService
{
    IAsyncEnumerable<TraderMessage> GetLiveTraderMessages(CancellationToken cancellationToken);

    Task<IEnumerable<TraderMessage>> GetLatestTrades(CancellationToken cancellationToken, DateTime? from = default);

    Task<IEnumerable<TraderMessage>> GetTradesByQuery(string query, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default);

    Task<IEnumerable<TraderMessage>> GetTradesByUsername(string username, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default);
}

﻿using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using Daybreak.Services.TradeChat.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.TradeChat;

/// <summary>
/// Based on https://github.com/3vcloud/kamadan-trade-chat
/// </summary>
/// <typeparam name="TChannelOptions"></typeparam>
public sealed class TraderQuoteService : ITraderQuoteService
{
    private const string TraderQuotesUri = "trader_quotes";

    private readonly IPriceHistoryDatabase priceHistoryDatabase;
    private readonly ILiveUpdateableOptions<TraderQuotesOptions> options;
    private readonly IHttpClient<TraderQuoteService> httpClient;
    private readonly ILogger<TraderQuoteService> logger;

    public TraderQuoteService(
        IPriceHistoryDatabase priceHistoryDatabase,
        ILiveUpdateableOptions<TraderQuotesOptions> options,
        IHttpClient<TraderQuoteService> client,
        ILogger<TraderQuoteService> logger)
    {
        this.priceHistoryDatabase = priceHistoryDatabase.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.httpClient = client.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.httpClient.BaseAddress = new Uri(this.options.Value.HttpsUri);
    }

    public Task<IEnumerable<TraderQuote>> GetBuyQuotes(CancellationToken cancellationToken)
    {
        return this.GetBuyQuotesInternal(cancellationToken);
    }

    public Task<IEnumerable<TraderQuote>> GetSellQuotes(CancellationToken cancellationToken)
    {
        return this.GetSellQuotesInternal(cancellationToken);
    }

    private async Task<IEnumerable<TraderQuote>> GetBuyQuotesInternal(CancellationToken cancellationToken)
    {
        if (DateTime.UtcNow >= this.options.Value.LastCheckTime + TimeSpan.FromMinutes(this.options.Value.CacheUpdateInterval))
        {
            (var buyQuotes, _) = await this.FetchAndCacheQuotes(cancellationToken);
            return buyQuotes;
        }

        var quotes = this.priceHistoryDatabase.GetQuotesByInsertionTime(TraderQuoteType.Buy, this.options.Value.LastCheckTime);
        var retList = new List<TraderQuote>();
        foreach(var quoteDTO in quotes)
        {
            if (!ItemBase.TryParse(quoteDTO.ItemId, out var item))
            {
                continue;
            }

            retList.Add(new TraderQuote
            {
                Item = item,
                Price = quoteDTO.Price,
                Timestamp = quoteDTO.TimeStamp,
            });
        }

        return retList;
    }

    private async Task<IEnumerable<TraderQuote>> GetSellQuotesInternal(CancellationToken cancellationToken)
    {
        if (DateTime.UtcNow >= this.options.Value.LastCheckTime + TimeSpan.FromMinutes(this.options.Value.CacheUpdateInterval))
        {
            (_, var sellQuotes) = await this.FetchAndCacheQuotes(cancellationToken);
            return sellQuotes;
        }

        var quotes = this.priceHistoryDatabase.GetQuotesByInsertionTime(TraderQuoteType.Sell, this.options.Value.LastCheckTime);
        var retList = new List<TraderQuote>();
        foreach (var quoteDTO in quotes)
        {
            if (!ItemBase.TryParse(quoteDTO.ItemId, out var item))
            {
                continue;
            }

            retList.Add(new TraderQuote
            {
                Item = item,
                Price = quoteDTO.Price,
                Timestamp = quoteDTO.TimeStamp,
            });
        }

        return retList;
    }

    private async Task<(IEnumerable<TraderQuote> BuyQuotes, IEnumerable<TraderQuote> SellQuotes)> FetchAndCacheQuotes(CancellationToken cancellationToken)
    {
        var buyQuotes = await this.FetchBuyQuotesInternal(cancellationToken);
        var sellQuotes = await this.FetchSellQuotesInternal(cancellationToken);
        var insertionTime = DateTime.UtcNow;
        this.priceHistoryDatabase.AddTraderQuotes(buyQuotes
            .Select(
                quote => new TraderQuoteDTO
                { 
                    Price = quote.Price,
                    ItemId = quote.Item?.Id ?? 0,
                    InsertionTime = insertionTime,
                    TimeStamp = quote.Timestamp ?? insertionTime,
                    TraderQuoteType = TraderQuoteType.Buy,
                }));

        this.priceHistoryDatabase.AddTraderQuotes(sellQuotes
            .Select(
                quote => new TraderQuoteDTO
                {
                    Price = quote.Price,
                    ItemId = quote.Item?.Id ?? 0,
                    InsertionTime = insertionTime,
                    TimeStamp = quote.Timestamp ?? insertionTime,
                    TraderQuoteType = TraderQuoteType.Sell
                }));

        this.options.Value.LastCheckTime = insertionTime;
        this.options.UpdateOption();
        return (buyQuotes, sellQuotes);
    }

    private async Task<IEnumerable<TraderQuote>> FetchBuyQuotesInternal(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetBuyQuotes), string.Empty);
        try
        {
            var content = await this.GetAsync(TraderQuotesUri, default, scopedLogger, cancellationToken);
            var response = JsonConvert.DeserializeObject<TraderQuotesResponse>(content);
            var responseList = new List<TraderQuote>();
            foreach (var buyQuote in response?.BuyQuotes!)
            {
                var itemIdString = buyQuote.Key;
                var quote = buyQuote.Value;
                if (!int.TryParse(itemIdString, out var itemId) ||
                    !ItemBase.TryParse(itemId, out var item))
                {
                    scopedLogger.LogInformation($"Unable to parse item {itemIdString}. Skipping quote");
                    continue;
                }

                var traderQuote = new TraderQuote { Item = item, Price = quote.Price };
                responseList.Add(traderQuote);
            }

            return responseList;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return Enumerable.Empty<TraderQuote>();
        }
    }

    private async Task<IEnumerable<TraderQuote>> FetchSellQuotesInternal(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetSellQuotes), string.Empty);
        try
        {
            var content = await this.GetAsync(TraderQuotesUri, default, scopedLogger, cancellationToken);
            var response = JsonConvert.DeserializeObject<TraderQuotesResponse>(content);
            var responseList = new List<TraderQuote>();
            foreach (var buyQuote in response?.SellQuotes!)
            {
                var itemIdString = buyQuote.Key;
                var quote = buyQuote.Value;
                if (!int.TryParse(itemIdString, out var itemId) ||
                    !ItemBase.TryParse(itemId, out var item))
                {
                    scopedLogger.LogInformation($"Unable to parse item {itemIdString}. Skipping quote");
                    continue;
                }

                var traderQuote = new TraderQuote { Item = item, Price = quote.Price };
                responseList.Add(traderQuote);
            }

            return responseList;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return Enumerable.Empty<TraderQuote>();
        }
    }

    private async Task<string> GetAsync(string? uri, HttpRequestMessage? httpRequestMessage, ScopedLogger<TraderQuoteService> scopedLogger, CancellationToken cancellationToken)
    {
        var response = httpRequestMessage is not null ?
            await this.httpClient.SendAsync(httpRequestMessage) :
            await this.httpClient.GetAsync(uri!, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Error occurred while retrieving trades from {this.httpClient.BaseAddress}/{uri}");
            throw new InvalidOperationException($"Received [{response.StatusCode}] from [{this.httpClient.BaseAddress}/{uri}]");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return content;
    }
}

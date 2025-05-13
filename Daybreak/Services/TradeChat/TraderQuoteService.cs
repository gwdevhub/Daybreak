using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using Daybreak.Services.TradeChat.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
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
internal sealed class TraderQuoteService : ITraderQuoteService
{
    private const string TraderQuotesUri = "trader_quotes";

    private readonly IItemHashService itemHashService;
    private readonly IPriceHistoryDatabase priceHistoryDatabase;
    private readonly ILiveUpdateableOptions<TraderQuotesOptions> options;
    private readonly IHttpClient<TraderQuoteService> httpClient;
    private readonly ILogger<TraderQuoteService> logger;

    public TraderQuoteService(
        IItemHashService itemHashService,
        IPriceHistoryDatabase priceHistoryDatabase,
        ILiveUpdateableOptions<TraderQuotesOptions> options,
        IHttpClient<TraderQuoteService> client,
        ILogger<TraderQuoteService> logger)
    {
        this.itemHashService = itemHashService.ThrowIfNull();
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

        var quotes = await this.priceHistoryDatabase.GetLatestQuotes(TraderQuoteType.Buy, cancellationToken);
        var retList = new List<TraderQuote>();
        foreach(var quoteDTO in quotes)
        {
            if (ItemBase.AllItems.FirstOrDefault(i =>
                i.Id == quoteDTO.ItemId &&
                (i.Modifiers is null || this.itemHashService.ComputeHash(i) == quoteDTO.ModifiersHash)) is not ItemBase item)
            {
                continue;
            }

            retList.Add(new TraderQuote
            {
                Item = item,
                Price = quoteDTO.Price,
                Timestamp = quoteDTO.TimeStamp.LocalDateTime,
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

        var quotes = await this.priceHistoryDatabase.GetLatestQuotes(TraderQuoteType.Sell, cancellationToken);
        var retList = new List<TraderQuote>();
        foreach (var quoteDTO in quotes)
        {
            if (ItemBase.AllItems.FirstOrDefault(i =>
                i.Id == quoteDTO.ItemId &&
                (i.Modifiers is null || this.itemHashService.ComputeHash(i) == quoteDTO.ModifiersHash)) is not ItemBase item)
            {
                continue;
            }

            retList.Add(new TraderQuote
            {
                Item = item,
                Price = quoteDTO.Price,
                Timestamp = quoteDTO.TimeStamp.LocalDateTime,
            });
        }

        return retList;
    }

    private async Task<(IEnumerable<TraderQuote> BuyQuotes, IEnumerable<TraderQuote> SellQuotes)> FetchAndCacheQuotes(CancellationToken cancellationToken)
    {
        var buyQuotes = await this.FetchBuyQuotesInternal(cancellationToken);
        var sellQuotes = await this.FetchSellQuotesInternal(cancellationToken);
        var insertionTime = DateTimeOffset.UtcNow;
        await this.priceHistoryDatabase.AddTraderQuotes(buyQuotes
            .Select(
                quote => new TraderQuoteDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    Price = quote.Price,
                    ItemId = quote.Item?.Id ?? 0,
                    ModifiersHash = quote.Item?.Modifiers is null ? string.Empty : this.itemHashService.ComputeHash(quote.Item),
                    InsertionTime = insertionTime,
                    TimeStamp = quote.Timestamp.ToSafeDateTimeOffset() ?? insertionTime,
                    TraderQuoteType = (int)TraderQuoteType.Buy
                }), cancellationToken);

        await this.priceHistoryDatabase.AddTraderQuotes(sellQuotes
            .Select(
                quote => new TraderQuoteDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    Price = quote.Price,
                    ItemId = quote.Item?.Id ?? 0,
                    ModifiersHash = quote.Item?.Modifiers is null ? string.Empty : this.itemHashService.ComputeHash(quote.Item),
                    InsertionTime = insertionTime,
                    TimeStamp = quote.Timestamp.ToSafeDateTimeOffset() ?? insertionTime,
                    TraderQuoteType = (int)TraderQuoteType.Sell
                }), cancellationToken);

        this.options.Value.LastCheckTime = insertionTime.UtcDateTime;
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
                if (itemIdString.IsNullOrWhiteSpace())
                {
                    continue;
                }

                var idTokens = itemIdString.Split('-');
                if (!int.TryParse(idTokens[0], out var id))
                {
                    scopedLogger.LogInformation($"Unable to parse item id {idTokens[0]}. Skipping quote");
                    continue;
                }

                // TODO: Search by modifiers as well
                var quote = buyQuote.Value;
                if (ItemBase.AllItems.FirstOrDefault(i =>
                        i.Id == id &&
                        (idTokens.Length <= 1 || this.itemHashService.ComputeHash(i)?.StartsWith(idTokens[1]) is true)) is not ItemBase item)
                {
                    scopedLogger.LogInformation($"Unable to parse item {itemIdString}. Skipping quote");
                    continue;
                }

                var traderQuote = new TraderQuote { Item = item, Price = quote.Price, Timestamp = quote.TimeStamp };
                responseList.Add(traderQuote);
            }

            return responseList;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return [];
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
            foreach (var sellQuote in response?.SellQuotes!)
            {
                var itemIdString = sellQuote.Key;
                if (itemIdString.IsNullOrWhiteSpace())
                {
                    continue;
                }

                var idTokens = itemIdString.Split('-');
                if (!int.TryParse(idTokens[0], out var id))
                {
                    scopedLogger.LogInformation($"Unable to parse item id {idTokens[0]}. Skipping quote");
                    continue;
                }

                // TODO: Search by modifiers as well
                var quote = sellQuote.Value;
                if (ItemBase.AllItems.FirstOrDefault(i =>
                        i.Id == id &&
                        (idTokens.Length <= 1 || this.itemHashService.ComputeHash(i)?.StartsWith(idTokens[1]) is true)) is not ItemBase item)
                {
                    scopedLogger.LogInformation($"Unable to parse item {itemIdString}. Skipping quote");
                    continue;
                }

                var traderQuote = new TraderQuote { Item = item, Price = quote.Price, Timestamp = quote.TimeStamp };
                responseList.Add(traderQuote);
            }

            return responseList;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return [];
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

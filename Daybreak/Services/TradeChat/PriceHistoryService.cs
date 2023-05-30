﻿using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using Daybreak.Services.TradeChat.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.TradeChat;

public sealed class PriceHistoryService : IPriceHistoryService
{
    private static readonly DateTime MinEpochTime = DateTimeOffset.FromUnixTimeSeconds(1420070400).UtcDateTime;

    private const string ModelIdPlaceholder = "[MODELID]";
    private const string FromPlaceholder = "[FROM]";
    private const string ToPlaceholder = "[TO]";
    private const string PricingHistory = $"pricing_history/{ModelIdPlaceholder}/{FromPlaceholder}/{ToPlaceholder}";

    private readonly IPriceHistoryDatabase priceHistoryDatabase;
    private readonly ILiveUpdateableOptions<PriceHistoryOptions> options;
    private readonly IHttpClient<PriceHistoryService> httpClient;
    private readonly ILogger<PriceHistoryService> logger;

    public PriceHistoryService(
        IPriceHistoryDatabase priceHistoryDatabase,
        ILiveUpdateableOptions<PriceHistoryOptions> options,
        IHttpClient<PriceHistoryService> client,
        ILogger<PriceHistoryService> logger)
    {
        this.priceHistoryDatabase = priceHistoryDatabase.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.httpClient = client.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.httpClient.BaseAddress = new Uri(this.options.Value.HttpsUri);
    }

    public async Task<IEnumerable<TraderQuote>> GetPriceHistory(ItemBase itemBase, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default)
    {
        return await this.UpdateCacheAndGetPriceHistory(itemBase, cancellationToken, from, to);
    }

    private async Task<IEnumerable<TraderQuote>> UpdateCacheAndGetPriceHistory(ItemBase itemBase, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default)
    {
        if (!this.options.Value.ItemHistoryMetadata.TryGetValue(itemBase.Id, out var lastQueryTime))
        {
            lastQueryTime = MinEpochTime;
        }

        if (lastQueryTime + this.options.Value.UpdateInterval < DateTime.UtcNow)
        {
            await this.FetchAndUpdatePricingHistoryCache(itemBase, cancellationToken, lastQueryTime, DateTime.UtcNow);
            this.options.Value.ItemHistoryMetadata[itemBase.Id] = DateTime.UtcNow;
            this.options.UpdateOption();
        }

        var quotes = this.priceHistoryDatabase.GetQuoteHistory(itemBase.Id, from, to);
        var retList = new List<TraderQuote>();
        foreach(var quote in quotes)
        {
            if (!ItemBase.TryParse(quote.ItemId, out var item))
            {
                continue;
            }

            retList.Add(new TraderQuote
            {
                Item = item,
                Price = quote.Price,
                Timestamp = quote.TimeStamp
            });
        }

        return retList;
    }

    private async Task FetchAndUpdatePricingHistoryCache(ItemBase itemBase, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default)
    {
        var insertionTime = DateTime.UtcNow;
        var quotes = await this.FetchPricingHistoryInternal(itemBase, cancellationToken, from, to);
        this.priceHistoryDatabase.AddTraderQuotes(quotes.Select(quote => new TraderQuoteDTO
        {
            TimeStamp = quote.Timestamp ?? insertionTime,
            ItemId = quote.Item?.Id ?? 0,
            Price = quote.Price,
            TraderQuoteType = TraderQuoteType.Buy,
            InsertionTime = quote.Timestamp ?? insertionTime //Use timestamp as this method basically recreates the kamadan tradechat database
        }));
    }

    private async Task<IEnumerable<TraderQuote>> FetchPricingHistoryInternal(ItemBase itemBase, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.FetchPricingHistoryInternal), itemBase.Id.ToString());
        try
        {
            from ??= MinEpochTime;
            to ??= DateTime.UtcNow;
            var response = await this.httpClient.GetAsync(
                PricingHistory.Replace(ModelIdPlaceholder, itemBase.Id.ToString())
                    .Replace(FromPlaceholder, new DateTimeOffset(from.Value).ToUnixTimeSeconds().ToString())
                    .Replace(ToPlaceholder, new DateTimeOffset(to.Value).ToUnixTimeSeconds().ToString()), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<TraderQuote>();
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var responseList = JsonConvert.DeserializeObject<List<TraderQuotePayload>>(responseString);
            return responseList?.Select(p => new TraderQuote { Item = itemBase, Price = p.Price, Timestamp = p.TimeStamp }) ??
                Enumerable.Empty<TraderQuote>();
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return Enumerable.Empty<TraderQuote>();
        }
    }
}
using Daybreak.Configuration.Options;
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

internal sealed class PriceHistoryService : IPriceHistoryService
{
    private static readonly DateTime MinEpochTime = DateTimeOffset.FromUnixTimeSeconds(1420070400).UtcDateTime;

    private const string ModelIdPlaceholder = "[MODELID]";
    private const string FromPlaceholder = "[FROM]";
    private const string ToPlaceholder = "[TO]";
    private const string PricingHistory = $"pricing_history/{ModelIdPlaceholder}/{FromPlaceholder}/{ToPlaceholder}";

    private readonly IItemHashService itemHashService;
    private readonly IPriceHistoryDatabase priceHistoryDatabase;
    private readonly ILiveUpdateableOptions<PriceHistoryOptions> options;
    private readonly IHttpClient<PriceHistoryService> httpClient;
    private readonly ILogger<PriceHistoryService> logger;

    public PriceHistoryService(
        IItemHashService itemHashService,
        IPriceHistoryDatabase priceHistoryDatabase,
        ILiveUpdateableOptions<PriceHistoryOptions> options,
        IHttpClient<PriceHistoryService> client,
        ILogger<PriceHistoryService> logger)
    {
        this.itemHashService = itemHashService.ThrowIfNull();
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
        var itemId = itemBase.Modifiers is not null ? $"{itemBase.Id}-{this.itemHashService.ComputeHash(itemBase)}" : itemBase.Id.ToString();
        if (!this.options.Value.ItemHistoryMetadata.TryGetValue(itemId, out var lastQueryTime))
        {
            lastQueryTime = MinEpochTime;
        }

        if (lastQueryTime + this.options.Value.UpdateInterval < DateTime.UtcNow)
        {
            await this.FetchAndUpdatePricingHistoryCache(itemBase, cancellationToken, lastQueryTime, DateTime.UtcNow);
            this.options.Value.ItemHistoryMetadata[itemId] = DateTime.UtcNow;
            this.options.UpdateOption();
        }

        var quotes = this.priceHistoryDatabase.GetQuoteHistory(itemBase, from, to);
        var retList = new List<TraderQuote>();
        foreach(var quote in quotes)
        {
            retList.Add(new TraderQuote
            {
                Item = itemBase,
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
            ModifiersHash = quote.Item?.Modifiers is not null ? this.itemHashService.ComputeHash(quote.Item) : default,
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
                PricingHistory.Replace(ModelIdPlaceholder, this.GetItemId(itemBase))
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

    private string GetItemId(ItemBase itemBase)
    {
        if (itemBase is not IItemModHash itemModHash ||
            itemModHash.ModHash?.IsNullOrWhiteSpace() is true)
        {
            return itemBase.Modifiers is null ?
                        itemBase.Id.ToString() :
                        $"{itemBase.Id}-{this.itemHashService.ComputeHash(itemBase)}";
        }

        return $"{itemBase.Id}-{itemModHash.ModHash}";
    }
}

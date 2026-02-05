using Daybreak.Configuration.Options;
using Daybreak.Services.TradeChat.Models;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.TradeChat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions;
using System.Logging;
using System.Text.Json;

namespace Daybreak.Services.TradeChat;

/// <summary>
/// Based on https://github.com/3vcloud/kamadan-trade-chat
/// </summary>
/// <typeparam name="TChannelOptions"></typeparam>
internal sealed class TraderQuoteService : ITraderQuoteService
{
    private const string TraderQuotesUri = "trader_quotes";

    private readonly IItemHashService itemHashService;
    private readonly IOptionsMonitor<TraderQuotesOptions> options;
    private readonly IHttpClient<TraderQuoteService> httpClient;
    private readonly ILogger<TraderQuoteService> logger;

    public TraderQuoteService(
        IItemHashService itemHashService,
        IOptionsMonitor<TraderQuotesOptions> options,
        IHttpClient<TraderQuoteService> client,
        ILogger<TraderQuoteService> logger)
    {
        this.itemHashService = itemHashService.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.httpClient = client.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.httpClient.BaseAddress = new Uri(this.options.CurrentValue.HttpsUri);
    }

    public Task<IEnumerable<TraderQuote>> GetBuyQuotes(CancellationToken cancellationToken)
    {
        return this.FetchBuyQuotesInternal(cancellationToken);
    }

    public Task<IEnumerable<TraderQuote>> GetSellQuotes(CancellationToken cancellationToken)
    {
        return this.FetchSellQuotesInternal(cancellationToken);
    }

    private async Task<IEnumerable<TraderQuote>> FetchBuyQuotesInternal(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetBuyQuotes), string.Empty);
        try
        {
            var content = await this.GetAsync(TraderQuotesUri, default, scopedLogger, cancellationToken);
            var response = JsonSerializer.Deserialize<TraderQuotesResponse>(content);
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
                    scopedLogger.LogWarning("Unable to parse item id {idToken}. Skipping quote", idTokens[0]);
                    continue;
                }

                // TODO: Search by modifiers as well
                var quote = buyQuote.Value;
                if (ItemBase.AllItems.FirstOrDefault(i =>
                        i.Id == id &&
                        (idTokens.Length <= 1 || this.itemHashService.ComputeHash(i)?.StartsWith(idTokens[1]) is true)) is not ItemBase item)
                {
                    scopedLogger.LogWarning("Unable to parse item {itemId}. Skipping quote", itemIdString);
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
            var response = JsonSerializer.Deserialize<TraderQuotesResponse>(content);
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
                    scopedLogger.LogWarning("Unable to parse item id {idToken}. Skipping quote", idTokens[0]);
                    continue;
                }

                // TODO: Search by modifiers as well
                var quote = sellQuote.Value;
                if (ItemBase.AllItems.FirstOrDefault(i =>
                        i.Id == id &&
                        (idTokens.Length <= 1 || this.itemHashService.ComputeHash(i)?.StartsWith(idTokens[1]) is true)) is not ItemBase item)
                {
                    scopedLogger.LogWarning("Unable to parse item {itemId}. Skipping quote", itemIdString);
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
            await this.httpClient.SendAsync(httpRequestMessage, cancellationToken) :
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

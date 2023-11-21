using Daybreak.Configuration.Options;
using Daybreak.Models.Trade;
using Daybreak.Services.TradeChat.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Logging;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.TradeChat;

/// <summary>
/// Client implementation for https://github.com/3vcloud/kamadan-trade-chat
/// </summary>
/// <typeparam name="TChannelOptions"></typeparam>
internal sealed class TradeChatService<TChannelOptions> : ITradeChatService<TChannelOptions>, IDisposable
    where TChannelOptions : class, ITradeChatOptions, new()
{
    private const long MinEpochTime = 1420070400;

    private const string ModelIdPlaceholder = "[MODELID]";
    private const string FromPlaceholder = "[FROM]";
    private const string ToPlaceholder = "[TO]";
    private const string PricingHistory = $"pricing_history/{ModelIdPlaceholder}/{FromPlaceholder}/{ToPlaceholder}";
    private const string TraderQuotesUri = "trader_quotes";
    private const string LatestTradesUri = "m";
    private const string SearchQueryUri = $"s/{QueryPlaceholder}";
    private const string SearchUsernameUri = $"u/{QueryPlaceholder}";
    private const string QueryPlaceholder = "[QUERY]";
    private const string IfNoneMatchHeader = "if-none-match";

    private static readonly TimeSpan MinBackoffTime = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan MaxBackoffTime = TimeSpan.FromSeconds(16);

    private readonly byte[] webSocketBuffer = new byte[1024];
    private readonly IPriceHistoryDatabase priceHistoryDatabase;
    private readonly IOptions<TChannelOptions> options;
    private readonly IClientWebSocket<TradeChatService<TChannelOptions>> clientWebSocket;
    private readonly IHttpClient<TradeChatService<TChannelOptions>> httpClient;
    private readonly ILogger<TradeChatService<TChannelOptions>> logger;

    private TimeSpan backoffTime = MinBackoffTime;

    public TradeChatService(
        IPriceHistoryDatabase priceHistoryDatabase,
        IOptions<TChannelOptions> options,
        IClientWebSocket<TradeChatService<TChannelOptions>> clientWebSocket,
        IHttpClient<TradeChatService<TChannelOptions>> client,
        ILogger<TradeChatService<TChannelOptions>> logger)
    {
        this.priceHistoryDatabase = priceHistoryDatabase.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.clientWebSocket = clientWebSocket.ThrowIfNull();
        this.httpClient = client.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.httpClient.BaseAddress = new Uri(this.options.Value.HttpsUri);
    }

    public async IAsyncEnumerable<TraderMessage> GetLiveTraderMessages([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var maybeTraderMessageResponse = await this.ReceiveTraderMessageResponseSafe(cancellationToken);
            if (maybeTraderMessageResponse is not TraderMessageResponse traderMessageResponse ||
                traderMessageResponse.Sender!.IsNullOrWhiteSpace() ||
                traderMessageResponse.Message!.IsNullOrWhiteSpace() ||
                !traderMessageResponse.Timestamp.HasValue)
            {
                continue;
            }

            yield return new TraderMessage
            {
                Message = traderMessageResponse.Message!,
                Sender = traderMessageResponse.Sender!,
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(traderMessageResponse.Timestamp.Value).ToLocalTime().DateTime
            };
        }
    }

    public async Task<IEnumerable<TraderMessage>> GetLatestTrades(CancellationToken cancellationToken, DateTime? from = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestTrades), string.Empty);
        return await this.GetTradeMessagesInternal(LatestTradesUri, from, scopedLogger, cancellationToken);
    }

    public async Task<IEnumerable<TraderMessage>> GetTradesByQuery(string query, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default)
    {
        var queryBuilder = new StringBuilder();
        queryBuilder.Append(SearchQueryUri.Replace(QueryPlaceholder, query));
        if (from.HasValue)
        {
            var fromUnixTimestamp = new DateTimeOffset(from.Value).ToUnixTimeMilliseconds();
            queryBuilder.Append('/').Append(fromUnixTimestamp);
        }

        if (to.HasValue)
        {
            var fromUnixTimestamp = new DateTimeOffset(to.Value).ToUnixTimeMilliseconds();
            queryBuilder.Append('/').Append(fromUnixTimestamp);
        }

        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetTradesByQuery), query);
        return await this.GetTradeMessagesInternal(queryBuilder.ToString(), default, scopedLogger, cancellationToken);
    }

    public async Task<IEnumerable<TraderMessage>> GetTradesByUsername(string username, CancellationToken cancellationToken, DateTime? from = default, DateTime? to = default)
    {
        var queryBuilder = new StringBuilder();
        queryBuilder.Append(SearchUsernameUri.Replace(QueryPlaceholder, username));
        if (from.HasValue)
        {
            var fromUnixTimestamp = new DateTimeOffset(from.Value).ToUnixTimeMilliseconds();
            queryBuilder.Append('/').Append(fromUnixTimestamp);
        }

        if (to.HasValue)
        {
            var fromUnixTimestamp = new DateTimeOffset(to.Value).ToUnixTimeMilliseconds();
            queryBuilder.Append('/').Append(fromUnixTimestamp);
        }

        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetTradesByUsername), username);
        return await this.GetTradeMessagesInternal(queryBuilder.ToString(), default, scopedLogger, cancellationToken);
        
    }

    private async Task<IEnumerable<TraderMessage>> GetTradeMessagesInternal(string uri, DateTime? from, ScopedLogger<TradeChatService<TChannelOptions>> scopedLogger, CancellationToken cancellationToken)
    {
        try
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(this.options.Value.HttpsUri), uri));
            if (from.HasValue)
            {
                requestMessage.Headers.TryAddWithoutValidation(IfNoneMatchHeader, new DateTimeOffset(from.Value).ToUnixTimeSeconds().ToString());
            }

            var content = await this.GetAsync(uri, requestMessage, scopedLogger, cancellationToken);
            var responses = JsonConvert.DeserializeObject<List<TraderMessageResponse>>(content) ?? Array.Empty<TraderMessageResponse>().As<IEnumerable<TraderMessageResponse>>();
            return responses!.Select(t => new TraderMessage
            {
                Message = t.Message ?? string.Empty,
                Sender = t.Sender ?? string.Empty,
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(t.Timestamp ?? 0).ToLocalTime().DateTime,
            });
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return Enumerable.Empty<TraderMessage>();
        }
    }

    private async Task<string> GetAsync(string? uri, HttpRequestMessage? httpRequestMessage, ScopedLogger<TradeChatService<TChannelOptions>> scopedLogger, CancellationToken cancellationToken)
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

    private async Task<TraderMessageResponse?> ReceiveTraderMessageResponseSafe(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReceiveTraderMessageResponseSafe), string.Empty);
        try
        {
            var responseString = await this.ReceiveWebsocketResponseSafe(cancellationToken);
            return JsonConvert.DeserializeObject<TraderMessageResponse>(responseString) ?? default;
        }
        catch (JsonSerializationException ex)
        {
            scopedLogger.LogError(ex, "Encountered serialization exception. Returning default response");
            return default;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception. Waiting 1s and retrying");
            await Task.Delay(1000);
            return await this.ReceiveTraderMessageResponseSafe(cancellationToken);
        }
    }

    private async Task<string> ReceiveWebsocketResponseSafe(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(ReceiveWebsocketResponseSafe), string.Empty);

        try
        {
            var response = await this.ReceiveWebsocketResponse(cancellationToken);
            this.backoffTime = MinBackoffTime;
            return response;
        }
        catch (OperationCanceledException ex)
        {
            this.logger.LogError(ex, "Receive operation timed out");
            return string.Empty;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, $"Encountered exception while receiving response. Waiting {this.backoffTime.TotalSeconds}s and retrying");
            await Task.Delay(this.backoffTime, cancellationToken);
            this.backoffTime = TimeSpan.FromSeconds(Math.Min(this.backoffTime.TotalSeconds * 2, MaxBackoffTime.TotalSeconds));
            return await this.ReceiveWebsocketResponseSafe(cancellationToken);
        }
    }

    private async Task<string> ReceiveWebsocketResponse(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ReceiveWebsocketResponse), string.Empty);
        await this.EnsureConnectionAsync(cancellationToken);
        var buffer = new ArraySegment<byte>(this.webSocketBuffer, 0, this.webSocketBuffer.Length);
        using var finalResponseBuffer = new MemoryStream();
        var endOfMessage = false;
        var textMessage = false;
        do
        {
            var response = await this.clientWebSocket.ReceiveAsync(buffer, cancellationToken);
            if (response.MessageType is WebSocketMessageType.Close)
            {
                scopedLogger.LogInformation("Received close. Attempting to reconnect");
                await this.EnsureConnectionAsync(cancellationToken);
                continue;
            }

            if (response.MessageType is WebSocketMessageType.Text)
            {
                textMessage = true;
            }

            finalResponseBuffer.Write(buffer.Array!, 0, response.Count);
            endOfMessage = response.EndOfMessage;
        } while (!endOfMessage);

        finalResponseBuffer.Position = 0;
        using var streamReader = new StreamReader(finalResponseBuffer, textMessage ? Encoding.ASCII : Encoding.UTF8);
        var parsedStringMessage = await streamReader.ReadToEndAsync();
        return parsedStringMessage;
    }

    private async Task EnsureConnectionAsync(CancellationToken cancellationToken)
    {
        if (this.clientWebSocket.State is WebSocketState.Open)
        {
            return;
        }

        this.clientWebSocket.RefreshSocket();
        await this.clientWebSocket.ConnectAsync(new Uri(this.options.Value.WssUri), cancellationToken);
    }

    public void Dispose()
    {
        this.clientWebSocket.Dispose();
    }
}

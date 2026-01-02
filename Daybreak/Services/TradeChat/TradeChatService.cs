using Daybreak.Services.TradeChat.Models;
using Daybreak.Shared.Configuration.Options;
using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.TradeChat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.IO;
using System.Logging;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Daybreak.Services.TradeChat;

/// <summary>
/// Client implementation for https://github.com/3vcloud/kamadan-trade-chat
/// </summary>
/// <typeparam name="TChannelOptions"></typeparam>
internal sealed class TradeChatService<TChannelOptions> : ITradeChatService<TChannelOptions>, IDisposable
    where TChannelOptions : class, ITradeChatOptions, new()
{
    private const string LatestTradesUri = "m";
    private const string IfNoneMatchHeader = "if-none-match";

    private static readonly TimeSpan MinBackoffTime = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan MaxBackoffTime = TimeSpan.FromSeconds(16);

    private readonly byte[] webSocketBuffer = new byte[1024];
    private readonly IOptions<TChannelOptions> options;
    private readonly IClientWebSocket<TradeChatService<TChannelOptions>> clientWebSocket;
    private readonly IHttpClient<TradeChatService<TChannelOptions>> httpClient;
    private readonly ILogger<TradeChatService<TChannelOptions>> logger;

    private TimeSpan backoffTime = MinBackoffTime;

    public TradeChatService(
        IOptions<TChannelOptions> options,
        IClientWebSocket<TradeChatService<TChannelOptions>> clientWebSocket,
        IHttpClient<TradeChatService<TChannelOptions>> client,
        ILogger<TradeChatService<TChannelOptions>> logger)
    {
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
        var scopedLogger = this.logger.CreateScopedLogger();
        return await this.GetTradeMessagesInternal(LatestTradesUri, from, scopedLogger, cancellationToken);
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
            return [];
        }
    }

    private async Task<string> GetAsync(string? uri, HttpRequestMessage? httpRequestMessage, ScopedLogger<TradeChatService<TChannelOptions>> scopedLogger, CancellationToken cancellationToken)
    {
        var response = httpRequestMessage is not null ?
            await this.httpClient.SendAsync(httpRequestMessage, cancellationToken) :
            await this.httpClient.GetAsync(uri!, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError("Error occurred while retrieving trades from {uri}", $"{this.httpClient.BaseAddress}/{uri}");
            throw new InvalidOperationException($"Received [{response.StatusCode}] from [{this.httpClient.BaseAddress}/{uri}]");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return content;
    }

    private async Task<TraderMessageResponse?> ReceiveTraderMessageResponseSafe(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
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
            await Task.Delay(1000, cancellationToken);
            return await this.ReceiveTraderMessageResponseSafe(cancellationToken);
        }
    }

    private async Task<string> ReceiveWebsocketResponseSafe(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        try
        {
            var response = await this.ReceiveWebsocketResponse(cancellationToken);
            this.backoffTime = MinBackoffTime;
            return response;
        }
        catch (OperationCanceledException ex)
        {
            scopedLogger.LogError(ex, "Receive operation timed out");
            return string.Empty;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception while receiving response. Waiting {this.backoffTime.TotalSeconds}s and retrying", this.backoffTime.TotalSeconds);
            await Task.Delay(this.backoffTime, cancellationToken);
            this.backoffTime = TimeSpan.FromSeconds(Math.Min(this.backoffTime.TotalSeconds * 2, MaxBackoffTime.TotalSeconds));
            return await this.ReceiveWebsocketResponseSafe(cancellationToken);
        }
    }

    private async Task<string> ReceiveWebsocketResponse(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
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
                scopedLogger.LogDebug("Received close. Attempting to reconnect");
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
        var parsedStringMessage = await streamReader.ReadToEndAsync(cancellationToken);
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

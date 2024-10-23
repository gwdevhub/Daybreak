using Daybreak.Configuration.Options;
using Daybreak.Converters;
using Daybreak.Models.Trade;
using Daybreak.Services.Notifications;
using Daybreak.Services.TradeChat.Models;
using Daybreak.Services.TradeChat.Notifications;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.TradeChat;
internal sealed class TradeAlertingService : ITradeAlertingService, IApplicationLifetimeService
{
    private static readonly TimeSpan QuoteCheckDelay = TimeSpan.FromMinutes(15);
    private static readonly PriceToStringConverter PriceToStringConverter = new();

    private readonly List<ITradeAlert> tradeAlerts = [];
    private readonly ITraderQuoteService traderQuoteService;
    private readonly INotificationService notificationService;
    private readonly ITradeHistoryDatabase tradeHistoryDatabase;
    private readonly ITradeChatService<KamadanTradeChatOptions> kamadanTradeChatService;
    private readonly ITradeChatService<AscalonTradeChatOptions> ascalonTradeChatService;
    private readonly ILiveUpdateableOptions<TradeAlertingOptions> options;
    private readonly ILogger<TradeAlertingService> logger;
    private readonly CancellationTokenSource cancellationTokenSource = new();

    public IEnumerable<ITradeAlert> TradeAlerts => this.tradeAlerts;

    public TradeAlertingService(
        ITraderQuoteService traderQuoteService,
        INotificationService notificationService,
        ITradeHistoryDatabase tradeHistoryDatabase,
        ITradeChatService<KamadanTradeChatOptions> kamadanTradeChatService,
        ITradeChatService<AscalonTradeChatOptions> ascalonTradeChatService,
        ILiveUpdateableOptions<TradeAlertingOptions> options,
        ILogger<TradeAlertingService> logger)
    {
        this.traderQuoteService = traderQuoteService.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.tradeHistoryDatabase = tradeHistoryDatabase.ThrowIfNull();
        this.kamadanTradeChatService = kamadanTradeChatService.ThrowIfNull();
        this.ascalonTradeChatService = ascalonTradeChatService.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.tradeAlerts = this.options.Value.Alerts ?? [];
    }

    public void AddTradeAlert(ITradeAlert tradeAlert)
    {
        if (this.tradeAlerts.FirstOrDefault(t => t.Id == tradeAlert.Id) is not null)
        {
            throw new InvalidOperationException($"Cannot add {nameof(ITradeAlert)}. Another {nameof(ITradeAlert)} exists with the same id");
        }

        this.tradeAlerts.Add(tradeAlert);
        this.SaveTradeAlerts();
    }

    public void ModifyTradeAlert(ITradeAlert modifiedTradeAlert)
    {
        var existingTradeAlertId = this.tradeAlerts.IndexOfWhere(t => t.Id == modifiedTradeAlert.Id);
        if (existingTradeAlertId is -1)
        {
            throw new InvalidOperationException($"No trade alert found to modify");
        }

        this.tradeAlerts.RemoveAt(existingTradeAlertId);
        this.tradeAlerts.Insert(existingTradeAlertId, modifiedTradeAlert);
        this.SaveTradeAlerts();
    }

    public void DeleteTradeAlert(string tradeAlertId)
    {
        var existingTradeAlert = this.tradeAlerts.FirstOrDefault(t => t.Id == tradeAlertId);
        if (existingTradeAlert is null)
        {
            return;
        }

        this.tradeAlerts.Remove(existingTradeAlert);
        this.SaveTradeAlerts();
    }

    public void OnClosing()
    {
        this.cancellationTokenSource.Cancel();
        this.cancellationTokenSource.Dispose();
    }

    public void OnStartup()
    {
        this.StartAlertingService(this.cancellationTokenSource.Token);
    }

    private async void StartAlertingService(CancellationToken cancellationToken)
    {
        var lastCheckTime = this.options.Value.LastCheckTime;
        var timeSinceLastCheckTime = DateTimeOffset.UtcNow - lastCheckTime;
        if (timeSinceLastCheckTime > this.options.Value.MaxLookbackPeriod)
        {
            timeSinceLastCheckTime = this.options.Value.MaxLookbackPeriod;
        }

        this.options.Value.LastCheckTime = DateTime.UtcNow;
        this.options.UpdateOption();
        var savedTrades = this.tradeHistoryDatabase.GetTraderMessagesSinceTime(DateTimeOffset.UtcNow - timeSinceLastCheckTime);
        if (savedTrades.None())
        {
            var kamadanHistoryTradesTask = GetTraderMessages(this.kamadanTradeChatService, TraderSource.Kamadan, DateTime.UtcNow - timeSinceLastCheckTime, cancellationToken);
            var ascalonHistoryTradesTask = GetTraderMessages(this.ascalonTradeChatService, TraderSource.Kamadan, DateTime.UtcNow - timeSinceLastCheckTime, cancellationToken);
            await Task.WhenAll(kamadanHistoryTradesTask, ascalonHistoryTradesTask);
            var kamadanHistoryTrades = await kamadanHistoryTradesTask;
            var ascalonHistoryTrades = await ascalonHistoryTradesTask;
            savedTrades = kamadanHistoryTrades.Concat(ascalonHistoryTrades).ToList();
            foreach(var trade in savedTrades)
            {
                this.tradeHistoryDatabase.StoreTraderMessage(trade);
            }
        }

        foreach(var trade in savedTrades)
        {
            this.CheckTrade(trade);
        }

        await Task.WhenAll(
            new TaskFactory().StartNew(() => this.CheckTraderQuotes(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap(),
            new TaskFactory().StartNew(() => this.CheckLiveTrades(this.kamadanTradeChatService, TraderSource.Kamadan, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap(),
            new TaskFactory().StartNew(() => this.CheckLiveTrades(this.ascalonTradeChatService, TraderSource.Ascalon, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap());
    }

    private async Task CheckLiveTrades<T>(ITradeChatService<T> tradeChatService, TraderSource traderSource, CancellationToken cancellationToken)
        where T : class, ITradeChatOptions, new()
    {
        await foreach(var message in tradeChatService.GetLiveTraderMessages(cancellationToken))
        {
            var traderMessageDTO = new TraderMessageDTO
            {
                Timestamp = message.Timestamp,
                Id = message.Timestamp.Ticks,
                Message = message.Message,
                Sender = message.Sender,
                TraderSource = (int)traderSource
            };

            this.tradeHistoryDatabase.StoreTraderMessage(traderMessageDTO);
            this.CheckTrade(traderMessageDTO);
        }
    }

    private async Task CheckTraderQuotes(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (this.tradeAlerts.OfType<QuoteAlert>().None(q => q.Enabled))
            {
                await Task.Delay(TimeSpan.FromSeconds(this.options.Value.QuoteAlertsInterval), cancellationToken);
                continue;
            }

            var buyQuotes = await this.traderQuoteService.GetBuyQuotes(cancellationToken);
            var sellQuotes = await this.traderQuoteService.GetSellQuotes(cancellationToken);
            var matches = new List<(TraderQuote Quote, QuoteAlert Alert, int TargetPrice)>();
            foreach (var alert in this.tradeAlerts.OfType<QuoteAlert>())
            {
                var maybeQuote = alert.TraderQuoteType is TraderQuoteType.Buy ?
                    buyQuotes.FirstOrDefault(q => q.Item?.Id == alert.ItemId) :
                    sellQuotes.FirstOrDefault(q => q.Item?.Id == alert.ItemId);
                if (maybeQuote is null)
                {
                    continue;
                }

                if (maybeQuote.Price >= alert.UpperPriceTarget &&
                    alert.UpperPriceTargetEnabled)
                {
                    matches.Add((maybeQuote, alert, alert.UpperPriceTarget));
                }
                else if (maybeQuote.Price <= alert.LowerPriceTarget &&
                    alert.LowerPriceTargetEnabled)
                {
                    matches.Add((maybeQuote, alert, alert.LowerPriceTarget));
                }
            }

            if (matches.Count > 0)
            {
                var description = string.Join('\n', matches.Select(m => $"[{m.Quote.Item?.Name}] Target: {ConvertPriceToString(m.TargetPrice)}, Current: {ConvertPriceToString(m.Quote.Price)}"));
                this.notificationService.NotifyInformation(
                    title: "Quote alert!",
                    description: description,
                    expirationTime: DateTime.Now + TimeSpan.FromSeconds(15));
            }

            await Task.Delay(TimeSpan.FromSeconds(this.options.Value.QuoteAlertsInterval), cancellationToken);
        }
    }

    private void CheckTrade(TraderMessageDTO traderMessageDTO)
    {
        foreach(var alert in this.tradeAlerts.OfType<TradeAlert>())
        {
            if (!alert.Enabled)
            {
                continue;
            }

            var alertMatched = false;
            if (CheckMatch(traderMessageDTO.Sender, alert.SenderCheck!, alert.SenderRegexCheck))
            {
                alertMatched = true;
            }

            if (CheckMatch(traderMessageDTO.Message, alert.MessageCheck!, alert.MessageRegexCheck))
            {
                alertMatched = true;
            }

            if (alertMatched)
            {
                this.NotifyAlertMatch(traderMessageDTO, alert);
            }
        }
    }

    private void SaveTradeAlerts()
    {
        this.options.Value.Alerts = this.tradeAlerts;
        this.options.UpdateOption();
    }

    private void NotifyAlertMatch(TraderMessageDTO traderMessageDTO, ITradeAlert alert)
    {
        var traderMessage = new TraderMessage
        {
            Message = traderMessageDTO.Message,
            Sender = traderMessageDTO.Sender,
            Timestamp = traderMessageDTO.Timestamp.LocalDateTime
        };

        this.notificationService.NotifyInformation<TradeMessageNotificationHandler>(
            title: $"{traderMessageDTO.TraderSource} Trader Alert",
            description: $"{alert.Name} has matched on a trader message. Sender: {traderMessageDTO.Sender}. Message: {traderMessageDTO.Message}",
            metaData: JsonConvert.SerializeObject(traderMessage),
            expirationTime: DateTime.Now + TimeSpan.FromDays(1));
    }

    private static bool CheckMatch(string toCheck, string toMatch, bool isRegex)
    {
        if (toCheck.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (toMatch.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (isRegex)
        {
            var regex = new Regex(toMatch, RegexOptions.IgnoreCase);
            return regex.IsMatch(toCheck);
        }
        else
        {
            return toCheck.ToLower().Contains(toMatch.ToLower());
        }
    }

    private static async Task<IEnumerable<TraderMessageDTO>> GetTraderMessages<T>(ITradeChatService<T> tradeChatService, TraderSource traderSource, DateTime since, CancellationToken cancellationToken)
        where T : class, ITradeChatOptions, new()
    {
        var trades = await tradeChatService.GetLatestTrades(cancellationToken);
        var orderedTrades = trades.OrderBy(t => t.Timestamp).Select(t => new TraderMessageDTO
        {
            Timestamp = t.Timestamp,
            Id = t.Timestamp.Ticks,
            Message = t.Message,
            Sender = t.Sender,
            TraderSource = (int)traderSource
        }).ToList();

        return orderedTrades.Where(t => t.Timestamp > since);
    }

    /// <summary>
    /// Returns all trades since the time interval by querying paginated responses.
    /// Currently is not supported by the API.
    /// </summary>
    private static async Task<IEnumerable<TraderMessageDTO>> GetTraderMessagesSince<T>(ITradeChatService<T> tradeChatService, TraderSource traderSource, DateTime since, CancellationToken cancellationToken)
        where T : class, ITradeChatOptions, new()
    {
        var retrievedTrades = new List<TraderMessageDTO>();
        var retrievedCount = 0;
        do
        {
            var trades = await tradeChatService.GetLatestTrades(cancellationToken, since);
            var orderedTrades = trades.OrderBy(t => t.Timestamp).Select(t => new TraderMessageDTO
            {
                Timestamp = t.Timestamp,
                Id = t.Timestamp.Ticks,
                Message = t.Message,
                Sender = t.Sender,
                TraderSource = (int)traderSource
            }).ToList();

            retrievedTrades.AddRange(orderedTrades);
            retrievedCount = orderedTrades.Count;
            var maybeMaxTime = orderedTrades.LastOrDefault()?.Timestamp;
            if (maybeMaxTime is DateTimeOffset maxTime)
            {
                since = maxTime.LocalDateTime;
            }
        } while (retrievedCount > 0);

        return retrievedTrades;
    }

    private static string ConvertPriceToString(int price)
    {
        return PriceToStringConverter.Convert(price, typeof(string), default!, CultureInfo.CurrentCulture).Cast<string>();
    }
}

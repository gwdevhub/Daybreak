using Daybreak.Configuration.Options;
using Daybreak.Services.TradeChat.Models;
using Daybreak.Services.TradeChat.Notifications;
using Daybreak.Shared.Configuration.Options;
using Daybreak.Shared.Converters;
using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.TradeChat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Text.RegularExpressions;

namespace Daybreak.Services.TradeChat;

internal sealed class TradeAlertingService : ITradeAlertingService, IHostedService
{
    private readonly List<ITradeAlert> tradeAlerts = [];
    private readonly ITraderQuoteService traderQuoteService;
    private readonly IOptionsProvider optionsProvider;
    private readonly INotificationService notificationService;
    private readonly ITradeHistoryDatabase tradeHistoryDatabase;
    private readonly ITradeChatService<KamadanTradeChatOptions> kamadanTradeChatService;
    private readonly ITradeChatService<AscalonTradeChatOptions> ascalonTradeChatService;
    private readonly IOptionsMonitor<TradeAlertingOptions> options;
    private readonly ILogger<TradeAlertingService> logger;

    public IEnumerable<ITradeAlert> TradeAlerts => this.tradeAlerts;

    public TradeAlertingService(
        ITraderQuoteService traderQuoteService,
        IOptionsProvider optionsProvider,
        INotificationService notificationService,
        ITradeHistoryDatabase tradeHistoryDatabase,
        ITradeChatService<KamadanTradeChatOptions> kamadanTradeChatService,
        ITradeChatService<AscalonTradeChatOptions> ascalonTradeChatService,
        IOptionsMonitor<TradeAlertingOptions> options,
        ILogger<TradeAlertingService> logger)
    {
        this.traderQuoteService = traderQuoteService.ThrowIfNull();
        this.optionsProvider = optionsProvider.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.tradeHistoryDatabase = tradeHistoryDatabase.ThrowIfNull();
        this.kamadanTradeChatService = kamadanTradeChatService.ThrowIfNull();
        this.ascalonTradeChatService = ascalonTradeChatService.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.tradeAlerts = this.options.CurrentValue.Alerts ?? [];
    }

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        return this.StartAlertingService(cancellationToken);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
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

    private async Task StartAlertingService(CancellationToken cancellationToken)
    {
        var options = this.options.CurrentValue;
        var lastCheckTime = options.LastCheckTime;
        var timeSinceLastCheckTime = DateTime.UtcNow - lastCheckTime;
        if (timeSinceLastCheckTime > options.MaxLookbackPeriod)
        {
            timeSinceLastCheckTime = options.MaxLookbackPeriod;
        }

        options.LastCheckTime = DateTime.UtcNow;
        this.optionsProvider.SaveOption(options);
        var savedTrades = await this.tradeHistoryDatabase.GetTraderMessagesSinceTime(DateTime.UtcNow - timeSinceLastCheckTime, cancellationToken);
        if (savedTrades.None())
        {
            var kamadanHistoryTradesTask = GetTraderMessages(this.kamadanTradeChatService, TraderSource.Kamadan, DateTime.UtcNow - timeSinceLastCheckTime, cancellationToken);
            var ascalonHistoryTradesTask = GetTraderMessages(this.ascalonTradeChatService, TraderSource.Kamadan, DateTime.UtcNow - timeSinceLastCheckTime, cancellationToken);
            await Task.WhenAll(kamadanHistoryTradesTask, ascalonHistoryTradesTask);
            var kamadanHistoryTrades = await kamadanHistoryTradesTask;
            var ascalonHistoryTrades = await ascalonHistoryTradesTask;
            savedTrades = [.. kamadanHistoryTrades, .. ascalonHistoryTrades];
            foreach (var trade in savedTrades)
            {
                await this.tradeHistoryDatabase.StoreTraderMessage(trade, cancellationToken);
            }
        }

        foreach (var trade in savedTrades)
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
        await foreach (var message in tradeChatService.GetLiveTraderMessages(cancellationToken))
        {
            var traderMessageDTO = new TraderMessageDTO
            {
                Timestamp = message.Timestamp,
                Id = message.Timestamp.Ticks,
                Message = message.Message,
                Sender = message.Sender,
                TraderSource = (int)traderSource
            };

            await this.tradeHistoryDatabase.StoreTraderMessage(traderMessageDTO, cancellationToken);
            this.CheckTrade(traderMessageDTO);
        }
    }

    private async Task CheckTraderQuotes(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (this.tradeAlerts.OfType<QuoteAlert>().None(q => q.Enabled))
            {
                await Task.Delay(TimeSpan.FromSeconds(this.options.CurrentValue.QuoteAlertsInterval), cancellationToken);
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

            await Task.Delay(TimeSpan.FromSeconds(this.options.CurrentValue.QuoteAlertsInterval), cancellationToken);
        }
    }

    private void CheckTrade(TraderMessageDTO traderMessageDTO)
    {
        foreach (var alert in this.tradeAlerts.OfType<TradeAlert>())
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
        var options = this.options.CurrentValue;
        options.Alerts = this.tradeAlerts;
        this.optionsProvider.SaveOption(options);
    }

    private void NotifyAlertMatch(TraderMessageDTO traderMessageDTO, TradeAlert alert)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var source = (TraderSource)traderMessageDTO.TraderSource;
        if (source is not TraderSource.Ascalon and not TraderSource.Kamadan)
        {
            scopedLogger.LogWarning("Unknown trader source {TraderSource} for trader message {TraderMessageId}", traderMessageDTO.TraderSource, traderMessageDTO.Id);
        }

        var traderMessage = new TraderMessage
        {
            Message = traderMessageDTO.Message,
            Sender = traderMessageDTO.Sender,
            Timestamp = traderMessageDTO.Timestamp
        };

        this.notificationService.NotifyInformation<TradeMessageNotificationHandler>(
            title: $"{source} Trader Alert",
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
            return toCheck.Contains(toMatch, StringComparison.CurrentCultureIgnoreCase);
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

    private static string ConvertPriceToString(int price)
    {
        return PriceToStringConverter.Convert(price);
    }
}

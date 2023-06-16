using Daybreak.Configuration.Options;
using Daybreak.Models.Trade;
using Daybreak.Services.Notifications;
using Daybreak.Services.TradeChat.Models;
using Daybreak.Services.TradeChat.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.TradeChat;
public sealed class TradeAlertingService : ITradeAlertingService, IApplicationLifetimeService
{
    private readonly List<TradeAlert> tradeAlerts = new List<TradeAlert>();
    private readonly INotificationService notificationService;
    private readonly ITradeHistoryDatabase tradeHistoryDatabase;
    private readonly ITradeChatService<KamadanTradeChatOptions> kamadanTradeChatService;
    private readonly ITradeChatService<AscalonTradeChatOptions> ascalonTradeChatService;
    private readonly IUpdateableOptions<TradeAlertingOptions> options;
    private readonly ILogger<TradeAlertingService> logger;
    private readonly CancellationTokenSource cancellationTokenSource = new();

    public IEnumerable<TradeAlert> TradeAlerts => this.tradeAlerts;

    public TradeAlertingService(
        INotificationService notificationService,
        ITradeHistoryDatabase tradeHistoryDatabase,
        ITradeChatService<KamadanTradeChatOptions> kamadanTradeChatService,
        ITradeChatService<AscalonTradeChatOptions> ascalonTradeChatService,
        IUpdateableOptions<TradeAlertingOptions> options,
        ILogger<TradeAlertingService> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.tradeHistoryDatabase = tradeHistoryDatabase.ThrowIfNull();
        this.kamadanTradeChatService = kamadanTradeChatService.ThrowIfNull();
        this.ascalonTradeChatService = ascalonTradeChatService.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.tradeAlerts = this.options.Value.Alerts ?? new List<TradeAlert>();
    }

    public void AddTradeAlert(TradeAlert tradeAlert)
    {
        if (this.tradeAlerts.FirstOrDefault(t => t.Id == tradeAlert.Id) is not null)
        {
            throw new InvalidOperationException($"Cannot add {nameof(TradeAlert)}. Another {nameof(TradeAlert)} exists with the same id");
        }

        this.tradeAlerts.Add(tradeAlert);
        this.SaveTradeAlerts();
    }

    public void ModifyTradeAlert(TradeAlert modifiedTradeAlert)
    {
        var existingTradeAlert = this.tradeAlerts.FirstOrDefault(t => t.Id == modifiedTradeAlert.Id);
        if (existingTradeAlert is null)
        {
            throw new InvalidOperationException($"No trade alert found to modify");
        }

        existingTradeAlert.Name = modifiedTradeAlert.Name;
        existingTradeAlert.Enabled = modifiedTradeAlert.Enabled;
        existingTradeAlert.MessageCheck = modifiedTradeAlert.MessageCheck;
        existingTradeAlert.MessageRegexCheck = modifiedTradeAlert.MessageRegexCheck;
        existingTradeAlert.SenderCheck = modifiedTradeAlert.SenderCheck;
        existingTradeAlert.SenderRegexCheck = modifiedTradeAlert.SenderRegexCheck;
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
        var timeSinceLastCheckTime = DateTime.UtcNow - lastCheckTime;
        if (timeSinceLastCheckTime > this.options.Value.MaxLookbackPeriod)
        {
            timeSinceLastCheckTime = this.options.Value.MaxLookbackPeriod;
        }

        this.options.Value.LastCheckTime = DateTime.UtcNow;
        this.options.UpdateOption();
        var savedTrades = this.tradeHistoryDatabase.GetTraderMessagesSinceTime(DateTime.UtcNow - timeSinceLastCheckTime).ToList();
        if (savedTrades.None())
        {
            var kamadanHistoryTradesTask = this.GetTraderMessages(this.kamadanTradeChatService, TraderSource.Kamadan, DateTime.UtcNow - timeSinceLastCheckTime, cancellationToken);
            var ascalonHistoryTradesTask = this.GetTraderMessages(this.ascalonTradeChatService, TraderSource.Kamadan, DateTime.UtcNow - timeSinceLastCheckTime, cancellationToken);
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
            this.CheckLiveTrades(this.kamadanTradeChatService, TraderSource.Kamadan, cancellationToken),
            this.CheckLiveTrades(this.ascalonTradeChatService, TraderSource.Ascalon, cancellationToken));
    }

    private async Task<IEnumerable<TraderMessageDTO>> GetTraderMessages<T>(ITradeChatService<T> tradeChatService, TraderSource traderSource, DateTime since, CancellationToken cancellationToken)
        where T : class, ITradeChatOptions, new()
    {
        var trades = await tradeChatService.GetLatestTrades(cancellationToken);
        var orderedTrades = trades.OrderBy(t => t.Timestamp).Select(t => new TraderMessageDTO
        {
            Timestamp = t.Timestamp,
            Id = t.Timestamp.Ticks,
            Message = t.Message,
            Sender = t.Sender,
            TraderSource = traderSource
        }).ToList();

        return orderedTrades.Where(t => t.Timestamp > since);
    }

    /// <summary>
    /// Returns all trades since the time interval by querying paginated responses.
    /// Currently is not supported by the API.
    /// </summary>
    private async Task<IEnumerable<TraderMessageDTO>> GetTraderMessagesSince<T>(ITradeChatService<T> tradeChatService, TraderSource traderSource, DateTime since, CancellationToken cancellationToken)
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
                TraderSource = traderSource
            }).ToList();

            retrievedTrades.AddRange(orderedTrades);
            retrievedCount = orderedTrades.Count;
            var maybeMaxTime = orderedTrades.LastOrDefault()?.Timestamp;
            if (maybeMaxTime is DateTime maxTime)
            {
                since = maxTime;
            }
        } while (retrievedCount > 0);

        return retrievedTrades;
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
                TraderSource = traderSource
            };

            this.tradeHistoryDatabase.StoreTraderMessage(traderMessageDTO);
            this.CheckTrade(traderMessageDTO);
        }
    }

    private void CheckTrade(TraderMessageDTO traderMessageDTO)
    {
        foreach(var alert in this.tradeAlerts)
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

    private void NotifyAlertMatch(TraderMessageDTO traderMessageDTO, TradeAlert alert)
    {
        var traderMessage = new TraderMessage
        {
            Message = traderMessageDTO.Message,
            Sender = traderMessageDTO.Sender,
            Timestamp = traderMessageDTO.Timestamp
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
            var regex = new Regex(toMatch, RegexOptions.None);
            return regex.IsMatch(toCheck);
        }
        else
        {
            return toCheck.ToLower().Contains(toMatch.ToLower());
        }
    }
}

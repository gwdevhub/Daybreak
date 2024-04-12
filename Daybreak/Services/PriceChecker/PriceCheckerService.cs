using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Notifications;
using Daybreak.Services.PriceChecker.CheckerModules;
using Daybreak.Services.PriceChecker.Models;
using Daybreak.Services.Scanner;
using Daybreak.Services.TradeChat;
using LiteDB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.PriceChecker;
internal sealed class PriceCheckerService : IPriceCheckerService
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly INotificationService notificationService;
    private readonly ITraderQuoteService traderQuoteService;
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly ILiteCollection<PriceCheckDTO> priceCache;
    private readonly ILiveOptions<PriceCheckerOptions> liveOptions;
    private readonly ILogger<PriceCheckerService> logger;
    private readonly List<IIdentifierModule> identifierModules =
    [
        new RuneIdentifierModule()
    ];

    public PriceCheckerService(
        INotificationService notificationService,
        ITraderQuoteService traderQuoteService,
        IGuildwarsMemoryReader guildwarsMemoryReader,
        IGuildwarsMemoryCache guildwarsMemoryCache,
        ILiteCollection<PriceCheckDTO> priceCache,
        ILiveOptions<PriceCheckerOptions> liveOptions,
        ILogger<PriceCheckerService> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.traderQuoteService = traderQuoteService.ThrowIfNull();
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.priceCache = priceCache.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public Task CheckForPrices(CancellationToken cancellationToken)
    {
        return this.PeriodicallyCheckPrices(cancellationToken);
    }

    private async Task PeriodicallyCheckPrices(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger<PriceCheckerService>(nameof(this.PeriodicallyCheckPrices), string.Empty);
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!this.liveOptions.Value.Enabled)
            {
                await Task.Delay(TimeSpan.FromSeconds(this.liveOptions.Value.CheckInterval), cancellationToken);
                continue;
            }

            try
            {
                var inventoryData = await this.guildwarsMemoryCache.ReadInventoryData(cancellationToken);
                if (inventoryData is null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(this.liveOptions.Value.CheckInterval), cancellationToken);
                    continue;
                }

                var loginInfo = await this.guildwarsMemoryCache.ReadLoginData(cancellationToken);
                if (loginInfo is null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(this.liveOptions.Value.CheckInterval), cancellationToken);
                    continue;
                }

                var itemsToCheck =
                    inventoryData.Backpack!.Items.Concat(
                    inventoryData.Bags.SelectMany(b => b!.Items).Concat(
                    inventoryData.BeltPouch!.Items.Concat(
                    inventoryData.EquipmentPack!.Items.Concat(
                    inventoryData.UnclaimedItems!.Items))))
                    .Select(i =>
                    {
                        if (i is BagItem bagItem)
                        {
                            return ($"{loginInfo.PlayerName}-{bagItem.Item.Id}-{string.Join("", i.Modifiers.Select(m => ((uint)m).ToString("X8")))}", (uint)bagItem.Item.Id, i);
                        }
                        else if (i is UnknownBagItem unknownBagItem)
                        {
                            return ($"{loginInfo.PlayerName}-{unknownBagItem.ItemId}-{string.Join("", i.Modifiers.Select(m => ((uint)m).ToString("X8")))}", unknownBagItem.ItemId, i);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Unable to parse bag content of type {i.GetType().Name}");
                        }
                    });


                var buyQuotes = await this.traderQuoteService.GetBuyQuotes(cancellationToken);
                foreach ((var id, var modelId, var item) in itemsToCheck)
                {
                    var identifiedParts = this.identifierModules.SelectMany(module => module.IdentifyItems(item)).ToList();
                    if (identifiedParts.Count == 0)
                    {
                        continue;
                    }

                    var itemsWithPrice = identifiedParts
                        .Select(item => (item, buyQuotes.FirstOrDefault(quote => quote.Item == item)?.Price ?? 0)).ToList();
                    var approxPrice = itemsWithPrice.Sum(t => t.Item2);
                    if (approxPrice < this.liveOptions.Value.MinimumPrice)
                    {
                        continue;
                    }

                    if (this.TryGetPrice(id, out var price) &&
                        price >= approxPrice)
                    {
                        continue;
                    }

                    this.StorePriceCheck(id, approxPrice);
                    var name = await this.guildwarsMemoryReader.GetItemName((int)modelId, item.Modifiers.Select(m => (uint)m).ToList(), cancellationToken);
                    this.notificationService.NotifyInformation(
                        title: $"Expensive item detected",
                        description: $"{(name is null ? string.Empty : $"{name}\n")}{string.Join('\n', itemsWithPrice.Select(t => $"{t.item.Name}: {t.Item2}g"))}",
                        expirationTime: DateTime.MaxValue,
                        persistent: true);
                    await this.guildwarsMemoryReader.SendWhisper($"<c=#f96677>Expensive item detected:\n<c=#ffffff>{(name is null ? string.Empty : $"{name}\n")}{string.Join('\n', itemsWithPrice.Select(t => $"<c=#8fce00>{t.item.Name}: {t.Item2}g<c=#ffffff>"))}", CancellationToken.None);
                }

            }
            catch(Exception e)
            {
                scopedLogger.LogError(e, "Encountered exception while parsing items");
            }

            await Task.Delay(TimeSpan.FromSeconds(this.liveOptions.Value.CheckInterval), cancellationToken);
        }
    }

    private bool TryGetPrice(string id, out double price)
    {
        price = 0;
        var priceCheckDTO = this.priceCache.Find(b => b.Id == id).FirstOrDefault();
        if (priceCheckDTO is null)
        {
            return false;
        }

        if (priceCheckDTO.InsertionTime + TimeSpan.FromHours(this.liveOptions.Value.ItemCacheDuration) < DateTime.UtcNow)
        {
            // Item has expired
            return false;
        }

        price = priceCheckDTO.Price;
        return true;
    }
    
    private void StorePriceCheck(string id, double price)
    {
        this.priceCache.Upsert(new PriceCheckDTO
        {
            Id = id,
            Price = price,
            InsertionTime = DateTime.UtcNow
        });
    }
}

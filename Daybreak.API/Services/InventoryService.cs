using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Guildwars;
using System.Extensions.Core;
using System.Text;

namespace Daybreak.API.Services;

public sealed class InventoryService(
    GameContextService gameContextService,
    GameThreadService gameThreadService,
    UIService uIService,
    ILogger<InventoryService> logger)
{
    private readonly GameContextService gameContextService = gameContextService;
    private readonly GameThreadService gameThreadService = gameThreadService;
    private readonly UIService uIService = uIService;
    private readonly ILogger<InventoryService> logger = logger;

    public async Task<InventoryInformation?> GetInventoryInformation(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var itemTuples = await this.gameThreadService.QueueOnGameThread(() =>
        {
            unsafe
            {
                var gameContext = this.gameContextService.GetGameContext();
                if (gameContext.IsNull)
                {
                    scopedLogger.LogError("Game context is null");
                    return default;
                }

                var inventory = gameContext.Pointer->ItemContext->Inventory;
                if (inventory.IsNull)
                {
                    scopedLogger.LogError("Inventory is null");
                    return default;
                }

                var itemTuples = new List<(BagType Type,List<(uint ModelId, string EncodedCompleteName, string EncodedSingleName, string EncodedName, bool Inscribable, int Quantity, uint[] Modifiers, ItemType ItemType)>)>();
                foreach (var bag in inventory.Pointer->Bags)
                {
                    if (bag is null)
                    {
                        continue;
                    }

                    var retBag = new List<(uint ModelId, string EncodedCompleteName, string EncodedSingleName, string EncodedName, bool Inscribable, int Quantity, uint[] Modifiers, ItemType ItemType)>();
                    itemTuples.Add((bag->Type, retBag));
                    if (bag->ItemsCount is 0)
                    {
                        continue;
                    }

                    foreach(var item in bag->Items)
                    {
                        if (item.IsNull)
                        {
                            continue;
                        }

                        var singleItemName = new string(item.Pointer->SingleItemName);
                        var nameEncoded = new string(item.Pointer->NameEncoded);
                        var completeNameEncoded = new string(item.Pointer->CompleteNameEncoded);
                        var modifiers = new uint[item.Pointer->ModifiersCount];
                        for(var j = 0; j < item.Pointer->ModifiersCount; j++)
                        {
                            modifiers[j] = item.Pointer->Modifiers[j].Mod;
                        }

                        retBag.Add((item.Pointer->ModelId, completeNameEncoded, singleItemName, nameEncoded, item.Pointer->Inscribable, item.Pointer->Quantity, modifiers, item.Pointer->Type));
                    }
                }

                return itemTuples;
            }
        }, cancellationToken);

        if (itemTuples is null)
        {
            return default;
        }

        var decodedItemTuples = await Task.WhenAll(itemTuples.Select(async tuple =>
        {
            var decodedItems = await Task.WhenAll(tuple.Item2.Select(async item =>
            {
                var decodedName = await this.uIService.DecodeString(item.EncodedName, Language.English, cancellationToken);
                var decodedCompleteName = await this.uIService.DecodeString(item.EncodedCompleteName, Language.English, cancellationToken);
                var decodedSingleName = await this.uIService.DecodeString(item.EncodedSingleName, Language.English, cancellationToken);
                return (item.ModelId, item.EncodedName, DecodedName: decodedName, item.EncodedSingleName, DecodedSingleName: decodedSingleName, item.EncodedCompleteName, DecodedCompleteName: decodedCompleteName, item.Inscribable, item.Quantity, item.Modifiers, item.ItemType);
            }));
            return (tuple.Type, Items: decodedItems.ToList());
        }));

        return new InventoryInformation(
            Bags: [.. decodedItemTuples.Select(tuple =>
                new BagEntry(
                    BagType: tuple.Type.ToString(),
                    Items: [.. tuple.Items.Select(item =>
                        new ItemEntry(
                            ItemModelId: item.ModelId,
                            EncodedName: ToBase64(item.EncodedName),
                            DecodedName: item.DecodedName ?? string.Empty,
                            EncodedSingleName: ToBase64(item.EncodedSingleName),
                            DecodedSingleName: item.DecodedSingleName ?? string.Empty,
                            EncodedCompleteName: ToBase64(item.EncodedCompleteName),
                            DecodedCompleteName: item.DecodedCompleteName ?? string.Empty,
                            Inscribable: item.Inscribable,
                            Quantity: item.Quantity,
                            Modifiers: item.Modifiers,
                            Properties: [.. ItemProperty.ParseItemModifiers([.. item.Modifiers.Select(m => (Shared.Models.Guildwars.ItemModifier)m)])],
                            ItemType: item.ItemType.ToString()))]))]);
    }

    private static string ToBase64(string encoded)
    {
        return Convert.ToBase64String(Encoding.Unicode.GetBytes(encoded));
    }
}

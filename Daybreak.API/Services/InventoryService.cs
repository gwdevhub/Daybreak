using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Services.Interop;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Guildwars;
using System.Extensions.Core;
using System.Logging;
using System.Runtime.CompilerServices;
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

                var inventory = gameContext.Pointer->Items->Inventory;
                if (inventory is null)
                {
                    scopedLogger.LogError("Inventory is null");
                    return default;
                }

                var itemTuples = new List<(GWCA.GW.Constants.BagType Type, List<(uint ModelId, string EncodedCompleteName, string EncodedSingleName, string EncodedName, bool Inscribable, int Quantity, uint[] Modifiers, ItemType ItemType, uint Interaction, uint ModelFileId, Byte DyeTint)>)>();
                for (var i = 0; i < 23; i++)
                {
                    // Use GWCA helper to fetch bag pointers by index — safer than reading an inline array directly.
                    var bag = GWCA.GW.Items.GetBagByIndex((uint)i);
                    if (bag is null)
                    {
                        continue;
                    }

                    var retBag = new List<(uint ModelId, string EncodedCompleteName, string EncodedSingleName, string EncodedName, bool Inscribable, int Quantity, uint[] Modifiers, ItemType ItemType, uint Interaction, uint ModelFileId, Byte DyeTint)>();
                    itemTuples.Add((bag->BagType, retBag));
                    if (bag->ItemsCount is 0)
                    {
                        continue;
                    }

                    foreach (var itemPtr in bag->Items.Value)
                    {
                        if (itemPtr is 0)
                        {
                            continue;
                        }

                        var item = (ItemStruct*)itemPtr;
                        var singleItemName = new string((char*)item->SingleItemName);
                        var nameEncoded = new string((char*)item->NameEnc);
                        var completeNameEncoded = new string((char*)item->CompleteNameEnc);
                        var modifiers = new uint[item->ModStructSize];
                        for (var j = 0; j < item->ModStructSize; j++)
                        {
                            modifiers[j] = item->ModStruct[j].Mod;
                        }

                        retBag.Add((item->ModelId, completeNameEncoded, singleItemName, nameEncoded, (item->Interaction & 0x08000000) != 0, item->Quantity, modifiers, (ItemType)item->Type, item->Interaction, item->ModelFileId, item->Dye.DyeTint));
                    }
                }

                return itemTuples;
            }
        }, cancellationToken);

        if (itemTuples is null)
        {
            return default;
        }

        // Decode strings sequentially to avoid race conditions with the game's TextParser language field.
        // Parallel decoding causes crashes because multiple operations race to set/restore the language.
        var decodedItemTuples = new List<(GWCA.GW.Constants.BagType Type, List<(uint ModelId, string EncodedName, string? DecodedName, string EncodedSingleName, string? DecodedSingleName, string EncodedCompleteName, string? DecodedCompleteName, bool Inscribable, int Quantity, uint[] Modifiers, ItemType ItemType, uint Interaction, uint ModelFileId, byte DyeTint)> Items)>();
        foreach (var tuple in itemTuples)
        {
            var decodedItems = new List<(uint ModelId, string EncodedName, string? DecodedName, string EncodedSingleName, string? DecodedSingleName, string EncodedCompleteName, string? DecodedCompleteName, bool Inscribable, int Quantity, uint[] Modifiers, ItemType ItemType, uint Interaction, uint ModelFileId, byte DyeTint)>();
            foreach (var item in tuple.Item2)
            {
                var decodedName = await this.uIService.DecodeString(item.EncodedName, (GWCA.GW.Constants.Language)Language.English, cancellationToken);
                var decodedCompleteName = await this.uIService.DecodeString(item.EncodedCompleteName, (GWCA.GW.Constants.Language)Language.English, cancellationToken);
                var decodedSingleName = await this.uIService.DecodeString(item.EncodedSingleName, (GWCA.GW.Constants.Language)Language.English, cancellationToken);
                decodedItems.Add((item.ModelId, item.EncodedName, decodedName, item.EncodedSingleName, decodedSingleName, item.EncodedCompleteName, decodedCompleteName, item.Inscribable, item.Quantity, item.Modifiers, item.ItemType, item.Interaction, item.ModelFileId, item.DyeTint));
            }

            decodedItemTuples.Add((tuple.Type, decodedItems));
        }

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
                            ItemType: item.ItemType.ToString(),
                            Interaction: item.Interaction,
                            ModelFileId: item.ModelFileId,
                            DyeTint: item.DyeTint))]))]);
    }

    private static string ToBase64(string encoded)
    {
        return Convert.ToBase64String(Encoding.Unicode.GetBytes(encoded));
    }
}

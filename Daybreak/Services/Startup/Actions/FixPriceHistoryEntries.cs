using Daybreak.Configuration.Options;
using Daybreak.Services.Registry;
using LiteDB;
using System.Configuration;
using System.Core.Extensions;
using System.Linq;

namespace Daybreak.Services.Startup.Actions;
internal sealed class FixPriceHistoryEntries : StartupActionBase
{
    private const string FixPriceHistoryKey = nameof(FixPriceHistoryEntries);
    private const string Done = "Done";

    private readonly IRegistryService registryService;
    private readonly IUpdateableOptions<PriceHistoryOptions> priceHistoryOptions;
    private readonly ILiteDatabase database;

    public FixPriceHistoryEntries(
        IRegistryService registryService,
        ILiteDatabase liteDatabase,
        IUpdateableOptions<PriceHistoryOptions> options)
    {
        this.registryService = registryService.ThrowIfNull();
        this.database = liteDatabase.ThrowIfNull();
        this.priceHistoryOptions = options.ThrowIfNull();
    }

    public override void ExecuteOnStartup()
    {
        if (this.registryService.TryGetValue<string>(FixPriceHistoryKey, out var value) &&
            value == Done)
        {
            return;
        }

        var collection = this.database.GetCollection(this.priceHistoryOptions.Value.CollectionName);
        if (collection.Find(b => b["_id"].AsString.Contains("_this")).Any())
        {
            collection.DeleteAll();
        }

        this.registryService.SaveValue(FixPriceHistoryKey, Done);
    }
}

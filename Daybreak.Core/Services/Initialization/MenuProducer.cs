using Daybreak.Shared.Models.Menu;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Initialization;

internal class MenuProducer(ILogger<MenuProducer> logger)
    : IMenuServiceProducer
{
    private readonly ILogger<MenuProducer> logger = logger;

    public readonly Dictionary<string, MenuCategory> categories = [];

    public MenuCategory CreateIfNotExistCategory(string name)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.categories.TryGetValue(name, out var category))
        {
            category = new MenuCategory(name);
            this.categories.Add(name, category);
            scopedLogger.LogDebug("Created menu category {Category.Name}", name);
        }

        return category;
    }
}

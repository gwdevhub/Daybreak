using Microsoft.Extensions.Logging;
using Slim;
using System.Collections.Generic;
using System.Core.Extensions;

namespace Daybreak.Services.Mods;

internal sealed class ModsManager : IModsManager
{
    private readonly IServiceManager serviceManager;
    private readonly ILogger<IModsManager> logger;

    public ModsManager(
        IServiceManager serviceManager,
        ILogger<ModsManager> logger)
    {
        this.serviceManager = serviceManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public IEnumerable<IModService> GetMods()
    {
        return this.serviceManager.GetServicesOfType<IModService>();
    }

    public void RegisterMod<TInterface, TImplementation>(bool singleton = false)
        where TInterface : class, IModService
        where TImplementation : TInterface
    {
        if (singleton)
        {
            this.serviceManager.RegisterSingleton<TInterface, TImplementation>();
        }
        else
        {
            this.serviceManager.RegisterScoped<TInterface, TImplementation>();
        }
        
        this.logger.LogInformation($"Registered mod [{typeof(TImplementation).Name}]");
    }
}

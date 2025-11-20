using Daybreak.Shared.Services.Mods;
using Microsoft.Extensions.Logging;
using Slim;
using System.Core.Extensions;

namespace Daybreak.Services.Mods;

internal sealed class ModsManager(
    IServiceManager serviceManager,
    ILogger<ModsManager> logger) : IModsManager
{
    private readonly IServiceManager serviceManager = serviceManager.ThrowIfNull();
    private readonly ILogger<IModsManager> logger = logger.ThrowIfNull();

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
        
        this.logger.LogDebug("Registered mod [{modType}]", typeof(TImplementation).Name);
    }
}

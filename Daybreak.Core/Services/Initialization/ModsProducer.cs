using Daybreak.Shared.Services.Initialization;
using Daybreak.Shared.Services.Mods;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;

namespace Daybreak.Services.Initialization;

public sealed class ModsProducer(IServiceCollection services, ILogger<ModsProducer> logger)
    : IModsProducer
{
    private readonly ILogger<ModsProducer> logger = logger;
    private readonly IServiceCollection services = services;

    public void RegisterMod<TInterface, TImplementation>(bool singleton)
        where TInterface : class, IModService
        where TImplementation : class, TInterface
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (singleton)
        {
            this.services.AddSingleton<TInterface, TImplementation>();
            this.services.AddSingleton<IModService>(sp => sp.GetRequiredService<TInterface>());
        }
        else
        {
            this.services.AddScoped<TInterface, TImplementation>();
            this.services.AddScoped<IModService>(sp => sp.GetRequiredService<TInterface>());
        }

        scopedLogger.LogDebug("Registered {Mod.Name}", typeof(TImplementation).Name);
    }
}

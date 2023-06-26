﻿using Microsoft.Extensions.Logging;
using Slim;
using System.Collections.Generic;
using System.Core.Extensions;

namespace Daybreak.Services.Mods;

public sealed class ModsManager : IModsManager
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

    public void RegisterMod<TInterface, TImplementation>()
        where TInterface : class, IModService
        where TImplementation : TInterface
    {
        this.serviceManager.RegisterScoped<TInterface, TImplementation>();
        this.logger.LogInformation($"Registered mod [{typeof(TImplementation).Name}]");
    }
}

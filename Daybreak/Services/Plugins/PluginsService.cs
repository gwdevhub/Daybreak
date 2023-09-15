﻿using Daybreak.Configuration.Options;
using Daybreak.Models.Plugins;
using Daybreak.Services.Drawing;
using Daybreak.Services.Mods;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Options;
using Daybreak.Services.Plugins.Resolvers;
using Daybreak.Services.Plugins.Validators;
using Daybreak.Services.Startup;
using Daybreak.Services.Updater.PostUpdate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plumsy;
using Plumsy.Models;
using Slim;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Logging;
using System.Reflection;
using System.Threading;

namespace Daybreak.Services.Plugins;

public sealed class PluginsService : IPluginsService
{
    private const string PluginsDirectory = "Plugins";

    private static readonly object Lock = new();

    private readonly List<AvailablePlugin> loadedPlugins = new();
    private readonly ILiveUpdateableOptions<PluginsServiceOptions> liveUpdateableOptions;
    private readonly ILogger<PluginsService> logger;
    private readonly DaybreakPluginValidator daybreakPluginValidator = new();
    private readonly DaybreakPluginDependencyResolver daybreakPluginDependencyResolver = new();
    private readonly PluginManager pluginManager;

    public PluginsService(
        ILiveUpdateableOptions<PluginsServiceOptions> liveUpdateableOptions,
        ILogger<PluginsService> logger)
    {
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.pluginManager = new PluginManager(PluginsDirectory)
            .WithEnvironmentVersionValidator(this.daybreakPluginValidator)
            .WithMetadataValidator(this.daybreakPluginValidator)
            .WithTypeDefinitionsValidator(this.daybreakPluginValidator)
            .WithDependencyResolver(this.daybreakPluginDependencyResolver)
            .WithForceLoadDependencies(true);
    }

    public IEnumerable<AvailablePlugin> GetCurrentlyLoadedPlugins() => this.loadedPlugins;

    public IEnumerable<AvailablePlugin> GetAvailablePlugins()
    {
        var availablePlugins = this.pluginManager.GetAvailablePlugins();
        var enabledPlugins = this.liveUpdateableOptions.Value.EnabledPlugins;
        return availablePlugins
            .Select(plugin => new AvailablePlugin
            {
                Name = plugin.Name,
                Path = plugin.Path,
                Enabled = enabledPlugins.Any(p => p.Name == plugin.Name)
            });
    }

    public void SaveEnabledPlugins(IEnumerable<AvailablePlugin> availablePlugins)
    {
        this.liveUpdateableOptions.Value.EnabledPlugins = availablePlugins
            .Where(p => p.Enabled)
            .Select(p => new Models.PluginEntry
            {
                Name = p.Name,
                Path = p.Path
            })
            .ToList();
        this.liveUpdateableOptions.UpdateOption();
    }

    public void LoadPlugins(
        IServiceManager serviceManager,
        IOptionsProducer optionsProducer,
        IViewManager viewManager,
        IPostUpdateActionProducer postUpdateActionProducer,
        IStartupActionProducer startupActionProducer,
        IDrawingModuleProducer drawingModuleProducer,
        INotificationHandlerProducer notificationHandlerProducer,
        IModsManager modsManager)
    {
        serviceManager.ThrowIfNull();
        optionsProducer.ThrowIfNull();
        viewManager.ThrowIfNull();
        postUpdateActionProducer.ThrowIfNull();
        startupActionProducer.ThrowIfNull();
        drawingModuleProducer.ThrowIfNull();
        notificationHandlerProducer.ThrowIfNull();
        modsManager.ThrowIfNull();

        while (!Monitor.TryEnter(Lock)) { }

        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.LoadPlugins), string.Empty);
        var pluginsPath = Path.GetFullPath(PluginsDirectory);
        if (!Directory.Exists(pluginsPath))
        {
            scopedLogger.LogInformation("Creating plugins folder");
            Directory.CreateDirectory(pluginsPath);
        }

        scopedLogger.LogInformation("Enumerating available plugins");
        var availablePlugins = this.pluginManager.GetAvailablePlugins().ToList();
        foreach (var availablePlugin in availablePlugins)
        {
            scopedLogger.LogInformation($"[{availablePlugin.Name}] - [{(this.liveUpdateableOptions.Value.EnabledPlugins.Any(p => p.Name == availablePlugin.Name) ? "Enabled" : "Disabled")}] - {availablePlugin.Path}");
        }

        scopedLogger.LogInformation("Loading plugins");
        IEnumerable<PluginLoadOperation> results;
        try
        {
            results = this.pluginManager.LoadPlugins(availablePlugins.Where(plugin => this.liveUpdateableOptions.Value.EnabledPlugins.Any(p => p.Name == plugin.Name)));
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Caught exception while loading plugins");
            throw;
        }

        foreach (var result in results)
        {
            var pluginScopedLogger = this.logger.CreateScopedLogger(nameof(this.LoadPlugins), result.PluginEntry?.Name ?? string.Empty);
            var assembly = ExtractAssembly(result);
            LogLoadOperation(result, pluginScopedLogger);

            if (assembly is null)
            {
                continue;
            }

            var entryPoint = assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(PluginConfigurationBase)));
            if (entryPoint is null)
            {
                pluginScopedLogger.LogError($"Assembly loaded but unable to find entry point. The plugin will not start");
                continue;
            }

            var pluginConfig = Activator.CreateInstance(entryPoint)?.As<PluginConfigurationBase>();
            if (pluginConfig is null)
            {
                pluginScopedLogger.LogError($"Assembly loaded but unable to create entry point. The plugin will not start");
                continue;
            }

            RegisterResolvers(pluginConfig, serviceManager);
            pluginScopedLogger.LogInformation("Registered resolvers");
            RegisterServices(pluginConfig, serviceManager);
            pluginScopedLogger.LogInformation("Registered services");
            RegisterOptions(pluginConfig, optionsProducer);
            pluginScopedLogger.LogInformation("Registered options");
            RegisterViews(pluginConfig, viewManager);
            pluginScopedLogger.LogInformation("Registered views");
            RegisterPostUpdateActions(pluginConfig, postUpdateActionProducer);
            pluginScopedLogger.LogInformation("Registered post-update actions");
            RegisterStartupActions(pluginConfig, startupActionProducer);
            pluginScopedLogger.LogInformation("Registered startup actions");
            RegisterDrawingModules(pluginConfig, drawingModuleProducer);
            pluginScopedLogger.LogInformation("Registered drawing modules");
            RegisterNotificationHandlers(pluginConfig, notificationHandlerProducer);
            pluginScopedLogger.LogInformation("Registered notification handlers");
            RegisterMods(pluginConfig, modsManager);
            pluginScopedLogger.LogInformation("Registered mods");
            this.loadedPlugins.Add(new AvailablePlugin { Name = result.PluginEntry?.Name ?? string.Empty, Path = result.PluginEntry?.Path ?? string.Empty, Enabled = true });
            pluginScopedLogger.LogInformation("Loaded plugin");
        }

        Monitor.Exit(Lock);
    }

    private static void LogLoadOperation(PluginLoadOperation result, ScopedLogger<PluginsService> scopedLogger)
    {
        _ = result switch
        {
            PluginLoadOperation.Success success => scopedLogger.LogInformation($"[{success.PluginEntry.Name}] - SUCCESS"),
            PluginLoadOperation.NullEntry entry => scopedLogger.LogInformation($"[{entry.PluginEntry.Name}] - NULL"),
            PluginLoadOperation.FileNotFound entry => scopedLogger.LogInformation($"[{entry.PluginEntry.Name}] - FILE NOT FOUND"),
            PluginLoadOperation.ExceptionEncountered entry => scopedLogger.LogError(entry.Exception, $"[{entry.PluginEntry.Name}] - EXCEPTION"),
            PluginLoadOperation.UnexpectedErrorOccurred entry => scopedLogger.LogError($"[{entry.PluginEntry.Name}] - UNEXPECTED ERROR"),
            _ => default
        };
    }

    private static Assembly? ExtractAssembly(PluginLoadOperation result) => result switch
    {
        PluginLoadOperation.Success success => success.Plugin.Assembly,
        _ => default
    };

    private static void RegisterResolvers(PluginConfigurationBase pluginConfig, IServiceManager serviceManager) => pluginConfig.RegisterResolvers(serviceManager);

    private static void RegisterServices(PluginConfigurationBase pluginConfig, IServiceManager serviceManager)
    {
        /*
         * This requires a bit of a hacky solution.
         * We create a new ServiceCollection and let the plugin populate it.
         * Then, we iterate over it and we move the registrations into the main service manager
         */

        var serviceCollection = new ServiceCollection();
        pluginConfig.RegisterServices(serviceCollection);
        foreach(var descriptor in serviceCollection)
        {
            switch (descriptor.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    if (descriptor.ImplementationFactory is not null)
                    {
                        serviceManager.RegisterSingleton(descriptor.ServiceType, descriptor.ImplementationType, descriptor.ImplementationFactory);
                    }
                    else if (descriptor.ImplementationInstance is not null)
                    {
                        serviceManager.RegisterSingleton(descriptor.ServiceType, descriptor.ImplementationType, sp => descriptor.ImplementationInstance);
                    }
                    else
                    {
                        serviceManager.RegisterSingleton(descriptor.ServiceType, descriptor.ImplementationType);
                    }

                    break;
                case ServiceLifetime.Transient:
                    if (descriptor.ImplementationFactory is not null)
                    {
                        serviceManager.RegisterTransient(descriptor.ServiceType, descriptor.ImplementationType, descriptor.ImplementationFactory);
                    }
                    else if (descriptor.ImplementationInstance is not null)
                    {
                        serviceManager.RegisterTransient(descriptor.ServiceType, descriptor.ImplementationType, sp => descriptor.ImplementationInstance);
                    }
                    else
                    {
                        serviceManager.RegisterTransient(descriptor.ServiceType, descriptor.ImplementationType);
                    }

                    break;
                case ServiceLifetime.Scoped:
                    if (descriptor.ImplementationFactory is not null)
                    {
                        serviceManager.RegisterScoped(descriptor.ServiceType, descriptor.ImplementationType, descriptor.ImplementationFactory);
                    }
                    else if (descriptor.ImplementationInstance is not null)
                    {
                        serviceManager.RegisterScoped(descriptor.ServiceType, descriptor.ImplementationType, sp => descriptor.ImplementationInstance);
                    }
                    else
                    {
                        serviceManager.RegisterScoped(descriptor.ServiceType, descriptor.ImplementationType);
                    }

                    break;
            }
        }
    }

    private static void RegisterOptions(PluginConfigurationBase pluginConfig, IOptionsProducer optionsProducer) => pluginConfig.RegisterOptions(optionsProducer);

    private static void RegisterViews(PluginConfigurationBase pluginConfig, IViewManager viewManager) => pluginConfig.RegisterViews(viewManager);

    private static void RegisterPostUpdateActions(PluginConfigurationBase pluginConfig, IPostUpdateActionProducer postUpdateActionProducer) => pluginConfig.RegisterPostUpdateActions(postUpdateActionProducer);

    private static void RegisterStartupActions(PluginConfigurationBase pluginConfig, IStartupActionProducer startupActionProducer) => pluginConfig.RegisterStartupActions(startupActionProducer);

    private static void RegisterDrawingModules(PluginConfigurationBase pluginConfig, IDrawingModuleProducer drawingModuleProducer) => pluginConfig.RegisterDrawingModules(drawingModuleProducer);

    private static void RegisterNotificationHandlers(PluginConfigurationBase pluginConfig, INotificationHandlerProducer notificationHandlerProducer) => pluginConfig.RegisterNotificationHandlers(notificationHandlerProducer);

    private static void RegisterMods(PluginConfigurationBase pluginConfig, IModsManager modsManager) => pluginConfig.RegisterMods(modsManager);
}

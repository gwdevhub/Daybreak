using Daybreak.Configuration.Options;
using Daybreak.Models.Plugins;
using Daybreak.Services.ApplicationArguments;
using Daybreak.Services.Browser;
using Daybreak.Services.Mods;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Options;
using Daybreak.Services.Plugins.Resolvers;
using Daybreak.Services.Plugins.Validators;
using Daybreak.Services.Startup;
using Daybreak.Services.Updater.PostUpdate;
using Daybreak.Utils;
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
using System.Threading.Tasks;

namespace Daybreak.Services.Plugins;

internal sealed class PluginsService : IPluginsService
{
    private const string DllExtension = ".dll";
    private const string PluginsDirectorySubPath = "Plugins";
    private static readonly string PluginsDirectory = PathUtils.GetAbsolutePathFromRoot(PluginsDirectorySubPath);

    private readonly SemaphoreSlim pluginsSemaphore = new(1, 1);
    private readonly List<AvailablePlugin> loadedPlugins = [];
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
        INotificationHandlerProducer notificationHandlerProducer,
        IModsManager modsManager,
        IBrowserExtensionsProducer browserExtensionsProducer,
        IArgumentHandlerProducer argumentHandlerProducer)
    {
        serviceManager.ThrowIfNull();
        optionsProducer.ThrowIfNull();
        viewManager.ThrowIfNull();
        postUpdateActionProducer.ThrowIfNull();
        startupActionProducer.ThrowIfNull();
        notificationHandlerProducer.ThrowIfNull();
        modsManager.ThrowIfNull();
        browserExtensionsProducer.ThrowIfNull();
        argumentHandlerProducer.ThrowIfNull();

        this.pluginsSemaphore.Wait();
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.LoadPlugins), string.Empty);
        var pluginsPath = PluginsDirectory;
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
            try
            {
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
                RegisterNotificationHandlers(pluginConfig, notificationHandlerProducer);
                pluginScopedLogger.LogInformation("Registered notification handlers");
                RegisterMods(pluginConfig, modsManager);
                pluginScopedLogger.LogInformation("Registered mods");
                RegisterBrowserExtensions(pluginConfig, browserExtensionsProducer);
                pluginScopedLogger.LogInformation("Registered browser extensions");
                RegisterArgumentHandlers(pluginConfig, argumentHandlerProducer);
                pluginScopedLogger.LogInformation("Registered argument handlers");
                this.loadedPlugins.Add(new AvailablePlugin { Name = result.PluginEntry?.Name ?? string.Empty, Path = result.PluginEntry?.Path ?? string.Empty, Enabled = true });
                pluginScopedLogger.LogInformation("Loaded plugin");
            }
            catch(Exception e)
            {
                pluginScopedLogger.LogError(e, $"Encountered exception while loading plugin");
            }
        }

        this.pluginsSemaphore.Release();
    }

    public async Task<bool> AddPlugin(string pathToPlugin)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.AddPlugin), pathToPlugin ?? string.Empty);
        if (pathToPlugin!.IsNullOrWhiteSpace())
        {
            scopedLogger.LogError("Plugin path is null or empty");
            return false;
        }

        var fullPath = Path.GetFullPath(pathToPlugin!);
        if (!File.Exists(fullPath) ||
            !Path.GetExtension(fullPath).Equals(DllExtension, StringComparison.OrdinalIgnoreCase))
        {
            scopedLogger.LogError("Plugin path is invalid. File must exist and must have .dll extension");
            return false;
        }

        var destinationPath = Path.GetFullPath(Path.Combine(PluginsDirectory, Path.GetFileName(fullPath)));
        if (File.Exists(destinationPath))
        {
            scopedLogger.LogError("Plugin already exists");
            return true;
        }

        using var sourceStream = new FileStream(fullPath, FileMode.Open);
        using var destinationStream = new FileStream(destinationPath, FileMode.CreateNew);
        await sourceStream.CopyToAsync(destinationStream);
        return true;
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

    private static void RegisterNotificationHandlers(PluginConfigurationBase pluginConfig, INotificationHandlerProducer notificationHandlerProducer) => pluginConfig.RegisterNotificationHandlers(notificationHandlerProducer);

    private static void RegisterMods(PluginConfigurationBase pluginConfig, IModsManager modsManager) => pluginConfig.RegisterMods(modsManager);

    private static void RegisterBrowserExtensions(PluginConfigurationBase pluginConfig, IBrowserExtensionsProducer browserExtensionsProducer) => pluginConfig.RegisterBrowserExtensions(browserExtensionsProducer);

    private static void RegisterArgumentHandlers(PluginConfigurationBase pluginConfig, IArgumentHandlerProducer argumentHandlerProducer) => pluginConfig.RegisterLaunchArgumentHandlers(argumentHandlerProducer);
}

using Daybreak.Configuration.Options;
using Daybreak.Services.Plugins.Resolvers;
using Daybreak.Services.Plugins.Validators;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Plugins;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Plumsy;
using Plumsy.Models;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Logging;
using System.Reflection;

namespace Daybreak.Services.Plugins;

//TODO: Fix bootstraping. Plugin loading should happen in two steps, one the initial loading and second the saving of the results after the main app is loaded.
internal sealed class PluginsService : IPluginsService
{
    private const string DllExtension = ".dll";
    private const string PluginsDirectorySubPath = "Plugins";
    private static readonly string PluginsDirectory = PathUtils.GetAbsolutePathFromRoot(PluginsDirectorySubPath);

    private readonly SemaphoreSlim pluginsSemaphore = new(1, 1);
    private readonly List<AvailablePlugin> loadedPlugins = [];
    private readonly IOptionsProvider optionsProvider;
    private readonly IOptionsMonitor<PluginsServiceOptions> options;
    private readonly ILogger<PluginsService> logger;
    private readonly DaybreakPluginValidator daybreakPluginValidator = new();
    private readonly DaybreakPluginDependencyResolver daybreakPluginDependencyResolver = new();
    private readonly PluginManager pluginManager;

    public PluginsService(
        IOptionsProvider optionsProvider,
        IOptionsMonitor<PluginsServiceOptions> options,
        ILogger<PluginsService> logger)
    {
        this.optionsProvider = optionsProvider.ThrowIfNull();
        this.options = options.ThrowIfNull();
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
        var enabledPlugins = this.options.CurrentValue.EnabledPlugins;
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
        var options = this.options.CurrentValue;
        options.EnabledPlugins = [.. availablePlugins
            .Where(p => p.Enabled)
            .Select(p => new Models.PluginEntry
            {
                Name = p.Name,
                Path = p.Path
            })];

        this.optionsProvider.SaveOption(options);
    }

    public void LoadPlugins()
    {
        this.pluginsSemaphore.Wait();
        var scopedLogger = this.logger.CreateScopedLogger();
        var pluginsPath = PluginsDirectory;
        if (!Directory.Exists(pluginsPath))
        {
            scopedLogger.LogDebug("Creating plugins folder");
            Directory.CreateDirectory(pluginsPath);
        }

        scopedLogger.LogDebug("Enumerating available plugins");
        var availablePlugins = this.pluginManager.GetAvailablePlugins().ToList();
        foreach (var availablePlugin in availablePlugins)
        {
            scopedLogger.LogDebug($"[{availablePlugin.Name}] - [{(this.options.CurrentValue.EnabledPlugins.Any(p => p.Name == availablePlugin.Name) ? "Enabled" : "Disabled")}] - {availablePlugin.Path}");
        }

        scopedLogger.LogDebug("Loading plugins");
        IEnumerable<PluginLoadOperation> results = [];
        try
        {
            results = this.pluginManager.LoadPlugins(availablePlugins.Where(plugin => this.options.CurrentValue.EnabledPlugins.Any(p => p.Name == plugin.Name)));
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Caught exception while loading plugins");
            throw;
        }

        foreach (var result in results)
        {
            var pluginScopedLogger = this.logger.CreateScopedLogger(flowIdentifier: result.PluginEntry?.Name ?? string.Empty);
            try
            {
                var assembly = ExtractAssembly(result);
                LogLoadOperation(result, pluginScopedLogger);

                if (assembly is null)
                {
                    // Exclude the plugin from the enabled list if it failed to load
                    this.DisablePlugin(result);
                    continue;
                }

                var entryPoint = assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(PluginConfigurationBase)));
                if (entryPoint is null)
                {
                    pluginScopedLogger.LogError($"Assembly loaded but unable to find entry point. The plugin will not start");
                    this.DisablePlugin(result);
                    continue;
                }

                var pluginConfig = Activator.CreateInstance(entryPoint)?.As<PluginConfigurationBase>();
                if (pluginConfig is null)
                {
                    pluginScopedLogger.LogError($"Assembly loaded but unable to create entry point. The plugin will not start");
                    this.DisablePlugin(result);
                    continue;
                }

                this.loadedPlugins.Add(new AvailablePlugin { Name = result.PluginEntry?.Name ?? string.Empty, Path = result.PluginEntry?.Path ?? string.Empty, Enabled = true, Configuration = pluginConfig });
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
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: pathToPlugin ?? string.Empty);
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

    private void DisablePlugin(PluginLoadOperation result)
    {
        var options = this.options.CurrentValue;
        options.EnabledPlugins = [.. options.EnabledPlugins.Where(p => p.Path != result.PluginEntry?.Path)];
        this.optionsProvider.SaveOption(options);
    }

    private static void LogLoadOperation(PluginLoadOperation result, ScopedLogger<PluginsService> scopedLogger)
    {
        _ = result switch
        {
            PluginLoadOperation.Success success => scopedLogger.LogDebug($"[{success.PluginEntry.Name}] - SUCCESS"),
            PluginLoadOperation.NullEntry entry => scopedLogger.LogError($"[{entry.PluginEntry.Name}] - NULL"),
            PluginLoadOperation.FileNotFound entry => scopedLogger.LogDebug($"[{entry.PluginEntry.Name}] - FILE NOT FOUND"),
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
}

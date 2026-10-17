using System.Extensions.Core;
using Daybreak.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class ModsViewModel(
    INotificationService notificationService,
    IViewManager viewManager,
    IModsManager modsManager,
    ILaunchConfigurationService launchConfigurationService,
    ILogger<ModsViewModel> logger)
    : ViewModelBase<ModsViewModel, ModsView>
{
    private readonly INotificationService notificationService = notificationService;
    private readonly IViewManager viewManager = viewManager;
    private readonly IModsManager modService = modsManager;
    private readonly ILaunchConfigurationService launchConfigurationService = launchConfigurationService;
    private readonly ILogger<ModsViewModel> logger = logger;

    private CancellationTokenSource? cts;

    public LaunchConfigurationWithCredentials? LaunchConfiguration { get; private set; }
    public IReadOnlyList<ModListEntry> Mods { get; private set; } = [];

    public override ValueTask ParametersSet(ModsView view, CancellationToken cancellationToken)
    {
        var globalMods = this.modService.GetMods().OrderBy(m => m.Name)
            .Select(m => new ModListEntry
            {
                Name = m.Name,
                Description = m.Description,
                IsVisible = m.IsVisible,
                ModService = m,
                IsEnabled = m.IsEnabled,
                CanManage = m.CanCustomManage,
                IsInstalled = m.IsInstalled,
                CanUninstall = m.CanUninstall,
                CanDisable = m.CanDisable,
                CanUpdate = false,
                Loading = true,
            }).ToList();

        if (this.launchConfigurationService.GetLaunchConfigurations().FirstOrDefault(l => l.Identifier == view.LaunchConfigurationIdentifier) is LaunchConfigurationWithCredentials config)
        {
            this.LaunchConfiguration = config;
            foreach (var mod in globalMods)
            {
                // Mods that cannot be disabled are always enabled
                mod.IsEnabled = !mod.CanDisable || (config.ModLoadout.TryGetValue(mod.Name, out var entry) && entry.IsEnabled);
            }
        }
        else
        {
            this.LaunchConfiguration = null;
        }

        this.Mods = globalMods;
        this.viewManager.ShowViewRequested += this.ViewManager_ShowViewRequested;
        this.cts?.Cancel();
        this.cts?.Dispose();
        this.cts = new CancellationTokenSource();
        Task.Factory.StartNew(() => this.CheckForUpdates(this.cts.Token), this.cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        return base.ParametersSet(view, cancellationToken);
    }

    public void ToggleMod(ModListEntry mod)
    {
        if (!mod.IsInstalled || !mod.CanDisable)
        {
            return;
        }

        mod.IsEnabled = !mod.IsEnabled;
        if (this.LaunchConfiguration is not null)
        {
            if (!this.LaunchConfiguration.ModLoadout.TryGetValue(mod.Name, out var entry))
            {
                entry = new ModLoadoutEntry();
                this.LaunchConfiguration.ModLoadout.Add(mod.Name, entry);
            }

            entry.IsEnabled = mod.IsEnabled;
            if (!this.launchConfigurationService.SaveConfiguration(this.LaunchConfiguration))
            {
                mod.IsEnabled = !mod.IsEnabled;
                entry.IsEnabled = mod.IsEnabled;
                this.notificationService.NotifyError("Failed to save mod loadout", "The launch configuration could not be saved.");
            }
        }
        else
        {
            mod.ModService.IsEnabled = mod.IsEnabled;
        }

        this.RefreshView();
    }

    public void InstallMod(ModListEntry mod)
    {
        this.viewManager.ShowView<ModInstallationConfirmationView>((nameof(ModInstallationConfirmationView.Name), mod.Name));
    }

    public async Task ManageMod(ModListEntry mod)
    {
        if (!mod.CanManage)
        {
            return;
        }

        await mod.ModService.OnCustomManagement(this.LaunchConfiguration, CancellationToken.None);
    }

    public bool HasCustomSettings(ModListEntry mod) =>
        this.LaunchConfiguration?.ModLoadout.TryGetValue(mod.Name, out var entry) is true && entry.Settings.Count > 0;

    public void ResetSettings(ModListEntry mod)
    {
        if (this.LaunchConfiguration is not { } configuration ||
            !configuration.ModLoadout.TryGetValue(mod.Name, out var entry))
        {
            return;
        }

        var previous = entry.Settings;
        entry.Settings = [];
        if (!this.launchConfigurationService.SaveConfiguration(configuration))
        {
            entry.Settings = previous;
            this.notificationService.NotifyError("Failed to reset mod settings", "The launch configuration could not be saved.");
        }

        this.RefreshView();
    }

    public async Task UpdateMod(ModListEntry mod)
    {
        if (!mod.CanUpdate)
        {
            return;
        }

        mod.Loading = true;
        await this.RefreshViewAsync();
        if (await mod.ModService.PerformUpdate(this.cts?.Token ?? CancellationToken.None))
        {
            mod.CanUpdate = false;
            this.notificationService.NotifyInformation($"{mod.Name} updated", $"{mod.Name} has been updated successfully.");
        }
        else
        {
            mod.CanUpdate = true;
            this.notificationService.NotifyError($"{mod.Name} update failed", $"{mod.Name} has failed to update. Please check logs for more details.");
        }

        mod.Loading = false;
        await this.RefreshViewAsync();
    }

    public async Task UninstallMod(ModListEntry mod)
    {
        if (!mod.CanUninstall)
        {
            return;
        }

        mod.Loading = true;
        await this.RefreshViewAsync();
        if (!await mod.ModService.PerformUninstallation(this.cts?.Token ?? CancellationToken.None))
        {
            this.notificationService.NotifyError($"{mod.Name} uninstallation failed", $"{mod.Name} has failed to uninstall. Please check logs for more details.");

        }

        mod.Loading = false;
        mod.IsInstalled = mod.ModService.IsInstalled;
        await this.RefreshViewAsync();
    }

    private void ViewManager_ShowViewRequested(object? sender, TrailBlazr.Models.ViewRequest e)
    {
        if (e.ViewType == typeof(ModsView))
        {
            return;
        }

        this.viewManager.ShowViewRequested -= this.ViewManager_ShowViewRequested;
        this.cts?.Cancel();
        this.cts?.Dispose();
        this.cts = default;
    }

    private async ValueTask CheckForUpdates(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        foreach (var mod in this.Mods)
        {
            mod.Loading = true;
        }

        await this.RefreshViewAsync();

        // Kinda hacky. Add delay for deduplication when navigating away.
        await Task.Delay(1000, cancellationToken);

        var tasks = this.Mods.Select(async m =>
        {
            var modName = m.Name ?? "Unknown mod";
            try
            {
                if (m.ModService.IsInstalled)
                {
                    scopedLogger.LogDebug("Checking for updates for mod {ModName}", modName);
                    m.CanUpdate = await m.ModService.IsUpdateAvailable(cancellationToken);
                }
                else
                {
                    m.CanUpdate = false;
                }
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "Failed to check for updates for mod {ModName}", modName);
            }

            m.Loading = false;
        });

        await Task.WhenAll(tasks);
        await this.RefreshViewAsync();
    }
}

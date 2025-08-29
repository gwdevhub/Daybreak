using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.Plugins;
using Microsoft.Win32;
using System.Diagnostics;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class PluginsViewModel(
    IApplicationLauncher applicationLauncher,
    IViewManager viewManager,
    IPluginsService pluginsService)
    : ViewModelBase<PluginsViewModel, PluginsView>
{
    private readonly IApplicationLauncher applicationLauncher = applicationLauncher;
    private readonly IViewManager viewManager = viewManager;
    private readonly IPluginsService pluginsService = pluginsService;

    public List<AvailablePlugin> AvailablePlugins { get; init; } = [];
    public bool CanSave { get; private set; } = false;

    public override ValueTask ParametersSet(PluginsView view, CancellationToken cancellationToken)
    {
        this.UpdatePlugins();
        return base.ParametersSet(view, cancellationToken);
    }

    public void ReloadDaybreak()
    {
        var currentlyLoadedPlugins = this.pluginsService.GetCurrentlyLoadedPlugins().ToList();
        var pluginsToBeLoaded = this.AvailablePlugins.Where(p => p.Enabled).ToList();
        this.pluginsService.SaveEnabledPlugins(pluginsToBeLoaded);
        if (pluginsToBeLoaded.Any(p => currentlyLoadedPlugins.None(p2 => p2.Path == p.Path)) ||
            currentlyLoadedPlugins.Any(p => pluginsToBeLoaded.None(p2 => p2.Path == p.Path)))
        {
            this.applicationLauncher.RestartDaybreak();
            return;
        }

        this.viewManager.ShowView<LaunchView>();
    }

    public void ChangePluginState(AvailablePlugin plugin, bool enabled)
    {
        plugin.Enabled = enabled;
        this.CheckChanges();
        this.RefreshView();
    }

    public async void LoadPluginsFromDisk()
    {
        var filePicker = new OpenFileDialog
        {
            Filter = "Dll Files (*.dll)|*.dll",
            Multiselect = true,
            RestoreDirectory = true,
            Title = "Please select dll files"
        };
        if (filePicker.ShowDialog() is false)
        {
            return;
        }

        foreach (var name in filePicker.FileNames)
        {
            await this.pluginsService.AddPlugin(name);
        }

        this.UpdatePlugins();
        this.CheckChanges();
        this.RefreshView();
    }

    public void NavigateToPlugin(AvailablePlugin plugin)
    {
        Process.Start("explorer.exe", plugin.Path);
    }

    private void UpdatePlugins()
    {
        this.AvailablePlugins.ClearAnd().AddRange(this.pluginsService.GetAvailablePlugins());
    }

    private void CheckChanges()
    {
        var currentlyLoadedPlugins = this.pluginsService.GetCurrentlyLoadedPlugins().ToList();
        var pluginsToBeLoaded = this.AvailablePlugins.Where(p => p.Enabled).ToList();
        this.CanSave = pluginsToBeLoaded.Any(p => currentlyLoadedPlugins.None(p2 => p2.Path == p.Path && p.Enabled == p2.Enabled))
            || currentlyLoadedPlugins.Any(p => pluginsToBeLoaded.None(p2 => p2.Path == p.Path && p2.Enabled == p.Enabled));
    }
}

using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Plugins;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for PluginsView.xaml
/// </summary>
public partial class PluginsView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly IPluginsService pluginsService;

    public ObservableCollection<AvailablePlugin> AvailablePlugins { get; private set; } = [];

    public PluginsView(
        IViewManager viewManager,
        IPluginsService pluginsService)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.pluginsService = pluginsService.ThrowIfNull();

        this.InitializeComponent();
    }

    private void View_Loaded(object sender, RoutedEventArgs e)
    {
        this.UpdatePlugins();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        var currentlyLoadedPlugins = this.pluginsService.GetCurrentlyLoadedPlugins().ToList();
        var pluginsToBeLoaded = this.AvailablePlugins.Where(p => p.Enabled).ToList();
        this.pluginsService.SaveEnabledPlugins(pluginsToBeLoaded);
        if (pluginsToBeLoaded.Any(p => currentlyLoadedPlugins.None(p2 => p2.Path == p.Path)) || // If the first list contains elements not present in the second list
            currentlyLoadedPlugins.Any(p => pluginsToBeLoaded.None(p2 => p2.Path == p.Path))) // If the second list contains elements not present in the first list
        {
            this.viewManager.ShowView<PluginsConfirmationView>();
            return;
        }

        this.viewManager.ShowView<LauncherView>();
    }

    private async void LoadPluginFromDisk_Clicked(object sender, EventArgs e)
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
    }

    private void NavigateFileButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not FrameworkElement element ||
            element.DataContext is not AvailablePlugin plugin)
        {
            return;
        }

        Process.Start("explorer.exe", plugin.Path);
    }

    private void UpdatePlugins()
    {
        this.AvailablePlugins.ClearAnd().AddRange(this.pluginsService.GetAvailablePlugins());
    }
}

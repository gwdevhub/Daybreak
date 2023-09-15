using Daybreak.Models.Plugins;
using Daybreak.Services.Navigation;
using Daybreak.Services.Plugins;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
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

    public ObservableCollection<AvailablePlugin> AvailablePlugins { get; private set; } = new();

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
        this.AvailablePlugins.ClearAnd().AddRange(this.pluginsService.GetAvailablePlugins());
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

    private void NavigateFileButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not FrameworkElement element ||
            element.DataContext is not AvailablePlugin plugin)
        {
            return;
        }

        Process.Start("explorer.exe", plugin.Path);
    }
}

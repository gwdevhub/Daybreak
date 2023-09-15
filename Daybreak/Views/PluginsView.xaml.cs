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
        var availablePlugins = this.pluginsService.GetAvailablePlugins().ToList();
        var modifiedPlugins = this.AvailablePlugins.ToList();
        if (availablePlugins.Count != modifiedPlugins.Count ||
            modifiedPlugins.Any(p => availablePlugins.None(p2 => p.Path == p2.Path)) || // The new list contains elements that the old list doesn't have
            availablePlugins.Any(p => modifiedPlugins.None(p2 => p.Path == p2.Path)) || // The old list contains elements that the new list doesn't have
            modifiedPlugins.Any(p => p.Enabled != availablePlugins.FirstOrDefault(p2 => p2.Path == p.Path)?.Enabled)) // The state of the plugins differs
        {
            this.pluginsService.SaveEnabledPlugins(modifiedPlugins);
            this.viewManager.ShowView<PluginsConfirmationView>(modifiedPlugins);
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

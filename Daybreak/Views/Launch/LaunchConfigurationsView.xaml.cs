using Daybreak.Models.LaunchConfigurations;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Services.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Launch;
/// <summary>
/// Interaction logic for LaunchConfigurationsView.xaml
/// </summary>
public partial class LaunchConfigurationsView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly ILaunchConfigurationService launchConfigurationService;

    public ObservableCollection<LaunchConfigurationWithCredentials> LaunchConfigurations { get; set; } = [];

    public LaunchConfigurationsView(
        IViewManager viewManager,
        ILaunchConfigurationService launchConfigurationService)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.launchConfigurationService = launchConfigurationService.ThrowIfNull();
        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.LaunchConfigurations.ClearAnd().AddRange(this.launchConfigurationService.GetLaunchConfigurations());
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        var configs = this.launchConfigurationService.GetLaunchConfigurations().ToList();
        var configsToRemove = configs.Except(this.LaunchConfigurations).ToList();
        foreach(var config in configsToRemove)
        {
            this.launchConfigurationService.DeleteConfiguration(config);
        }

        this.viewManager.ShowView<LauncherView>();
    }

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<LaunchConfigurationView>(this.launchConfigurationService.CreateConfiguration()!);
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not FrameworkElement element ||
            element.DataContext is not LaunchConfigurationWithCredentials config)
        {
            return;
        }

        this.LaunchConfigurations.Remove(config);
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not FrameworkElement element ||
            element.DataContext is not LaunchConfigurationWithCredentials config)
        {
            return;
        }

        this.viewManager.ShowView<LaunchConfigurationView>(config);
    }
}

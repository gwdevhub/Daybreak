using Daybreak.Configuration;
using Daybreak.Launch;
using Daybreak.Services.Navigation;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for MenuList.xaml
/// </summary>
public partial class MenuList : UserControl
{
    private readonly IViewManager viewManager;
    private readonly ILiveOptions<ApplicationConfiguration> liveOptions;

    public MenuList()
        : this(
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveOptions<ApplicationConfiguration>>())
    {
        this.InitializeComponent();
    }

    private MenuList(
        IViewManager viewManager,
        ILiveOptions<ApplicationConfiguration> liveOptions)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
    }

    private void GameCompanionButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }

    private void AccountSettingsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<AccountsView>();
    }

    private void GuildwarsSettingsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<ExecutablesView>();
    }

    private void LauncherSettingsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<SettingsView>();
    }

    private void ExperimentalSettingsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<ExperimentalSettingsView>();
    }

    private void ManageBuildsButton_Clicked(object sender, EventArgs e)
    {
        if (this.liveOptions.Value.ExperimentalFeatures.DownloadIcons)
        {
            this.viewManager.ShowView<IconDownloadView>();
        }
        else
        {
            this.viewManager.ShowView<BuildsListView>();
        }
    }

    private void VersionManagementButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<VersionManagementView>();
    }
    
    private void LogsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<LogsView>();
    }

    private void MetricsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<MetricsView>();
    }

    private void DownloadGuildwarsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<DownloadView>();
    }
}

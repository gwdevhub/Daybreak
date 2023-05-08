using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Services.Navigation;
using Daybreak.Views;
using Daybreak.Views.Onboarding.Toolbox;
using Daybreak.Views.Onboarding.UMod;
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
    private readonly ILiveOptions<LauncherOptions> liveOptions;

    public MenuList()
        : this(
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveOptions<LauncherOptions>>())
    {
        this.InitializeComponent();
    }

    private MenuList(
        IViewManager viewManager,
        ILiveOptions<LauncherOptions> liveOptions)
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

    private void ManageBuildsButton_Clicked(object sender, EventArgs e)
    {
        if (this.liveOptions.Value.DownloadIcons)
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
        this.viewManager.ShowView<GuildwarsDownloadView>();
    }

    private void UModButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<UModOnboardingEntryView>();
    }

    private void ToolboxButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<ToolboxOnboardingEntryView>();
    }
}

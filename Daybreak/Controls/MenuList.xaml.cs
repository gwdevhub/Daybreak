using Daybreak.Configuration;
using Daybreak.Launch;
using Daybreak.Services.Navigation;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Windows.Controls;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for MenuList.xaml
    /// </summary>
    public partial class MenuList : UserControl
    {
        public MenuList()
        {
            this.InitializeComponent();
        }

        private void GameCompanionButton_Clicked(object sender, EventArgs e)
        {
            var viewManager = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>();
            viewManager.ShowView<CompanionView>();
        }

        private void AccountSettingsButton_Clicked(object sender, EventArgs e)
        {
            var viewManager = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>();
            viewManager.ShowView<AccountsView>();
        }

        private void GuildwarsSettingsButton_Clicked(object sender, EventArgs e)
        {
            var viewManager = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>();
            viewManager.ShowView<ExecutablesView>();
        }

        private void LauncherSettingsButton_Clicked(object sender, EventArgs e)
        {
            var viewManager = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>();
            viewManager.ShowView<SettingsView>();
        }

        private void ExperimentalSettingsButton_Clicked(object sender, EventArgs e)
        {
            var viewManager = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>();
            viewManager.ShowView<ExperimentalSettingsView>();
        }

        private void ManageBuildsButton_Clicked(object sender, EventArgs e)
        {
            var viewManager = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>();
            var options = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveOptions<ApplicationConfiguration>>();
            if (options.Value.ExperimentalFeatures.DownloadIcons)
            {
                viewManager.ShowView<IconDownloadView>();
            }
            else
            {
                viewManager.ShowView<BuildsListView>();
            }
        }

        private void VersionManagementButton_Clicked(object sender, EventArgs e)
        {
            var viewManager = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>();
            viewManager.ShowView<VersionManagementView>();
        }

        private void LogsButton_Clicked(object sender, EventArgs e)
        {
            var viewManager = Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>();
            viewManager.ShowView<LogsView>();
        }
    }
}

using Daybreak.Configuration;
using Daybreak.Services.ViewManagement;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for SettingsCategoryView.xaml
    /// </summary>
    public partial class SettingsCategoryView : UserControl
    {
        private readonly IViewManager viewManager;
        private readonly ILiveOptions<ApplicationConfiguration> liveOptions;

        public SettingsCategoryView(
            ILiveOptions<ApplicationConfiguration> liveOptions,
            IViewManager viewManager)
        {
            this.liveOptions = liveOptions.ThrowIfNull();
            this.viewManager = viewManager.ThrowIfNull();
            this.InitializeComponent();
        }

        private void AccountButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<AccountsView>();
        }

        private void LauncherButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<SettingsView>();
        }

        private void ExperimentalButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<ExperimentalSettingsView>();
        }

        private void BuildsButton_Clicked(object sender, System.EventArgs e)
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

        private void VersionButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<VersionManagementView>(this);
        }

        private void LogsButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<LogsView>();
        }

        private void FileButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<ExecutablesView>();
        }

        private void BackButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<MainView>();
        }
    }
}

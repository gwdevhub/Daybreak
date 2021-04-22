using Daybreak.Services.ViewManagement;
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

        public SettingsCategoryView(
            IViewManager viewManager)
        {
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            InitializeComponent();
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
            this.viewManager.ShowView<BuildsListView>();
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

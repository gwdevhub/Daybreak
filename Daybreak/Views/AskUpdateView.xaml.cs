using Daybreak.Services.Privilege;
using Daybreak.Services.Runtime;
using Daybreak.Services.Updater;
using Daybreak.Services.ViewManagement;
using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Linq;
using System.Windows.Controls;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for AskUpdateView.xaml
    /// </summary>
    public partial class AskUpdateView : UserControl
    {
        private const string UpdateDesiredKey = "UpdateDesired";

        private readonly ILogger<AskUpdateView> logger;
        private readonly IViewManager viewManager;
        private readonly IRuntimeStore runtimeStore;
        private readonly IPrivilegeManager privilegeManager;
        private readonly IApplicationUpdater applicationUpdater;

        public AskUpdateView(
            ILogger<AskUpdateView> logger,
            IViewManager viewManager,
            IRuntimeStore runtimeStore,
            IPrivilegeManager privilegeManager,
            IApplicationUpdater applicationUpdater)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.runtimeStore = runtimeStore.ThrowIfNull(nameof(runtimeStore));
            this.privilegeManager = privilegeManager.ThrowIfNull(nameof(privilegeManager));
            this.applicationUpdater = applicationUpdater.ThrowIfNull(nameof(applicationUpdater));
            this.InitializeComponent();
        }

        private bool CheckIfAdmin()
        {
            if (this.privilegeManager.AdminPrivileges is false)
            {
                this.privilegeManager.RequestAdminPrivileges<MainView>("Application needs to be in administrator mode in order to update.");
                return false;
            }

            return true;
        }

        private void NoButton_Clicked(object sender, System.EventArgs e)
        {
            this.logger.LogInformation("User declined update");
            this.runtimeStore.StoreValue(UpdateDesiredKey, false);
            this.viewManager.ShowView<MainView>();
        }

        private async void YesButton_Clicked(object sender, System.EventArgs e)
        {
            this.logger.LogInformation("User accepted update");
            this.runtimeStore.StoreValue(UpdateDesiredKey, true);
            if (this.CheckIfAdmin() is false)
            {
                return;
            }

            var latestVersion = (await this.applicationUpdater.GetVersions()).Last();
            this.viewManager.ShowView<UpdateView>(latestVersion);
        }
    }
}

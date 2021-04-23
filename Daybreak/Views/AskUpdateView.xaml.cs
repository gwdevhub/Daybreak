using Daybreak.Services.Logging;
using Daybreak.Services.Privilege;
using Daybreak.Services.Runtime;
using Daybreak.Services.Updater;
using Daybreak.Services.ViewManagement;
using Daybreak.Utils;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for AskUpdateView.xaml
    /// </summary>
    public partial class AskUpdateView : UserControl
    {
        private const string UpdateDesiredKey = "UpdateDesired";

        private readonly ILogger logger;
        private readonly IViewManager viewManager;
        private readonly IRuntimeStore runtimeStore;
        private readonly IPrivilegeManager privilegeManager;

        public AskUpdateView(
            ILogger logger,
            IViewManager viewManager,
            IRuntimeStore runtimeStore,
            IPrivilegeManager privilegeManager)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.runtimeStore = runtimeStore.ThrowIfNull(nameof(runtimeStore));
            this.privilegeManager = privilegeManager.ThrowIfNull(nameof(privilegeManager));
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

        private void YesButton_Clicked(object sender, System.EventArgs e)
        {
            this.logger.LogInformation("User accepted update");
            this.runtimeStore.StoreValue(UpdateDesiredKey, true);
            if (this.CheckIfAdmin() is false)
            {
                return;
            }

            this.viewManager.ShowView<UpdateView>();
        }
    }
}

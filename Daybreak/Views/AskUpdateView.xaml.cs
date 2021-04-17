using Daybreak.Services.Logging;
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

        public AskUpdateView(
            ILogger logger,
            IViewManager viewManager,
            IRuntimeStore runtimeStore)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.runtimeStore = runtimeStore.ThrowIfNull(nameof(runtimeStore));
            this.InitializeComponent();
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
            this.viewManager.ShowView<UpdateView>();
        }
    }
}

using Daybreak.Services.Logging;
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
        private readonly IApplicationUpdater applicationUpdater;
        private readonly ILogger logger;
        private readonly IViewManager viewManager;

        public AskUpdateView(
            IApplicationUpdater applicationUpdater,
            ILogger logger,
            IViewManager viewManager)
        {
            this.applicationUpdater = applicationUpdater.ThrowIfNull(nameof(applicationUpdater));
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
        }

        private void NoButton_Clicked(object sender, System.EventArgs e)
        {
            this.logger.LogInformation("User declined update");
            this.viewManager.ShowView<MainView>();
        }

        private void YesButton_Clicked(object sender, System.EventArgs e)
        {
            this.logger.LogInformation("User accepted update");
            this.viewManager.ShowView<UpdateView>();
        }
    }
}

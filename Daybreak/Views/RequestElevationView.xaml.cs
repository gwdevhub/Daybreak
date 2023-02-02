using Daybreak.Models;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for RequestElevationView.xaml
    /// </summary>
    public partial class RequestElevationView : UserControl
    {
        private readonly IApplicationLauncher applicationLauncher;
        private readonly IViewManager viewManager;
        private readonly ILogger<RequestElevationView> logger;

        public RequestElevationView(
            IApplicationLauncher applicationLauncher,
            IViewManager viewManager,
            ILogger<RequestElevationView> logger)
        {
            this.applicationLauncher = applicationLauncher.ThrowIfNull(nameof(applicationLauncher));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.InitializeComponent();
        }

        private void YesButton_Clicked(object sender, System.EventArgs e)
        {
            this.applicationLauncher.RestartDaybreakAsAdmin();
        }

        private void NoButton_Clicked(object sender, System.EventArgs e)
        {
            if (this.DataContext is ElevationRequest elevationRequest)
            {
                this.logger.LogWarning($"Elevation request denied. Showing {elevationRequest.View} with {elevationRequest.DataContext}");
                this.viewManager.ShowView(elevationRequest.View!, elevationRequest.DataContext!);
            }
            else
            {
                this.logger.LogWarning("Elevation request context is not set. Returning to home page");
                this.viewManager.ShowView<CompanionView>();
            }
        }
    }
}

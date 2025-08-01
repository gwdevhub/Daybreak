using Daybreak.Shared.Models;
using Daybreak.Shared.Services.ApplicationLauncher;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for RequestDelevationView.xaml
/// </summary>
public partial class RequestDelevationView : UserControl
{
    private readonly IApplicationLauncher applicationLauncher;
    //private readonly IViewManager viewManager;
    private readonly ILogger<RequestDelevationView> logger;

    public RequestDelevationView(
        IApplicationLauncher applicationLauncher,
        //IViewManager viewManager,
        ILogger<RequestDelevationView> logger)
    {
        this.applicationLauncher = applicationLauncher.ThrowIfNull(nameof(applicationLauncher));
        //this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
        this.logger = logger.ThrowIfNull(nameof(logger));
        this.InitializeComponent();
    }

    private void YesButton_Clicked(object sender, System.EventArgs e)
    {
        this.applicationLauncher.RestartDaybreakAsNormalUser();
    }

    private void NoButton_Clicked(object sender, System.EventArgs e)
    {
        if (this.DataContext is ElevationRequest elevationRequest)
        {
            this.logger.LogWarning($"De-elevation request denied. Showing {elevationRequest.View} with {elevationRequest.DataContext}");
            //this.viewManager.ShowView(elevationRequest.View!, elevationRequest.DataContext!);
        }
        else
        {
            this.logger.LogWarning("De-elevation request context is not set. Returning to home page");
            //this.viewManager.ShowView<LauncherView>();
        }
    }
}

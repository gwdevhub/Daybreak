using Daybreak.Services.DSOAL;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.DSOAL;
/// <summary>
/// Interaction logic for DSOALSwitchView.xaml
/// </summary>
public partial class DSOALSwitchView : UserControl
{
    private const string DSOALLemmyUrl = "https://lemmy.wtf/post/27911";
    private readonly IDSOALService dSOALService;
    private readonly IViewManager viewManager;
    private readonly ILogger<DSOALSwitchView> logger;

    [GenerateDependencyProperty]
    private bool dsoalEnabled;

    public DSOALSwitchView(
        IDSOALService dSOALService,
        IViewManager viewManager,
        ILogger<DSOALSwitchView> logger)
    {
        this.dSOALService = dSOALService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
        this.DsoalEnabled = this.dSOALService.IsEnabled;
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        this.dSOALService.IsEnabled = !this.dSOALService.IsEnabled;
        this.viewManager.ShowView<LauncherView>();
    }

    private void Lemmy_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.viewManager.ShowView<DSOALBrowserView>(DSOALLemmyUrl);
    }
}

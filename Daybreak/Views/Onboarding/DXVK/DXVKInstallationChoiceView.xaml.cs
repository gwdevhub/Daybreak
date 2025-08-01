using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.DXVK;
/// <summary>
/// Interaction logic for DXVKInstallationChoiceView.xaml
/// </summary>
public partial class DXVKInstallationChoiceView : UserControl
{
    private readonly ILogger<DXVKInstallationChoiceView> logger;
    //private readonly IViewManager viewManager;

    public DXVKInstallationChoiceView(
        ILogger<DXVKInstallationChoiceView> logger)
        //IViewManager viewManager)
    {
        this.logger = logger.ThrowIfNull();
        //this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void YesButton_Clicked(object sender, System.EventArgs e)
    {
        //this.viewManager.ShowView<DXVKInstallingView>();
    }

    private void NoButton_Clicked(object sender, System.EventArgs e)
    {
        //this.viewManager.ShowView<LauncherView>();
    }
}

using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModInstallationChoiceView.xaml
/// </summary>
public partial class UModInstallationChoiceView : UserControl
{
    //private readonly IViewManager viewManager;
    private readonly ILogger<UModInstallationChoiceView> logger;

    public UModInstallationChoiceView(
        //IViewManager viewManager,
        ILogger<UModInstallationChoiceView> logger)
    {
        //this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        //this.viewManager.ShowView<LauncherView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        //this.viewManager.ShowView<UModInstallingView>();
    }
}

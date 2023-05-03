using Daybreak.Services.Navigation;
using Daybreak.Services.UMod;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModInstallationChoiceView.xaml
/// </summary>
public partial class UModInstallationChoiceView : UserControl
{
    private readonly IUModService uModService;
    private readonly IViewManager viewManager;
    private readonly ILogger<UModInstallationChoiceView> logger;

    public UModInstallationChoiceView(
        IUModService uModService,
        IViewManager viewManager,
        ILogger<UModInstallationChoiceView> logger)
    {
        this.uModService = uModService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<UModInstallingView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        if (this.uModService.LoadUModFromDisk())
        {
            this.viewManager.ShowView<UModSwitchView>();
        }
    }
}

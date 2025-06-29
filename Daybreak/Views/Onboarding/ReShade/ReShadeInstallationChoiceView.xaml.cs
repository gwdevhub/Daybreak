using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.ReShade;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.ReShade;
/// <summary>
/// Interaction logic for UModInstallationChoiceView.xaml
/// </summary>
public partial class ReShadeInstallationChoiceView : UserControl
{
    private readonly IReShadeService reShadeService;
    private readonly IViewManager viewManager;
    private readonly ILogger<ReShadeInstallationChoiceView> logger;

    public ReShadeInstallationChoiceView(
        IReShadeService reShadeService,
        IViewManager viewManager,
        ILogger<ReShadeInstallationChoiceView> logger)
    {
        this.reShadeService = reShadeService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<ReShadeInstallingView>();
    }

    private async void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        if (await this.reShadeService.LoadReShadeFromDisk(CancellationToken.None))
        {
            this.viewManager.ShowView<ReShadeMainView>();
        }
    }
}

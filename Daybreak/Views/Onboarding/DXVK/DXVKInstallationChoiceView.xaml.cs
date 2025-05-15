using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Navigation;
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
    private readonly IViewManager viewManager;

    public DXVKInstallationChoiceView(
        ILogger<DXVKInstallationChoiceView> logger,
        IViewManager viewManager)
    {
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void NvidiaCappedButton_Clicked(object sender, System.EventArgs e)
    {
        this.ProceedWithInstallation(DXVKInstallationChoice.NvidiaCapped);
    }

    private void NvidiaButton_Clicked(object sender, System.EventArgs e)
    {
        this.ProceedWithInstallation(DXVKInstallationChoice.Nvidia);
    }

    private void AMDCappedButton_Clicked(object sender, System.EventArgs e)
    {
        this.ProceedWithInstallation(DXVKInstallationChoice.AMDCapped);
    }

    private void AMDButton_Clicked(object sender, System.EventArgs e)
    {
        this.ProceedWithInstallation(DXVKInstallationChoice.AMD);
    }

    private void ProceedWithInstallation(DXVKInstallationChoice dXVKInstallationChoice)
    {
        this.viewManager.ShowView<DXVKInstallingView>(dXVKInstallationChoice);
    }
}

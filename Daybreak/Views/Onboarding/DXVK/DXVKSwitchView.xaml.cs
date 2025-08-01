using Daybreak.Shared.Services.DXVK;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.DXVK;
/// <summary>
/// Interaction logic for DXVKSwitchView.xaml
/// </summary>
public partial class DXVKSwitchView : UserControl
{
    private readonly IDXVKService dXVKService;
    //private readonly IViewManager viewManager;
    private readonly ILogger<DXVKSwitchView> logger;

    [GenerateDependencyProperty]
    private bool dxvkEnabled;

    public DXVKSwitchView(
        IDXVKService dXVKService,
        //IViewManager viewManager,
        ILogger<DXVKSwitchView> logger)
    {
        this.dXVKService = dXVKService.ThrowIfNull();
        //this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
        this.DxvkEnabled = this.dXVKService.IsEnabled;
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        //this.viewManager.ShowView<LauncherView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        this.dXVKService.IsEnabled = !this.dXVKService.IsEnabled;
        //this.viewManager.ShowView<LauncherView>();
    }
}

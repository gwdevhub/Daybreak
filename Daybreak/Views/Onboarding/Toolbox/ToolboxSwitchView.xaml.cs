using Daybreak.Services.Navigation;
using Daybreak.Services.Toolbox;
using Daybreak.Views.Onboarding.UMod;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.Toolbox;
/// <summary>
/// Interaction logic for UModSwitchView.xaml
/// </summary>
public partial class ToolboxSwitchView : UserControl
{
    private readonly IToolboxService toolboxService;
    private readonly IViewManager viewManager;
    private readonly ILogger<ToolboxSwitchView> logger;

    [GenerateDependencyProperty]
    private bool toolboxEnabled;

    public ToolboxSwitchView(
        IToolboxService toolboxService,
        IViewManager viewManager,
        ILogger<ToolboxSwitchView> logger)
    {
        this.toolboxService = toolboxService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
        this.ToolboxEnabled = this.toolboxService.Enabled;
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        this.toolboxService.Enabled = !this.toolboxService.Enabled;
        this.viewManager.ShowView<LauncherView>();
    }

    private void Wiki_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.viewManager.ShowView<ToolboxHomepageView>();
    }
}

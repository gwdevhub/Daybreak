using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Toolbox;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.Toolbox;
/// <summary>
/// Interaction logic for ToolboxInstallationChoiceView.xaml
/// </summary>
public partial class ToolboxInstallationChoiceView : UserControl
{
    private readonly IToolboxService toolboxService;
    private readonly IViewManager viewManager;
    private readonly ILogger<ToolboxInstallationChoiceView> logger;

    public ToolboxInstallationChoiceView(
        IToolboxService toolboxService,
        IViewManager viewManager,
        ILogger<ToolboxInstallationChoiceView> logger)
    {
        this.toolboxService = toolboxService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<ToolboxInstallationView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        if (this.toolboxService.LoadToolboxFromDisk())
        {
            this.viewManager.ShowView<ToolboxSwitchView>();
        }
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.toolboxService.LoadToolboxFromUsualLocation())
        {
            this.viewManager.ShowView<ToolboxOnboardingEntryView>();
        }
    }
}

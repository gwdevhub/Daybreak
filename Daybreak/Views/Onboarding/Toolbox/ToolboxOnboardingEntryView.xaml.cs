using Daybreak.Services.Navigation;
using Daybreak.Services.Toolbox;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.Toolbox;
/// <summary>
/// Interaction logic for UModOnboardingEntryView.xaml
/// </summary>
public partial class ToolboxOnboardingEntryView : UserControl
{
    private readonly IToolboxService toolboxService;
    private readonly IViewManager viewManager;
    private readonly ILogger<ToolboxOnboardingEntryView> logger;

    public ToolboxOnboardingEntryView(
        IToolboxService toolboxService,
        IViewManager viewManager,
        ILogger<ToolboxOnboardingEntryView> logger)
    {
        this.toolboxService = toolboxService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.toolboxService.IsInstalled)
        {
            this.viewManager.ShowView<ToolboxSwitchView>();
        }
        else
        {
            this.viewManager.ShowView<ToolboxInstallationChoiceView>();
        }
    }
}

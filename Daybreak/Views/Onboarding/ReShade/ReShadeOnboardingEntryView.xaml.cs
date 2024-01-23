using Daybreak.Services.Navigation;
using Daybreak.Services.ReShade;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.ReShade;
/// <summary>
/// Interaction logic for UModOnboardingEntryView.xaml
/// </summary>
public partial class ReShadeOnboardingEntryView : UserControl
{
    private readonly IReShadeService reShadeService;
    private readonly IViewManager viewManager;
    private readonly ILogger<ReShadeOnboardingEntryView> logger;

    public ReShadeOnboardingEntryView(
        IReShadeService reShadeService,
        IViewManager viewManager,
        ILogger<ReShadeOnboardingEntryView> logger)
    {
        this.reShadeService = reShadeService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.reShadeService.IsInstalled)
        {
            this.viewManager.ShowView<ReShadeMainView>();
        }
        else
        {
            this.viewManager.ShowView<ReShadeInstallationChoiceView>();
        }
    }
}

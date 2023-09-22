using Daybreak.Services.Navigation;
using Daybreak.Services.UMod;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModOnboardingEntryView.xaml
/// </summary>
public partial class UModOnboardingEntryView : UserControl
{
    private readonly IUModService uModService;
    private readonly IViewManager viewManager;
    private readonly ILogger<UModOnboardingEntryView> logger;

    public UModOnboardingEntryView(
        IUModService uModService,
        IViewManager viewManager,
        ILogger<UModOnboardingEntryView> logger)
    {
        this.uModService = uModService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.uModService.IsInstalled)
        {
            this.viewManager.ShowView<UModMainView>();
        }
        else
        {
            this.viewManager.ShowView<UModInstallationChoiceView>();
        }
    }
}

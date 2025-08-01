using Daybreak.Shared.Services.DXVK;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.DXVK;
/// <summary>
/// Interaction logic for DXVKOnboardingEntryView.xaml
/// </summary>
public partial class DXVKOnboardingEntryView : UserControl
{
    private readonly IDXVKService dXVKService;
    //private readonly IViewManager viewManager;
    private readonly ILogger<DXVKOnboardingEntryView> logger;

    public DXVKOnboardingEntryView(
        IDXVKService dXVKService,
        //IViewManager viewManager,
        ILogger<DXVKOnboardingEntryView> logger)
    {
        this.dXVKService = dXVKService.ThrowIfNull();
        //this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.dXVKService.IsInstalled)
        {
            //this.viewManager.ShowView<DXVKSwitchView>();
        }
        else
        {
            //this.viewManager.ShowView<DXVKInstallationChoiceView>();
        }
    }
}

using Daybreak.Shared.Services.DSOAL;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.DSOAL;
/// <summary>
/// Interaction logic for DSOALOnboardingEntryView.xaml
/// </summary>
public partial class DSOALOnboardingEntryView : UserControl
{
    private readonly IDSOALService dsoalService;
    //private readonly IViewManager viewManager;
    private readonly ILogger<DSOALOnboardingEntryView> logger;

    public DSOALOnboardingEntryView(
        IDSOALService dsoalService,
        //IViewManager viewManager,
        ILogger<DSOALOnboardingEntryView> logger)
    {
        this.dsoalService = dsoalService.ThrowIfNull();
        //this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.dsoalService.IsInstalled)
        {
            //this.viewManager.ShowView<DSOALSwitchView>();
        }
        else
        {
            //this.viewManager.ShowView<DSOALInstallingView>();
        }
    }
}

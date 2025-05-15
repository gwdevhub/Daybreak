using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.DSOAL;
using Daybreak.Shared.Services.Navigation;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.DSOAL;
/// <summary>
/// Interaction logic for UModInstallerView.xaml
/// </summary>
public partial class DSOALInstallingView : UserControl
{
    private readonly ILogger<DSOALInstallingView> logger;
    private readonly IViewManager viewManager;
    private readonly IDSOALService dSOALService;

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool progressVisible;

    public DSOALInstallingView(
        IDSOALService dSOALService,
        ILogger<DSOALInstallingView> logger,
        IViewManager viewManager)
    {
        this.dSOALService = dSOALService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var installationStatus = sender?.As<DSOALInstallationStatus>();
        var newProgress = (int)(installationStatus?.CurrentStep.As<DownloadStatus.DownloadProgressStep>()?.Progress * 100 ??
            0);

        // Skip Dispatcher invokation if no visible change
        if (this.progressValue == newProgress)
        {
            return;
        }

        this.Dispatcher.Invoke(() =>
        {
            this.ProgressVisible = false;
            if (installationStatus!.CurrentStep is DownloadStatus.DownloadProgressStep downloadUpdateStep)
            {
                this.ProgressValue = newProgress;
                this.ProgressVisible = true;
            }

            this.Description = installationStatus.CurrentStep.Description;
        });
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var installationStatus = new DSOALInstallationStatus();
        installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged;
        this.Description = installationStatus.CurrentStep.Description;
        await this.dSOALService.SetupDSOAL(installationStatus);
        installationStatus.PropertyChanged -= this.DownloadStatus_PropertyChanged;
        this.ContinueButtonEnabled = true;
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<DSOALSwitchView>();
    }
}

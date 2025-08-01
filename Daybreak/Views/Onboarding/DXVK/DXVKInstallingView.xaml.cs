using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.DXVK;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.DXVK;
/// <summary>
/// Interaction logic for DXVKInstallerView.xaml
/// </summary>
public partial class DXVKInstallingView : UserControl
{
    private readonly ILogger<DXVKInstallingView> logger;
    //private readonly IViewManager viewManager;
    private readonly IDXVKService dXVKService;

    private CancellationTokenSource? cancellationTokenSource;

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool progressVisible;

    public DXVKInstallingView(
        IDXVKService dXVKService,
        ILogger<DXVKInstallingView> logger)
        //IViewManager viewManager)
    {
        this.dXVKService = dXVKService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        //this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var installationStatus = sender?.As<DXVKInstallationStatus>();
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
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var installationStatus = new DXVKInstallationStatus();
        installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged;
        this.Description = installationStatus.CurrentStep.Description;
        await this.dXVKService.SetupDXVK(installationStatus, this.cancellationTokenSource.Token);
        installationStatus.PropertyChanged -= this.DownloadStatus_PropertyChanged;
        this.ContinueButtonEnabled = true;
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        //this.viewManager.ShowView<DXVKSwitchView>();
    }
}

using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.ReShade;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.ReShade;
/// <summary>
/// Interaction logic for UModInstallerView.xaml
/// </summary>
public partial class ReShadeInstallingView : UserControl
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly ILogger<ReShadeInstallingView> logger;
    private readonly IViewManager viewManager;
    private readonly IReShadeService reShadeService;

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool progressVisible;

    public ReShadeInstallingView(
        IReShadeService reShadeService,
        ILogger<ReShadeInstallingView> logger,
        IViewManager viewManager)
    {
        this.reShadeService = reShadeService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var installationStatus = sender?.As<ReShadeInstallationStatus>();
        this.Dispatcher.Invoke(() =>
        {
            this.ProgressVisible = false;
            if (installationStatus?.CurrentStep is DownloadStatus.DownloadProgressStep downloadUpdateStep)
            {
                this.ProgressValue = downloadUpdateStep.Progress * 100;
                this.ProgressVisible = true;
            }

            this.Description = installationStatus?.CurrentStep.Description;
        });
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var installationStatus = new ReShadeInstallationStatus();
        installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged;
        await this.reShadeService.SetupReShade(installationStatus, this.cancellationTokenSource.Token);
        installationStatus.PropertyChanged -= this.DownloadStatus_PropertyChanged;
        this.ContinueButtonEnabled = true;
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<ReShadeMainView>();
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
    }
}

using Daybreak.Models.Progress;
using Daybreak.Services.Navigation;
using Daybreak.Services.UMod;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for UModInstallerView.xaml
/// </summary>
public partial class UModInstallerView : UserControl
{
    private readonly ILogger<UModInstallerView> logger;
    private readonly IViewManager viewManager;
    private readonly IUModService uModService;
    private readonly UModInstallationStatus installationStatus = new();

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool progressVisible;

    public UModInstallerView(
        IUModService uModService,
        ILogger<UModInstallerView> logger,
        IViewManager viewManager)
    {
        this.uModService = uModService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged!;
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.Dispatcher.Invoke(() =>
        {
            this.ProgressVisible = false;
            if (this.installationStatus.CurrentStep is DownloadStatus.DownloadProgressStep downloadUpdateStep)
            {
                this.ProgressValue = downloadUpdateStep.Progress * 100;
                this.ProgressVisible = true;
            }

            this.Description = this.installationStatus.CurrentStep.Description;
        });
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await this.uModService.SetupUMod(this.installationStatus);
        this.ContinueButtonEnabled = true;
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }
}

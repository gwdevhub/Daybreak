using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Toolbox;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.Toolbox;
/// <summary>
/// Interaction logic for ToolboxOnboardingView.xaml
/// </summary>
public partial class ToolboxInstallationView : UserControl
{
    private readonly ILogger<ToolboxInstallationView> logger;
    private readonly IViewManager viewManager;
    private readonly IToolboxService toolboxService;

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool progressVisible;

    public ToolboxInstallationView(
        IToolboxService toolboxService,
        ILogger<ToolboxInstallationView> logger,
        IViewManager viewManager)
    {
        this.toolboxService = toolboxService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var installationStatus = sender?.As<ToolboxInstallationStatus>();
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
        var installationStatus = new ToolboxInstallationStatus();
        installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged;
        await this.toolboxService.SetupToolbox(installationStatus);
        installationStatus.PropertyChanged -= this.DownloadStatus_PropertyChanged;
        this.ContinueButtonEnabled = true;
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<ToolboxSwitchView>();
    }
}

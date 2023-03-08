using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System.Windows;
using System;
using System.Windows.Controls;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using System.Core.Extensions;
using System.Windows.Extensions;
using System.Windows.Forms;
using Daybreak.Launch;
using Daybreak.Models;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for DownloadView.xaml
/// </summary>
public partial class DownloadView : System.Windows.Controls.UserControl
{
    private readonly ILogger<DownloadView> logger;
    private readonly IViewManager viewManager;
    private readonly IDownloadService downloadService;
    private readonly DownloadStatus downloadStatus = new();

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty]
    private bool progressVisible;

    public DownloadView(
        IDownloadService downloadService,
        ILogger<DownloadView> logger,
        IViewManager viewManager)
    {
        this.downloadService = downloadService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.downloadStatus.PropertyChanged += this.DownloadStatus_PropertyChanged!;
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.Dispatcher.Invoke(() =>
        {
            this.ProgressVisible = false;
            if (this.downloadStatus.CurrentStep is DownloadStatus.DownloadProgressStep downloadUpdateStep)
            {
                this.ProgressValue = downloadUpdateStep.Progress * 100;
                this.ProgressVisible = true;
            }

            this.Description = this.downloadStatus.CurrentStep.Description;
        });
    }

    private async void DownloadView_Loaded(object sender, RoutedEventArgs e)
    {
        var folderPicker = new FolderBrowserDialog()
        {
            ShowNewFolderButton = true,
            AutoUpgradeEnabled = true,
            Description = "Select where to download the Guildwars installer",
            UseDescriptionForTitle = true
        };

        var result = folderPicker.ShowDialog(new Win32Window(Launcher.Instance.MainWindow));
        if (result is DialogResult.Abort or DialogResult.Cancel or DialogResult.No)
        {
            this.downloadStatus.CurrentStep = DownloadStatus.DownloadCancelled;
            this.ContinueButtonEnabled = true;
            return;
        }

        var folderPath = folderPicker.SelectedPath;
        this.logger.LogInformation("Starting download procedure");
        var success = await this.downloadService.DownloadGuildwars(folderPath, this.downloadStatus).ConfigureAwait(true);
        if (success is false)
        {
            this.logger.LogError("Download procedure failed");
        }
        else
        {
            this.logger.LogInformation("Installed guildwars");
        }

        this.ContinueButtonEnabled = true;
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }
}

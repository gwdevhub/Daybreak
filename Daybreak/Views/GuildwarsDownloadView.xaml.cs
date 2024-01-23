using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System.Windows;
using Daybreak.Models.Progress;
using System.Core.Extensions;
using System.Windows.Extensions;
using System.Windows.Forms;
using Daybreak.Launch;
using Daybreak.Models;
using Daybreak.Services.Guildwars;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for DownloadView.xaml
/// </summary>
public partial class GuildwarsDownloadView : System.Windows.Controls.UserControl
{
    private readonly ILogger<GuildwarsDownloadView> logger;
    private readonly IViewManager viewManager;
    private readonly IGuildwarsInstaller guildwarsInstaller;
    private readonly GuildwarsInstallationStatus installationStatus = new();

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty]
    private bool progressVisible;

    public GuildwarsDownloadView(
        IGuildwarsInstaller guildwarsInstaller,
        ILogger<GuildwarsDownloadView> logger,
        IViewManager viewManager)
    {
        this.guildwarsInstaller = guildwarsInstaller.ThrowIfNull();
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

    private async void DownloadView_Loaded(object sender, RoutedEventArgs e)
    {
        var folderPicker = new FolderBrowserDialog()
        {
            ShowNewFolderButton = true,
            AutoUpgradeEnabled = true,
            Description = "Select where to download the Guildwars installer",
            UseDescriptionForTitle = true
        };

        var result = folderPicker.ShowDialog();
        if (result is DialogResult.Abort or DialogResult.Cancel or DialogResult.No)
        {
            this.installationStatus.CurrentStep = DownloadStatus.DownloadCancelled;
            this.ContinueButtonEnabled = true;
            return;
        }

        var folderPath = folderPicker.SelectedPath;
        this.logger.LogInformation("Starting download procedure");
        var success = await this.guildwarsInstaller.InstallGuildwars(folderPath, this.installationStatus).ConfigureAwait(true);
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

using Daybreak.Models.Progress;
using Daybreak.Services.Guildwars;
using Daybreak.Services.Guildwars.Models;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Daybreak.Views.Installation;
/// <summary>
/// Interaction logic for GuildWarsDownloadSelectionView.xaml
/// </summary>
public partial class GuildWarsDownloadSelectionView : System.Windows.Controls.UserControl
{
    private readonly IViewManager viewManager;
    private readonly IGuildWarsInstaller guildwarsInstaller;
    private readonly ILogger<GuildWarsDownloadSelectionView> logger;

    public GuildWarsDownloadSelectionView(
        IViewManager viewManager,
        IGuildWarsInstaller guildwarsInstaller,
        ILogger<GuildWarsDownloadSelectionView> logger)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.guildwarsInstaller = guildwarsInstaller.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.InitializeComponent();
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
            this.viewManager.ShowView<LauncherView>();
            return;
        }

        var context = new GuildWarsDownloadContext
        {
            CancellationTokenSource = new CancellationTokenSource(),
            GuildwarsInstallationStatus = new GuildwarsInstallationStatus()
        };
        var folderPath = folderPicker.SelectedPath;
        this.logger.LogInformation("Starting download procedure");
        this.viewManager.ShowView<GuildWarsDownloadView>(context);
        var success = await this.guildwarsInstaller.InstallGuildwars(folderPath, context.GuildwarsInstallationStatus, context.CancellationTokenSource.Token);
        if (success is false)
        {
            this.logger.LogError("Download procedure failed");
        }
        else
        {
            this.logger.LogInformation("Installed guildwars");
        }
    }
}

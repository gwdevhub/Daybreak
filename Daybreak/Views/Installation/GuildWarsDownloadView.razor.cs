using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Win32;
using System.IO;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Installation;
public sealed class GuildWarsDownloadViewModel(
    INotificationService notificationService,
    IViewManager viewManager,
    IGuildWarsInstaller guildWarsInstaller)
    : ViewModelBase<GuildWarsDownloadViewModel, GuildWarsDownloadView>
{
    private readonly INotificationService notificationService = notificationService;
    private readonly IViewManager viewManager = viewManager;
    private readonly IGuildWarsInstaller guildWarsInstaller = guildWarsInstaller;

    public string Description { get; private set; } = string.Empty;
    public double Progress { get; private set; }
    public bool ContinueEnabled { get; private set; }

    public override ValueTask Initialize(CancellationToken cancellationToken)
    {
        Task.Factory.StartNew(this.StartDownload);
        return base.Initialize(cancellationToken);
    }

    public void NavigateToExecutables()
    {
        this.viewManager.ShowView<ExecutablesView>();
    }

    private async ValueTask StartDownload()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Select Guild Wars Installation Folder",
            Multiselect = false,
            ValidateNames = true
        };

        if (dialog.ShowDialog() is not true)
        {
            this.notificationService.NotifyInformation(
                title: "Guild Wars Installation Cancelled",
                description: "Installation cancelled by user.");
            this.viewManager.ShowView<LaunchView>();
            return;
        }

        var installationPath = Path.GetFullPath(dialog.FolderName);
        var installationProgress = new Progress<ProgressUpdate>();
        installationProgress.ProgressChanged += this.InstallationStatus_PropertyChanged;
        var result = false;
        try
        {
            result = await this.guildWarsInstaller.InstallGuildwars(installationPath, installationProgress, CancellationToken.None);
        }
        finally
        {
            installationProgress.ProgressChanged -= this.InstallationStatus_PropertyChanged;
        }
        
        if (result)
        {
            this.notificationService.NotifyInformation(
                title: "Guild Wars Installation Completed",
                description: $"Guild Wars has been successfully installed at {installationPath}");
            this.ContinueEnabled = true;
            await this.RefreshViewAsync();
        }
        else
        {
            this.notificationService.NotifyError(
                title: "Guild Wars Installation Failed",
                description: "Guild Wars installation has failed. Please check logs for details.");
            this.viewManager.ShowView<LaunchView>();
        }
    }

    private void InstallationStatus_PropertyChanged(object? _, ProgressUpdate e)
    {
        this.Progress = e.Percentage;
        this.Description = e.StatusMessage ?? string.Empty;
        this.RefreshView();
    }
}

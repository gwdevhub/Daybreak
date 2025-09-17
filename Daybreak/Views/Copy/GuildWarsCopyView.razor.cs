using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Notifications;
using System.IO;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Copy;
public sealed class GuildWarsCopyViewModel(
    INotificationService notificationService,
    IViewManager viewManager,
    IGuildWarsCopyService guildWarsCopyService)
    : ViewModelBase<GuildWarsCopyViewModel, GuildWarsCopyView>
{
    private readonly INotificationService notificationService = notificationService;
    private readonly IViewManager viewManager = viewManager;
    private readonly IGuildWarsCopyService guildWarsCopyService = guildWarsCopyService;

    private string? sourcePath;

    public string Description { get; private set; } = string.Empty;
    public bool ContinueEnabled { get; private set; }
    public double Progress { get; private set; }

    public override ValueTask Initialize(CancellationToken cancellationToken)
    {
        this.sourcePath = this.View?.Source ?? string.Empty;
        Task.Factory.StartNew(this.CopyGuildWars);
        return base.Initialize(cancellationToken);
    }

    public void NavigateToExecutables()
    {
        this.viewManager.ShowView<ExecutablesView>();
    }

    private async ValueTask CopyGuildWars()
    {
        if (!Path.Exists(this.sourcePath))
        {
            this.notificationService.NotifyError(
                "Source Path Invalid",
                "The source path provided does not exist. Please select a valid Guild Wars installation.");
            this.viewManager.ShowView<LaunchView>();
            return;
        }

        var copyStatus = new CopyStatus();
        copyStatus.PropertyChanged += this.CopyStatus_PropertyChanged;

        try
        {
            await this.guildWarsCopyService.CopyGuildwars(this.sourcePath, copyStatus, CancellationToken.None);
        }
        finally
        {
            copyStatus.PropertyChanged -= this.CopyStatus_PropertyChanged;
        }

        this.ContinueEnabled = true;
        this.RefreshView();
    }

    private void CopyStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not CopyStatus copyStatus)
        {
            return;
        }

        this.Description = copyStatus.CurrentStep.Description;
        this.Progress = copyStatus.CurrentStep.Progress;
        this.RefreshView();
    }
}

using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Notifications;
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

        this.ContinueEnabled = false;
        this.RefreshView();
        var operation = this.guildWarsCopyService.CopyGuildwars(this.sourcePath, CancellationToken.None);
        operation.ProgressChanged += this.CopyStatus_PropertyChanged;
        try
        {
            await operation;
        }
        finally
        {
            operation.ProgressChanged -= this.CopyStatus_PropertyChanged;
        }

        this.Description = operation.CurrentProgress?.StatusMessage ?? string.Empty;
        this.Progress = operation.CurrentProgress?.Percentage ?? 1;
        this.ContinueEnabled = true;
        this.RefreshView();
    }

    private void CopyStatus_PropertyChanged(object? _, ProgressUpdate e)
    {
        this.Description = e.StatusMessage ?? string.Empty;
        this.Progress = e.Percentage;
        this.RefreshView();
    }
}

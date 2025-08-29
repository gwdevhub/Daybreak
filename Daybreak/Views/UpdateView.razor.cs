using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Updater;
using System.Windows;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;
using Version = Daybreak.Shared.Models.Versioning.Version;

namespace Daybreak.Views;
public sealed class UpdateViewModel(
    IViewManager viewManager,
    IApplicationUpdater applicationUpdater)
    : ViewModelBase<UpdateViewModel, UpdateView>
{
    private readonly IViewManager viewManager = viewManager;
    private readonly IApplicationUpdater applicationUpdater = applicationUpdater;

    public UpdateStatus? Status { get; set; }
    public string? Description { get; set; }
    public double Progress { get; set; }
    public bool ContinueEnabled { get; set; }

    public override ValueTask ParametersSet(UpdateView view, CancellationToken cancellationToken)
    {
        if (!Version.TryParse(view.Version, out var version))
        {
            this.viewManager.ShowView<LaunchView>();
            return base.ParametersSet(view, cancellationToken);
        }

        this.ContinueEnabled = false;
        this.Status = new UpdateStatus();
        this.Status.PropertyChanged += (_, _) =>
        {
            this.Description = this.Status.CurrentStep.Description;
            this.Progress = this.Status.CurrentStep.Progress;
            this.ContinueEnabled = this.Status.CurrentStep == UpdateStatus.PendingRestart;
            this.RefreshView();
        };

        Task.Factory.StartNew(() => this.applicationUpdater.DownloadUpdate(version, this.Status), TaskCreationOptions.LongRunning);
        return base.ParametersSet(view, cancellationToken);
    }

    public void Continue()
    {
        this.applicationUpdater.FinalizeUpdate();
        Application.Current.Shutdown();
    }
}

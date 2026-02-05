using Daybreak.Shared.Services.Updater;
using Photino.NET;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class UpdateViewModel(
    PhotinoWindow window,
    IViewManager viewManager,
    IApplicationUpdater applicationUpdater)
    : ViewModelBase<UpdateViewModel, UpdateView>
{
    private readonly PhotinoWindow window = window;
    private readonly IViewManager viewManager = viewManager;
    private readonly IApplicationUpdater applicationUpdater = applicationUpdater;

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

        Task.Factory.StartNew(() => this.PerformUpdate(version), CancellationToken.None,  TaskCreationOptions.LongRunning, TaskScheduler.Current);
        return base.ParametersSet(view, cancellationToken);
    }

    public void Continue()
    {
        this.applicationUpdater.FinalizeUpdate();
        this.window.Close();
    }

    private async ValueTask PerformUpdate(Version version)
    {
        this.ContinueEnabled = false;
        var updateOperation = this.applicationUpdater.DownloadUpdate(version, CancellationToken.None);
        updateOperation.ProgressChanged += (_, up) =>
        {
            this.Description = up.StatusMessage;
            this.Progress = up.Percentage;
            this.RefreshView();
        };

        await updateOperation;
        this.ContinueEnabled = true;
        this.RefreshView();
    }
}

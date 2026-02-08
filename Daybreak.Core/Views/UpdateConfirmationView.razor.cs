using Daybreak.Shared.Services.Updater;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class UpdateConfirmationViewModel(
    IViewManager viewManager,
    IApplicationUpdater applicationUpdater)
    : ViewModelBase<UpdateConfirmationViewModel, UpdateConfirmationView>
{
    private readonly IViewManager viewManager = viewManager;
    private readonly IApplicationUpdater applicationUpdater = applicationUpdater;

    public Version? Version { get; private set; }
    public string? ChangeLog { get; private set; }

    public override async ValueTask ParametersSet(UpdateConfirmationView view, CancellationToken cancellationToken)
    {
        var versionString = view.Version;
        if (!Version.TryParse(versionString, out var parsedVersion))
        {
            this.viewManager.ShowView<LaunchView>();
            return;
        }

        this.Version = parsedVersion;
        _ = Task.Factory.StartNew(async () =>
        {
            this.ChangeLog = await this.applicationUpdater.GetChangelog(this.Version, cancellationToken);
            await this.RefreshViewAsync();
        }, CancellationToken.None);
    }

    public void Confirm()
    {
        this.viewManager.ShowView<UpdateView>((nameof(UpdateView.Version), this.Version?.ToString() ?? string.Empty));
    }

    public void Cancel()
    {
        this.viewManager.ShowView<LaunchView>();
    }
}

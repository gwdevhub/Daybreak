using Daybreak.Shared.Services.Updater;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;
using Version = Daybreak.Shared.Models.Versioning.Version;

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
        this.ChangeLog = await this.applicationUpdater.GetChangelog(this.Version);
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

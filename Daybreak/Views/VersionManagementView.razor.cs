using Daybreak.Shared.Services.Updater;
using System.Diagnostics;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class VersionManagementViewModel(
    IViewManager viewManager,
    IApplicationUpdater applicationUpdater)
    : ViewModelBase<VersionManagementViewModel, VersionManagementView>
{
    private const string VersionPlaceholder = "{VERSION}";
    private const string ReleaseURL = $"https://github.com/AlexMacocian/Daybreak/releases/tag/{VersionPlaceholder}";
    private readonly IViewManager viewManager = viewManager;
    private readonly IApplicationUpdater applicationUpdater = applicationUpdater;

    public List<Version> Versions { get; init; } = [];
    public Version? CurrentVersion { get; set; }

    public override async ValueTask ParametersSet(VersionManagementView view, CancellationToken cancellationToken)
    {
        this.Versions.ClearAnd().AddRange((await this.applicationUpdater.GetVersions(cancellationToken)).OrderDescending());
        this.CurrentVersion = this.applicationUpdater.CurrentVersion;
    }

    public void DownloadVersion(Version version)
    {
        this.viewManager.ShowView<UpdateConfirmationView>((nameof(UpdateConfirmationView.Version), version.ToString()));
    }

    public void OpenVersionPage(Version version)
    {
        Process.Start("explorer.exe", ReleaseURL.Replace(VersionPlaceholder, version.ToString()));
    }
}

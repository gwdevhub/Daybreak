using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Notifications;
using System.Core.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class BuildRoutingViewModel(
    INotificationService notificationService,
    IBuildTemplateManager buildTemplateManager,
    IViewManager viewManager)
    : ViewModelBase<BuildRoutingViewModel, BuildRoutingView>
{
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public override async ValueTask ParametersSet(BuildRoutingView view, CancellationToken cancellationToken)
    {
        var build = await this.buildTemplateManager.GetBuild(Uri.UnescapeDataString(view.BuildName));
        if (build is null)
        {
            this.notificationService.NotifyError(
                    title: "Failed to load build",
                    description: $"Encountered an error while loading build {view.BuildName}. Check logs for details");
            this.viewManager.ShowView<LaunchView>();
            return;
        }

        if (build is SingleBuildEntry singleBuildEntry)
        {
            this.viewManager.ShowView<SingleBuildTemplateView>((nameof(SingleBuildTemplateView.BuildName), Uri.EscapeDataString(singleBuildEntry.Name ?? string.Empty)));
        }
        else if (build is TeamBuildEntry teamBuildEntry)
        {
            this.viewManager.ShowView<TeamBuildTemplateView>((nameof(TeamBuildTemplateView.BuildName), Uri.EscapeDataString(teamBuildEntry.Name ?? string.Empty)));
        }
        else
        {
            throw new Exception($"Unexpected build entry type: {build.GetType().Name}");
        }
    }
}

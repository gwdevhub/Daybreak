using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using TrailBlazr.Services;

namespace Daybreak.Views;
public sealed class SingleBuildTemplateViewModel(
    IViewManager viewManager,
    IBuildTemplateManager buildTemplateManager)
    : BuildTemplateViewModelBase<SingleBuildTemplateViewModel, SingleBuildTemplateView>(buildTemplateManager, viewManager)
{
    protected override void LoadBuild(IBuildEntry? buildEntry)
    {
        if (buildEntry is not SingleBuildEntry singleBuildEntry)
        {
            throw new InvalidOperationException($"Expected build entry to be of type {nameof(SingleBuildEntry)} but got {buildEntry?.GetType().Name}");
        }

        this.BuildEntry = singleBuildEntry;
    }

    protected override IBuildEntry SaveBuild()
    {
        return this.BuildEntry ?? throw new InvalidOperationException("Single build is null");
    }

    protected override void ChangeBuildName(string buildName)
    {
        if (this.BuildEntry is null)
        {
            return;
        }

        this.BuildEntry.Name = buildName;
    }
}

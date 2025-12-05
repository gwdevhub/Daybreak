using Daybreak.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Toolbox;
using Daybreak.Shared.Utils;
using System.Core.Extensions;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class BuildListViewModel(
    IViewManager viewManager,
    IBuildTemplateManager buildTemplateManager,
    IToolboxService toolboxService)
    : ViewModelBase<BuildListViewModel, BuildListView>
{
    private static readonly TimeSpan SearchDebounce = TimeSpan.FromSeconds(1);

    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager.ThrowIfNull();
    private readonly IToolboxService toolboxService = toolboxService.ThrowIfNull();

    private readonly List<BuildListEntry> buildEntryCache = [];

    public List<BuildListEntry> BuildEntries { get; } = [];

    public bool IsLoading
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.IsLoading));
        }
    }

    public string SearchTerm
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.SearchTerm));
        }
    } = string.Empty;

    public bool PendingSearch
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.PendingSearch));
        }
    }

    public override async ValueTask ParametersSet(BuildListView view, CancellationToken cancellationToken)
    {
        this.IsLoading = true;
        await this.RefreshViewAsync();

        // Queue loading asynchronously to avoid blocking the UI thread
        _ = Task.Factory.StartNew(async () =>
        {
            var buildEntries = await this.buildTemplateManager.GetBuilds().Select(ConvertToBuildListEntry).ToListAsync(cancellationToken);
            var toolboxBuildEntries = await this.toolboxService.GetToolboxBuilds(cancellationToken).Select(ConvertToBuildListEntry).ToListAsync(cancellationToken);
            this.buildEntryCache.ClearAnd().AddRange(buildEntries)
                .AddRange(toolboxBuildEntries);
            this.BuildEntries.ClearAnd()
                .AddRange(this.buildEntryCache);

            this.IsLoading = false;
            await this.RefreshViewAsync();
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public async Task Search()
    {
        await this.SearchByTerm(this.SearchTerm,  CancellationToken.None);
    }

    public void SearchTermChanged(string searchTerm)
    {
        Task.Run(() => this.SearchByTerm(searchTerm, CancellationToken.None));
    }

    public void BuildClicked(BuildListEntry buildListEntry)
    {
        this.viewManager.ShowView<BuildRoutingView>((nameof(BuildRoutingView.BuildName), buildListEntry.BuildEntry.Name ?? string.Empty));
    }

    public void DeleteBuild(BuildListEntry buildListEntry)
    {
        this.buildEntryCache.Remove(buildListEntry);
        this.BuildEntries.Remove(buildListEntry);

        this.buildTemplateManager.RemoveBuild(buildListEntry.BuildEntry);
        this.RefreshView();
    }

    public void CreateNewSingleBuild()
    {
        var build = this.buildTemplateManager.CreateSingleBuild();
        this.buildTemplateManager.SaveBuild(build);
        this.viewManager.ShowView<BuildRoutingView>((nameof(BuildRoutingView.BuildName), build.Name ?? string.Empty));
    }

    public void CreateNewTeamBuild()
    {
        var build = this.buildTemplateManager.CreateTeamBuild();
        this.buildTemplateManager.SaveBuild(build);
        this.viewManager.ShowView<BuildRoutingView>((nameof(BuildRoutingView.BuildName), build.Name ?? string.Empty));
    }

    private async ValueTask SearchByTerm(string term, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            this.BuildEntries.ClearAnd().AddRange(this.buildEntryCache);
            this.RefreshView();
            return;
        }

        this.IsLoading = true;
        await this.RefreshViewAsync();
        await Task.Factory.StartNew(() =>
        {
            var selectedEntries = this.buildEntryCache
                .Where(b => 
                    StringUtils.MatchesSearchString(b.BuildEntry.Name ?? string.Empty, term) ||
                    StringUtils.MatchesSearchString(b.PrimaryProfession?.Name ?? string.Empty, term));
            this.BuildEntries.ClearAnd()
                .AddRange(selectedEntries);
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);

        this.IsLoading = false;
        await this.RefreshViewAsync();
    }

    private static BuildListEntry ConvertToBuildListEntry(IBuildEntry buildEntry)
    {
        if (buildEntry is SingleBuildEntry singleBuildEntry)
        {
            return new BuildListEntry
            {
                BuildEntry = singleBuildEntry,
                PrimaryProfession = singleBuildEntry.Primary
            };
        }
        else if (buildEntry is TeamBuildEntry teamBuildEntry)
        {
            if (teamBuildEntry.PartyComposition?.FirstOrDefault(c => c.Type is PartyCompositionMemberType.MainPlayer) is PartyCompositionMetadataEntry compositionEntry &&
                teamBuildEntry.Builds.Skip(compositionEntry.Index).FirstOrDefault() is SingleBuildEntry singleBuildEntry2)
            {
                return new BuildListEntry
                { 
                    BuildEntry = teamBuildEntry,
                    PrimaryProfession = singleBuildEntry2.Primary
                };
            }
            else
            {
                return new BuildListEntry
                {
                    BuildEntry = teamBuildEntry,
                    PrimaryProfession = default
                };
            }
        }

        throw new Exception($"Unexpected build entry type: {buildEntry.GetType().Name}");
    }
}

using Daybreak.Models;
using Daybreak.Shared.Extensions;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Toolbox;
using Daybreak.Shared.Utils;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Drawing;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class BuildListViewModel
    : ViewModelBase<BuildListViewModel, BuildListView>
{
    private readonly DotNetObjectReference<BuildListViewModel> dotNetObjectReference;
    private readonly IViewManager viewManager;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IJSRuntime jsRuntime;
    private readonly IToolboxService toolboxService;

    private readonly List<BuildListEntry> buildEntryCache = [];

    public BuildListViewModel(
        IViewManager viewManager,
        IBuildTemplateManager buildTemplateManager,
        IJSRuntime jsRuntime,
        IToolboxService toolboxService)
    {
        this.dotNetObjectReference = DotNetObjectReference.Create(this);
        this.viewManager = viewManager;
        this.buildTemplateManager = buildTemplateManager;
        this.jsRuntime = jsRuntime;
        this.toolboxService = toolboxService;
    }

    public List<BuildListEntry> BuildEntries { get; } = [];

    public BuildListEntry? HoveredEntry { get; private set; }

    public Point? SnippetPosition { get; private set; }

    public bool ShowSnippet { get; private set; }

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
        this.CloseSnippet();
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

    public async void OpenSnippet(BuildListEntry buildListEntry, MouseEventArgs e)
    {
        this.ShowSnippet = false;
        this.HoveredEntry = buildListEntry;
        this.SnippetPosition = new Point((int)e.ClientX, (int)e.ClientY);
        await this.jsRuntime.HoverDelayStart(this.dotNetObjectReference, nameof(this.HoverComplete));
    }

    public async void CloseSnippet()
    {
        this.HoveredEntry = default;
        this.ShowSnippet = false;
        await this.jsRuntime.HoverDelayStop();
    }

    public async void MouseMoveBuildEntry(MouseEventArgs e)
    {
        if (!this.ShowSnippet)
        {
            this.SnippetPosition = new Point((int)e.ClientX, (int)e.ClientY);
        }
    }


    [JSInvokable]
    public void HoverComplete()
    {
        this.ShowSnippet = true;
        this.RefreshView();
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

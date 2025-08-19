using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Wiki;
using Microsoft.AspNetCore.Components;
using TrailBlazr.Services;

namespace Daybreak.Views;

public sealed class TeamBuildTemplateViewModel(
    IWikiService wikiService,
    IBuildTemplateManager buildTemplateManager,
    IViewManager viewManager)
    : BuildTemplateViewModelBase<TeamBuildTemplateViewModel, TeamBuildTemplateView>(wikiService, buildTemplateManager, viewManager)
{
    public TeamBuildEntry? TeamEntry
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.TeamEntry));
        }
    }

    public int BuildEntryIndex
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.BuildEntryIndex));
        }
    }

    public bool SummaryVisible
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.SummaryVisible));
        }
    } = false;

    protected override void LoadBuild(IBuildEntry? buildEntry)
    {
        if (buildEntry is not TeamBuildEntry teamBuildEntry)
        {
            throw new InvalidOperationException($"Expected build entry to be of type {nameof(TeamBuildEntry)} but got {buildEntry?.GetType().Name}");
        }

        this.TeamEntry = teamBuildEntry;
        this.BuildEntry = teamBuildEntry.Builds.FirstOrDefault();
        this.BuildEntryIndex = 0;
    }

    protected override IBuildEntry SaveBuild()
    {
        return this.TeamEntry ?? throw new InvalidOperationException("Team build is null");
    }

    protected override void ChangeBuildName(string buildName)
    {
        if (this.TeamEntry is null)
        {
            return;
        }

        this.TeamEntry.Name = buildName;
    }

    public void SummarySkillMouseEnter(Skill skill)
    {
        this.OpenSkillSnippet(skill);
    }

    public void SummarySkillMouseLeave(Skill skill)
    {
        this.CloseSkillSnippet(skill);
    }

    public void BuildNameChanged(string buildName)
    {
        if (this.TeamEntry is null)
        {
            return;
        }

        this.TeamEntry.Name = buildName;
        this.RefreshView();
    }

    public void BuildSelectionChanged(ChangeEventArgs args)
    {
        if (this.TeamEntry is null)
        {
            return;
        }

        if (!int.TryParse(args.Value?.ToString(), out var index))
        {
            return;
        }

        this.BuildEntry = this.TeamEntry.Builds.Skip(index).FirstOrDefault();
        this.BuildEntryIndex = index;
        this.UpdateBuildCode();
    }

    public void ShowSummary()
    {
        this.SummaryVisible = true;
        this.RefreshView();
    }

    public void HideSummary()
    {
        this.SummaryVisible = false;
        this.RefreshView();
    }
}

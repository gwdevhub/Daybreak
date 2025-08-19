using Daybreak.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Wiki;
using Daybreak.Shared.Utils;
using Microsoft.AspNetCore.Components.Web;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public abstract class BuildTemplateViewModelBase<TViewModel, TView>(
    IWikiService wikiService,
    IBuildTemplateManager buildTemplateManager,
    IViewManager viewManager)
    : ViewModelBase<TViewModel, TView>
        where TViewModel : BuildTemplateViewModelBase<TViewModel, TView>
        where TView : BuildTemplateViewBase<TView, TViewModel>
{
    private static readonly TimeSpan PendingSkillTimeout = TimeSpan.FromSeconds(5);

    private readonly IWikiService wikiService = wikiService;
    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager;
    private readonly IViewManager viewManager = viewManager;

    private bool pendingSkillSnippet = false;
    private DateTimeOffset pendingSkillStartTime = DateTimeOffset.MinValue;

    public SingleBuildEntry? BuildEntry
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.BuildEntry));
        }
    }

    public string BuildTemplateCode
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.BuildTemplateCode));
        }
    } = string.Empty;

    public List<Skill> AvailableSkills
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.AvailableSkills));
        }
    } = [];

    public SkillSnippetContext? SkillSnippetContext
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.SkillSnippetContext));
        }
    }

    public (int PosX, int PosY) MousePosition
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.MousePosition));
        }
    }

    protected abstract void LoadBuild(IBuildEntry? buildEntry);
    protected abstract IBuildEntry SaveBuild();
    protected abstract void ChangeBuildName(string buildName);

    public void BuildNameChanged(string buildName)
    {
        this.ChangeBuildName(buildName);
        this.RefreshView();
    }

    public override sealed async ValueTask ParametersSet(TView view, CancellationToken cancellationToken)
    {
        var buildLoad = await this.buildTemplateManager.GetBuild(view.BuildName);
        if (!buildLoad.TryExtractSuccess(out var buildEntry))
        {
            throw new InvalidOperationException($"Failed to load build by name {view.BuildName}");
        }

        this.LoadBuild(buildEntry);
        this.UpdateBuildCode();
        this.FilterSkillsByProfessionsAndString();
        await this.RefreshViewAsync();
    }

    public void Save()
    {
        this.buildTemplateManager.SaveBuild(this.SaveBuild());
        this.viewManager.ShowView<BuildListView>();
    }

    public void AttributeIncreased(AttributeEntry attributeEntry)
    {
        if (attributeEntry.Points < 12)
        {
            attributeEntry.Points += 1;
            this.UpdateBuildCode();
            this.RefreshView();
        }
    }

    public void AttributeDecreased(AttributeEntry attributeEntry)
    {
        if (attributeEntry.Points > 0)
        {
            attributeEntry.Points -= 1;
            this.UpdateBuildCode();
            this.RefreshView();
        }
    }

    public void SkillChanged(int skillSlot, Skill skill)
    {
        if (skillSlot >= 0 ||
            skillSlot < (this.BuildEntry?.Skills.Count ?? 0))
        {
            this.BuildEntry?.Skills[skillSlot] = skill;
            this.UpdateBuildCode();
            this.RefreshView();
        }
    }

    public void SearchFilterChanged(string searchFilter)
    {
        this.FilterSkillsByProfessionsAndString(searchFilter);
        this.RefreshView();
    }

    public void PrimaryProfessionChanged(Profession primaryProfession)
    {
        if (this.BuildEntry is null)
        {
            return;
        }

        this.BuildEntry.Primary = primaryProfession;
        this.FilterSkillsByProfessionsAndString();
        this.UpdateBuildCode();
        this.RefreshView();
    }

    public void SecondaryProfessionChanged(Profession secondaryProfession)
    {
        if (this.BuildEntry is null)
        {
            return;
        }

        this.BuildEntry.Secondary = secondaryProfession;
        this.FilterSkillsByProfessionsAndString();
        this.UpdateBuildCode();
        this.RefreshView();
    }

    public async void OpenSkillSnippet(Skill skill)
    {
        if (this.pendingSkillSnippet &&
            DateTimeOffset.UtcNow - this.pendingSkillStartTime > PendingSkillTimeout)
        {
            return;
        }

        try
        {
            this.pendingSkillSnippet = true;
            this.pendingSkillStartTime = DateTimeOffset.UtcNow;
            var mousePosition = this.MousePosition;
            var description = await this.wikiService.GetSkillDescription(skill, CancellationToken.None);
            if (description is null)
            {
                return;
            }

            this.SkillSnippetContext = new SkillSnippetContext(
                mousePosition,
                skill,
                description);
            this.RefreshView();
            
            // Debounce the skill snippet to avoid flickering
            await Task.Delay(TimeSpan.FromSeconds(1), CancellationToken.None);
        }
        finally
        {
            this.pendingSkillSnippet = false;
        }
    }

    public void CloseSkillSnippet(Skill _)
    {
        if (this.pendingSkillSnippet &&
            DateTimeOffset.UtcNow - this.pendingSkillStartTime > PendingSkillTimeout)
        {
            return;
        }

        this.SkillSnippetContext = default;
        this.RefreshView();
    }

    public void OnMouseMove(MouseEventArgs e)
    {
        this.MousePosition = ((int)e.ClientX, (int)e.ClientY);
    }

    protected void UpdateBuildCode()
    {
        if (this.BuildEntry is null)
        {
            return;
        }

        this.BuildTemplateCode = this.buildTemplateManager.EncodeTemplate(this.BuildEntry);
    }

    protected void FilterSkillsByProfessionsAndString(string? searchFilter = default)
    {
        var filteredSkills = Skill.Skills.Where(skill =>
        {
            if (skill.IsPvP)
            {
                return false;
            }

            if (this.BuildEntry is null)
            {
                return true;
            }

            if (skill.Profession == Profession.None)
            {
                return true;
            }

            if (this.BuildEntry.Primary == Profession.None)
            {
                return true;
            }

            if (this.BuildEntry.Primary == skill.Profession ||
                (this.BuildEntry.Secondary == skill.Profession && this.BuildEntry.Secondary != Profession.None))
            {
                return true;
            }

            return false;
        }).Where(skill =>
        {
            if (string.IsNullOrWhiteSpace(searchFilter))
            {
                return true;
            }

            return StringUtils.MatchesSearchString(skill.Name, searchFilter);
        }).ToList();

        this.AvailableSkills.ClearAnd().AddRange(filteredSkills);
    }
}

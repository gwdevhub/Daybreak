using Daybreak.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Utils;
using Microsoft.AspNetCore.Components.Web;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public abstract class BuildTemplateViewModelBase<TViewModel, TView>(
    IBuildTemplateManager buildTemplateManager,
    IViewManager viewManager)
    : ViewModelBase<TViewModel, TView>
        where TViewModel : BuildTemplateViewModelBase<TViewModel, TView>
        where TView : BuildTemplateViewBase<TView, TViewModel>
{
    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager;
    private readonly IViewManager viewManager = viewManager;

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

    public SkillSnippetContext SkillSnippetContext
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.SkillSnippetContext));
        }
    } = new SkillSnippetContext((0, 0), Skill.None);

    public bool ShowSkillSnippet
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged(nameof(this.ShowSkillSnippet));
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

    public void BuildCodeChanged(string buildCode)
    {
        if (!this.buildTemplateManager.TryDecodeTemplate(buildCode, out var build))
        {
            return;
        }

        if (build is SingleBuildEntry s1 && this.BuildEntry is SingleBuildEntry thisBuild)
        {
            thisBuild.Skills = s1.Skills;
            thisBuild.Attributes = s1.Attributes;
            thisBuild.Primary = s1.Primary;
            thisBuild.Secondary = s1.Secondary;
        }
        else
        {
            // Currently only SingleBuildEntry is supported to change build code.
            return;
        }

        this.RefreshView();
    }

    public override sealed async ValueTask ParametersSet(TView view, CancellationToken cancellationToken)
    {
        var buildLoad = await this.buildTemplateManager.GetBuild(view.BuildName);
        if (buildLoad is null)
        {
            return;
        }

        this.LoadBuild(buildLoad);
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

    public void OpenSkillSnippet(Skill skill, MouseEventArgs e)
    {
        if (skill == Skill.None)
        {
            return;
        }

        this.SkillSnippetContext = new SkillSnippetContext(((int)e.ClientX, (int)e.ClientY), skill);
        this.ShowSkillSnippet = true;
        this.RefreshView();
    }

    public void CloseSkillSnippet(Skill skill, MouseEventArgs _)
    {
        this.ShowSkillSnippet = false;
        this.SkillSnippetContext = new SkillSnippetContext((0, 0), skill);
        this.RefreshView();
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
            if (skill.PvP)
            {
                return false;
            }

            if (this.BuildEntry is null)
            {
                return true;
            }

            if (skill.Profession == Profession.None)
            {
                return false;
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

using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Utils;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class SingleBuildTemplateViewModel(
    IViewManager viewManager,
    IBuildTemplateManager buildTemplateManager)
    : ViewModelBase<SingleBuildTemplateViewModel, SingleBuildTemplateView>
{
    private readonly IViewManager viewManager = viewManager;
    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager;

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

    public override async ValueTask ParametersSet(SingleBuildTemplateView view, CancellationToken cancellationToken)
    {
        var buildLoad = await this.buildTemplateManager.GetBuild(view.BuildName);
        if (!buildLoad.TryExtractSuccess(out var buildEntry))
        {
            throw new InvalidOperationException($"Failed to load build by name {view.BuildName}");
        }

        if (buildEntry is not SingleBuildEntry singleBuildEntry)
        {
            throw new InvalidOperationException($"Expected build entry to be of type {nameof(SingleBuildEntry)} but got {buildEntry?.GetType().Name}");
        }

        this.BuildEntry = singleBuildEntry;
        this.UpdateBuildCode();
        this.FilterSkillsByProfessionsAndString();
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

    public void BuildNameChanged(string buildName)
    {
        if (this.BuildEntry is null)
        {
            return;
        }

        this.BuildEntry.Name = buildName;
        this.UpdateBuildCode();
        this.RefreshView();
    }

    public void SaveBuild()
    {
        if (this.BuildEntry is null)
        {
            return;
        }

        this.buildTemplateManager.SaveBuild(this.BuildEntry);
        this.viewManager.ShowView<BuildListView>();
    }

    private void UpdateBuildCode()
    {
        if (this.BuildEntry is null)
        {
            return;
        }

        this.BuildTemplateCode = this.buildTemplateManager.EncodeTemplate(this.BuildEntry);
    }

    private void FilterSkillsByProfessionsAndString(string? searchFilter = default)
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

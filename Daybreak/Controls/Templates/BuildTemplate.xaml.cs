using Daybreak.Controls.Templates;
using Daybreak.Launch;
using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates;
using Daybreak.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.Templates;

/// <summary>
/// Interaction logic for BuildTemplate.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class BuildTemplate : UserControl
{
    private const string InfoNamePlaceholder = "[NAME]";
    private const string BaseAddress = $"https://wiki.guildwars.com/wiki/{InfoNamePlaceholder}";

    private bool showingSkillList = false;
    private bool replacingSecondaryProfession;
    private bool replacingPrimaryProfession;
    private IAttributePointCalculator? attributePointCalculator;
    private SkillTemplate? selectingSkillTemplate;
    private List<Skill>? skillListCache;
    private CancellationTokenSource? cancellationTokenSource = new();

    [GenerateDependencyProperty]
    private string skillSearchText = string.Empty;
    [GenerateDependencyProperty]
    private BuildEntry buildEntry;
    [GenerateDependencyProperty]
    private int attributePoints;

    [GenerateDependencyProperty]
    private List<Skill> availableSkills = new();

    public event EventHandler? BuildChanged;
    public ObservableCollection<Profession> Professions { get; } = new ObservableCollection<Profession>();

    public BuildTemplate()
        : this(Launcher.Instance.ApplicationServiceProvider.GetService<IAttributePointCalculator>()!)
    {
        
    }
    
    public BuildTemplate(
        IAttributePointCalculator attributePointCalculator)
    {
        this.attributePointCalculator = attributePointCalculator.ThrowIfNull();

        this.InitializeComponent();
        this.HideSkillListView();
        this.HideInfoBrowser();
        this.buildEntry = new BuildEntry();
        this.DataContextChanged += this.BuildTemplate_DataContextChanged;
        this.Professions.ClearAnd().AddRange(Profession.Professions);
    }

    private void BuildTemplate_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
    }

    private void BuildTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if(e.NewValue is BuildEntry buildEntry)
        {
            if (this.BuildEntry is not null)
            {
                this.BuildEntry.PropertyChanged -= this.BuildEntry_Changed;
            }
            
            this.BuildEntry = buildEntry;
            this.BuildEntry.PropertyChanged += this.BuildEntry_Changed;
            this.AttributePoints = this.attributePointCalculator!.GetRemainingFreePoints(this.BuildEntry.Build!);
            this.LoadSkills();
        }
    }

    private void BuildEntry_Changed(object? sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        this.LoadSkills();
    }

    private async void LoadSkills()
    {
        var filteredSkills = await this.FilterSkills(this.SkillSearchText).ToListAsync().ConfigureAwait(true);
        this.PrepareSkillListCache(filteredSkills);
    }

    private void BrowseToInfo(string infoName)
    {
        var address = BaseAddress.Replace(InfoNamePlaceholder, infoName.Replace(" ", "_"));
        this.SkillBrowser.Address = address;
        this.ShowInfoBrowser();
    }

    private void ShowInfoBrowser()
    {
        if (this.SkillBrowser.BrowserSupported is true)
        {
            this.HideSkillListView();
            this.SkillBrowser.Width = 400;
        }
    }

    private void HideInfoBrowser()
    {
        if (this.SkillBrowser.BrowserSupported is true)
        {
            this.SkillBrowser.Width = 0;
        }
    }

    private void ShowSkillListView()
    {
        this.HideInfoBrowser();
        this.SkillListContainer.Width = 400;
        this.SkillListContainer.Visibility = Visibility.Visible;
        this.showingSkillList = true;
        if (this.AvailableSkills is not null &&
            this.skillListCache?.Except(this.AvailableSkills).None() is true &&
            this.skillListCache.Count == this.AvailableSkills.Count)
        {
            return;
        }

        this.AvailableSkills = this.skillListCache;
    }

    private void HideSkillListView()
    {
        this.SkillListContainer.Visibility = Visibility.Hidden;
        this.SkillListContainer.Width = 0;
        this.showingSkillList = false;
    }

    private void PrepareSkillListCache(List<Skill> skills)
    {
        /*
         * To improve application performance, only load the skill list when it is showing.
         * Otherwise, defer the loading to when the skill list will show.
         */

        this.skillListCache = skills;
        if (this.showingSkillList)
        {
            this.ShowSkillListView();
        }
    }

    private async IAsyncEnumerable<Skill> FilterSkills(string searchTerm)
    {
        // Replace symbols to ease search
        searchTerm = searchTerm?.Replace("\"", "").Replace("!", "")!;

        foreach (var skill in Skill.Skills)
        {
            if (skill == Skill.NoSkill)
            {
                continue;
            }

            if (skill.Profession != this.BuildEntry!.Primary &&
                skill.Profession != this.BuildEntry!.Secondary &&
                skill.Profession != Profession.None)
            {
                continue;
            }

            if (searchTerm.IsNullOrWhiteSpace())
            {
                yield return skill;
                continue;
            }

            var matchesName = await Task.Run(() => StringUtils.MatchesSearchString(skill.Name!.Replace("\"", "").Replace("!", ""), searchTerm!));
            if (matchesName)
            {
                yield return skill;
                continue;
            }
        }
    }

    private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.HideSkillListView();
        this.HideInfoBrowser();
    }

    private void HelpButtonPrimary_Clicked(object sender, System.EventArgs e)
    {
        if (this.BuildEntry!.Primary == Profession.None)
        {
            return;
        }
        
        this.BrowseToInfo(this.BuildEntry.Primary.Name!);
        if (e is RoutedEventArgs routedEventArgs)
        {
            routedEventArgs.Handled = true;
        }
    }

    private void HelpButtonSecondary_Clicked(object sender, System.EventArgs e)
    {
        if (this.BuildEntry!.Secondary == Profession.None)
        {
            return;
        }

        this.BrowseToInfo(this.BuildEntry.Secondary.Name!);
        if (e is RoutedEventArgs routedEventArgs)
        {
            routedEventArgs.Handled = true;
        }
    }

    private void AttributeTemplate_HelpClicked(object _, AttributeEntry e)
    {
        this.BrowseToInfo(e.Attribute?.Name!);
    }

    private void AttributeTemplate_AttributeChanged(object _, AttributeEntry e)
    {
        e.ThrowIfNull();
        this.BuildChanged?.Invoke(this, new EventArgs());
        this.AttributePoints = this.attributePointCalculator!.GetRemainingFreePoints(this.BuildEntry.Build!);
    }

    private void SkillTemplate_Clicked(object sender, RoutedEventArgs e)
    {
        var skill = sender.As<SkillTemplate>().DataContext.As<Skill>();
        if (skill == Skill.NoSkill)
        {
            this.SkillSearchText = string.Empty;
            this.ShowSkillListView();
            this.selectingSkillTemplate = sender.As<SkillTemplate>();
        }
        else
        {
            this.BrowseToInfo(skill.Name!);
        }

        e.Handled = true;
    }

    private void SearchTextBox_TextChanged(object _, string e)
    {
        e.ThrowIfNull();
        this.LoadSkills();
    }

    private void SkillTemplate_RemoveClicked(object sender, System.EventArgs e)
    {
        if (sender == this.SkillTemplate0)
        {
            this.BuildEntry.FirstSkill = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate1)
        {
            this.BuildEntry.SecondSkill = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate2)
        {
            this.BuildEntry.ThirdSkill = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate3)
        {
            this.BuildEntry.FourthSkill = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate4)
        {
            this.BuildEntry.FifthSkill = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate5)
        {
            this.BuildEntry.SixthSkill = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate6)
        {
            this.BuildEntry.SeventhSkill = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate7)
        {
            this.BuildEntry.EigthSkill = Skill.NoSkill;
        }
    }

    private void SkillListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.selectingSkillTemplate is null)
        {
            this.HideSkillListView();
            return;
        }

        var selectedSkilll = sender.As<ListView>().SelectedItem.As<Skill>();
        if (this.selectingSkillTemplate == this.SkillTemplate0)
        {
            this.BuildEntry.FirstSkill = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate1)
        {
            this.BuildEntry.SecondSkill = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate2)
        {
            this.BuildEntry.ThirdSkill = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate3)
        {
            this.BuildEntry.FourthSkill = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate4)
        {
            this.BuildEntry.FifthSkill = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate5)
        {
            this.BuildEntry.SixthSkill = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate6)
        {
            this.BuildEntry.SeventhSkill = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate7)
        {
            this.BuildEntry.EigthSkill = selectedSkilll;
        }

        this.HideSkillListView();
    }

    private void AttributeTemplate_Loaded(object sender, RoutedEventArgs e)
    {
        sender.As<AttributeTemplate>().InitializeAttributeTemplate(this.attributePointCalculator!);
    }
}

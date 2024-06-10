using Daybreak.Controls.Buttons;
using Daybreak.Launch;
using Daybreak.Models;
using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Navigation;
using Daybreak.Utils;
using Daybreak.Views;
using MahApps.Metro.Controls;
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
using System.Windows.Input;

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
    private const string AttributePointInfo = "Attribute_point";

    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IViewManager viewManager;
    private readonly IAttributePointCalculator attributePointCalculator;
    private readonly CancellationTokenSource? cancellationTokenSource = new();

    private bool browserMaximized = false;
    private bool showingSkillList = false;
    private bool replacingSecondaryProfession;
    private bool replacingPrimaryProfession;
    
    private SkillTemplate? selectingSkillTemplate;
    private List<Skill>? skillListCache;

    [GenerateDependencyProperty]
    private string skillSearchText = string.Empty;
    [GenerateDependencyProperty]
    private SingleBuildEntry buildEntry;
    [GenerateDependencyProperty]
    private int attributePoints;

    [GenerateDependencyProperty]
    private List<Skill> availableSkills = [];

    public event EventHandler? BuildChanged;
    public ObservableCollection<Profession> PrimaryProfessions { get; } = [];
    public ObservableCollection<Profession> SecondaryProfessions { get; } = [];

    public BuildTemplate()
        : this(Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IBuildTemplateManager>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IAttributePointCalculator>())
    {
        
    }
    
    public BuildTemplate(
        IBuildTemplateManager buildTemplateManager,
        IViewManager viewManager,
        IAttributePointCalculator attributePointCalculator)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.attributePointCalculator = attributePointCalculator.ThrowIfNull();

        this.InitializeComponent();
        this.HideSkillListView();
        this.HideInfoBrowser();
        this.buildEntry = new SingleBuildEntry();
        this.DataContextChanged += this.BuildTemplate_DataContextChanged;
    }

    public void BrowseToUrl(string url)
    {
        this.SkillBrowser.Address = url;
        this.ShowInfoBrowser();
    }

    private void BuildTemplate_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
    }

    private void BuildTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if(e.NewValue is SingleBuildEntry buildEntry)
        {
            this.BuildEntry = buildEntry;
            this.BuildEntry.PropertyChanged += this.BuildEntry_Changed;
            this.AttributePoints = this.attributePointCalculator!.GetRemainingFreePoints(this.BuildEntry);
            this.SetupProfessions();
            this.LoadSkills();
        }
    }

    private void BuildEntry_Changed(object? sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        this.SetupProfessions();
        this.LoadSkills();
        this.AttributePoints = this.attributePointCalculator.GetRemainingFreePoints(this.BuildEntry);
        this.BuildChanged?.Invoke(this, propertyChangedEventArgs);
    }

    private void SetupProfessions()
    {
        if (this.BuildEntry is not SingleBuildEntry buildEntry ||
            buildEntry.Primary is null ||
            buildEntry.Secondary is null)
        {
            return;
        }

        var primaryProfessionsToAdd = Profession.Professions.Except(this.PrimaryProfessions).Where(p => p == Profession.None || p != buildEntry.Secondary).ToList();
        var primaryProfessionsToRemove = this.PrimaryProfessions.Where(p => p != Profession.None && p == buildEntry.Secondary).ToList();
        var secondaryProfessionsToAdd = Profession.Professions.Except(this.SecondaryProfessions).Where(p => p == Profession.None || p != buildEntry.Primary).ToList();
        var secondaryProfessionsToRemove = this.SecondaryProfessions.Where(p => p != Profession.None && p == buildEntry.Primary).ToList();

        if (primaryProfessionsToRemove.Count > 0)
        {
            foreach(var profession in primaryProfessionsToRemove)
            {
                this.PrimaryProfessions.Remove(profession);
            }
        }

        if (primaryProfessionsToAdd.Count > 0)
        {
            foreach (var profession in primaryProfessionsToAdd)
            {
                this.PrimaryProfessions.Add(profession);
            }
        }

        if (secondaryProfessionsToRemove.Count > 0)
        {
            foreach (var profession in secondaryProfessionsToRemove)
            {
                this.SecondaryProfessions.Remove(profession);
            }
        }

        if (secondaryProfessionsToAdd.Count > 0)
        {
            foreach (var profession in secondaryProfessionsToAdd)
            {
                this.SecondaryProfessions.Add(profession);
            }
        }
    }

    private async void LoadSkills()
    {
        var filteredSkills = await this.FilterSkills(this.SkillSearchText).ToListAsync().ConfigureAwait(true);
        this.PrepareSkillListCache(filteredSkills);
    }

    private void BrowseToInfo(string infoName)
    {
        var address = BaseAddress.Replace(InfoNamePlaceholder, infoName.Replace(" ", "_"));
        this.BrowseToUrl(address);
    }

    private void ShowInfoBrowser()
    {
        if (!this.SkillBrowser.BrowserSupported ||
            this.browserMaximized)
        {
            return;
        }

        this.HideSkillListView();
        this.SkillBrowser.MaxWidth = 400;
        this.SkillBrowser.Visibility = Visibility.Visible;
    }

    private void HideInfoBrowser()
    {
        if (this.SkillBrowser.BrowserSupported is true)
        {
            this.SkillBrowser.MaxWidth = 0;
            this.SkillBrowser.Visibility = Visibility.Hidden;
        }
    }

    private void ShowSkillListView()
    {
        this.HideInfoBrowser();
        this.SkillListContainer.Visibility = Visibility.Visible;
        this.showingSkillList = true;
        if (this.AvailableSkills is not null &&
            this.skillListCache?.Except(this.AvailableSkills).None() is true &&
            this.skillListCache.Count == this.AvailableSkills.Count)
        {
            return;
        }

        this.AvailableSkills = this.skillListCache;
        this.SkillListSearchTextBox.FocusOnTextBox();
    }

    private void HideSkillListView()
    {
        this.SkillListContainer.Visibility = Visibility.Collapsed;
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

    private void HelpButtonAttributePoints_Clicked(object sender, EventArgs e)
    {
        this.BrowseToInfo(AttributePointInfo);
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
        this.AttributePoints = this.attributePointCalculator!.GetRemainingFreePoints(this.BuildEntry);
    }

    private void SkillTemplate_Clicked(object sender, RoutedEventArgs e)
    {
        var skill = sender.As<SkillTemplate>()?.DataContext.As<Skill>();
        if (skill == Skill.NoSkill)
        {
            this.SkillSearchText = string.Empty;
            this.ShowSkillListView();
            this.selectingSkillTemplate = sender.As<SkillTemplate>();
        }
        else
        {
            this.BrowseToInfo(skill?.Name!);
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
            this.SkillTemplate0.DataContext = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate1)
        {
            this.BuildEntry.SecondSkill = Skill.NoSkill;
            this.SkillTemplate1.DataContext = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate2)
        {
            this.BuildEntry.ThirdSkill = Skill.NoSkill;
            this.SkillTemplate2.DataContext = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate3)
        {
            this.BuildEntry.FourthSkill = Skill.NoSkill;
            this.SkillTemplate3.DataContext = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate4)
        {
            this.BuildEntry.FifthSkill = Skill.NoSkill;
            this.SkillTemplate4.DataContext = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate5)
        {
            this.BuildEntry.SixthSkill = Skill.NoSkill;
            this.SkillTemplate5.DataContext = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate6)
        {
            this.BuildEntry.SeventhSkill = Skill.NoSkill;
            this.SkillTemplate6.DataContext = Skill.NoSkill;
        }
        else if (sender == this.SkillTemplate7)
        {
            this.BuildEntry.EigthSkill = Skill.NoSkill;
            this.SkillTemplate7.DataContext = Skill.NoSkill;
        }
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if (this.selectingSkillTemplate is null)
        {
            this.HideSkillListView();
            return;
        }

        var selectedSkilll = sender.As<HighlightButton>()?.DataContext.As<Skill>() ?? throw new InvalidOperationException();
        if (this.selectingSkillTemplate == this.SkillTemplate0)
        {
            this.BuildEntry.FirstSkill = selectedSkilll;
            this.SkillTemplate0.DataContext = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate1)
        {
            this.BuildEntry.SecondSkill = selectedSkilll;
            this.SkillTemplate1.DataContext = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate2)
        {
            this.BuildEntry.ThirdSkill = selectedSkilll;
            this.SkillTemplate2.DataContext = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate3)
        {
            this.BuildEntry.FourthSkill = selectedSkilll;
            this.SkillTemplate3.DataContext = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate4)
        {
            this.BuildEntry.FifthSkill = selectedSkilll;
            this.SkillTemplate4.DataContext = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate5)
        {
            this.BuildEntry.SixthSkill = selectedSkilll;
            this.SkillTemplate5.DataContext = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate6)
        {
            this.BuildEntry.SeventhSkill = selectedSkilll;
            this.SkillTemplate6.DataContext = selectedSkilll;
        }
        else if (this.selectingSkillTemplate == this.SkillTemplate7)
        {
            this.BuildEntry.EigthSkill = selectedSkilll;
            this.SkillTemplate7.DataContext = selectedSkilll;
        }

        this.HideSkillListView();
    }

    private void SkillBrowser_MaximizeClicked(object sender, EventArgs e)
    {
        this.browserMaximized = !this.browserMaximized;
        if (this.browserMaximized)
        {
            this.SideHolder.Children.Remove(this.SkillBrowser);
            this.FullScreenHolder.Children.Add(this.SkillBrowser);
            this.FullScreenHolder.Visibility = Visibility.Visible;
            this.SkillBrowser.MaxWidth = double.MaxValue;
        }
        else
        {
            this.FullScreenHolder.Children.Remove(this.SkillBrowser);
            this.SideHolder.Children.Add(this.SkillBrowser);
            this.FullScreenHolder.Visibility = Visibility.Hidden;
            this.SkillBrowser.MaxWidth = 400;
        }

        if (e is MouseButtonEventArgs mouseButtonEventArgs)
        {
            mouseButtonEventArgs.Handled = true;
        }
    }

    private void SkillBrowser_BuildDecoded(object _, DownloadedBuild e)
    {
        if (e is null)
        {
            return;
        }

        e.Build!.Name = e.PreferredName ?? Guid.NewGuid().ToString();
        this.viewManager.ShowView<SingleBuildTemplateView>(e.Build);
    }
}

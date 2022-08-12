using Daybreak.Configuration;
using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.IconRetrieve;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for BuildTemplate.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class BuildTemplate : UserControl
    {
        private const string InfoNamePlaceholder = "[NAME]";
        private const string BaseAddress = $"https://wiki.guildwars.com/wiki/{InfoNamePlaceholder}";

        private bool suppressBuildChanged = false;
        private bool loadedProperties = false;
        private IIconBrowser iconBrowser;
        private BuildEntry loadedBuild;
        private SkillTemplate selectingSkillTemplate;
        private CancellationTokenSource cancellationTokenSource = new();

        public event EventHandler BuildChanged;

        [GenerateDependencyProperty]
        private Profession primaryProfession;
        [GenerateDependencyProperty]
        private Profession secondaryProfession;
        [GenerateDependencyProperty]
        private Skill skill0;
        [GenerateDependencyProperty]
        private Skill skill1;
        [GenerateDependencyProperty]
        private Skill skill2;
        [GenerateDependencyProperty]
        private Skill skill3;
        [GenerateDependencyProperty]
        private Skill skill4;
        [GenerateDependencyProperty]
        private Skill skill5;
        [GenerateDependencyProperty]
        private Skill skill6;
        [GenerateDependencyProperty]
        private Skill skill7;
        public ObservableCollection<Skill> AvailableSkills { get; } = new ObservableCollection<Skill>();
        public ObservableCollection<AttributeEntry> Attributes { get; } = new ObservableCollection<AttributeEntry>();
        public ObservableCollection<Profession> Professions { get; } = new ObservableCollection<Profession>(Profession.Professions);

        public BuildTemplate()
        {
            this.InitializeComponent();
            this.InitializeProperties();
            this.DataContextChanged += this.BuildTemplate_DataContextChanged;
        }

        public void InitializeTemplate(
            IIconRetriever iconRetriever,
            IIconBrowser iconBrowser,
            ILiveOptions<ApplicationConfiguration> liveOptions,
            IBuildTemplateManager buildTemplateManager,
            ILogger<ChromiumBrowserWrapper> logger)
        {
            this.iconBrowser = iconBrowser.ThrowIfNull();
            this.SkillBrowser.InitializeBrowser(liveOptions, buildTemplateManager, logger);
            this.iconBrowser.InitializeWebView(this.SkillBrowser.WebBrowser, this.cancellationTokenSource.Token);
            this.SkillTemplate0.InitializeSkillTemplate(iconRetriever);
            this.SkillTemplate1.InitializeSkillTemplate(iconRetriever);
            this.SkillTemplate2.InitializeSkillTemplate(iconRetriever);
            this.SkillTemplate3.InitializeSkillTemplate(iconRetriever);
            this.SkillTemplate4.InitializeSkillTemplate(iconRetriever);
            this.SkillTemplate5.InitializeSkillTemplate(iconRetriever);
            this.SkillTemplate6.InitializeSkillTemplate(iconRetriever);
            this.SkillTemplate7.InitializeSkillTemplate(iconRetriever);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (this.loadedProperties is false)
            {
                return;
            }

            if (e.Property == PrimaryProfessionProperty || e.Property == SecondaryProfessionProperty)
            {
                if (e.Property == PrimaryProfessionProperty)
                {
                    this.loadedBuild.Build.Primary = this.PrimaryProfession;
                }
                else
                {
                    this.loadedBuild.Build.Secondary = this.SecondaryProfession;
                }

                this.LoadSkills();
                this.LoadAttributes();
                if (this.suppressBuildChanged is false)
                {
                    this.BuildChanged?.Invoke(this, new EventArgs());
                }
            }

            if (e.Property == Skill0Property ||
                e.Property == Skill1Property ||
                e.Property == Skill2Property ||
                e.Property == Skill3Property ||
                e.Property == Skill4Property ||
                e.Property == Skill5Property ||
                e.Property == Skill6Property ||
                e.Property == Skill7Property)
            {
                if (this.suppressBuildChanged is false)
                {
                    this.loadedBuild.Build.Skills[0] = this.Skill0;
                    this.loadedBuild.Build.Skills[1] = this.Skill1;
                    this.loadedBuild.Build.Skills[2] = this.Skill2;
                    this.loadedBuild.Build.Skills[3] = this.Skill3;
                    this.loadedBuild.Build.Skills[4] = this.Skill4;
                    this.loadedBuild.Build.Skills[5] = this.Skill5;
                    this.loadedBuild.Build.Skills[6] = this.Skill6;
                    this.loadedBuild.Build.Skills[7] = this.Skill7;
                    this.BuildChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private void BuildTemplate_Unloaded(object sender, RoutedEventArgs e)
        {
            this.cancellationTokenSource.Cancel();
        }

        private void InitializeProperties()
        {
            this.PrimaryProfession = Profession.None;
            this.SecondaryProfession = Profession.None;
            this.Skill0 = Skill.NoSkill;
            this.Skill1 = Skill.NoSkill;
            this.Skill2 = Skill.NoSkill;
            this.Skill3 = Skill.NoSkill;
            this.Skill4 = Skill.NoSkill;
            this.Skill5 = Skill.NoSkill;
            this.Skill6 = Skill.NoSkill;
            this.Skill7 = Skill.NoSkill;
            this.loadedProperties = true;
        }

        private void BuildTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue is BuildEntry)
            {
                this.LoadBuild();
                this.LoadSkills();
                this.LoadAttributes();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.HideSkillListView();
            this.HideInfoBrowser();
        }

        private void LoadAttributes()
        {
            var possibleAttributes = new List<AttributeEntry>();
            if (this.PrimaryProfession != Profession.None)
            {
                possibleAttributes.Add(new AttributeEntry { Attribute = this.PrimaryProfession.PrimaryAttribute });
                possibleAttributes.AddRange(this.PrimaryProfession.Attributes.Select(a => new AttributeEntry { Attribute = a }));
            }

            if (this.SecondaryProfession != Profession.None && this.SecondaryProfession != this.PrimaryProfession)
            {
                possibleAttributes.AddRange(this.SecondaryProfession.Attributes.Select(a => new AttributeEntry { Attribute = a }));
            }

            this.Attributes.ClearAnd().AddRange(possibleAttributes.Select(entry =>
            {
                var maybePresentAttribute = this.loadedBuild.Build.Attributes.Where(buildEntry => entry.Attribute == buildEntry.Attribute).FirstOrDefault();
                if (maybePresentAttribute is null)
                {
                    return entry;
                }

                entry.Points = maybePresentAttribute.Points;
                return entry;
            }));

            this.loadedBuild.Build.Attributes = this.Attributes.ToList();
        }

        private void LoadSkills()
        {
            var possibleSkills = Skill.Skills
                .Where(s => s.Profession == this.PrimaryProfession || s.Profession == this.SecondaryProfession || s.Profession == Profession.None)
                .Where(s => s != Skill.NoSkill)
                .OrderBy(s => s.Name);
            this.AvailableSkills.ClearAnd().AddRange(possibleSkills);

            if (this.Skill0.Profession != this.PrimaryProfession &&
                this.Skill0.Profession != this.SecondaryProfession &&
                this.Skill0.Profession != Profession.None)
            {
                this.Skill0 = Skill.NoSkill;
            }

            if (this.Skill1.Profession != this.PrimaryProfession &&
                this.Skill1.Profession != this.SecondaryProfession &&
                this.Skill1.Profession != Profession.None)
            {
                this.Skill1 = Skill.NoSkill;
            }

            if (this.Skill2.Profession != this.PrimaryProfession &&
                this.Skill2.Profession != this.SecondaryProfession &&
                this.Skill2.Profession != Profession.None)
            {
                this.Skill2 = Skill.NoSkill;
            }

            if (this.Skill3.Profession != this.PrimaryProfession &&
                this.Skill3.Profession != this.SecondaryProfession &&
                this.Skill3.Profession != Profession.None)
            {
                this.Skill3 = Skill.NoSkill;
            }

            if (this.Skill4.Profession != this.PrimaryProfession &&
                this.Skill4.Profession != this.SecondaryProfession &&
                this.Skill4.Profession != Profession.None)
            {
                this.Skill4 = Skill.NoSkill;
            }

            if (this.Skill5.Profession != this.PrimaryProfession &&
                this.Skill5.Profession != this.SecondaryProfession &&
                this.Skill5.Profession != Profession.None)
            {
                this.Skill5 = Skill.NoSkill;
            }

            if (this.Skill6.Profession != this.PrimaryProfession &&
                this.Skill6.Profession != this.SecondaryProfession &&
                this.Skill6.Profession != Profession.None)
            {
                this.Skill6 = Skill.NoSkill;
            }

            if (this.Skill7.Profession != this.PrimaryProfession &&
                this.Skill7.Profession != this.SecondaryProfession &&
                this.Skill7.Profession != Profession.None)
            {
                this.Skill7 = Skill.NoSkill;
            }
        }

        private void LoadBuild()
        {
            this.suppressBuildChanged = true;
            var build = this.DataContext.As<BuildEntry>();
            this.loadedBuild = build;
            this.PrimaryProfession = build.Build.Primary;
            this.SecondaryProfession = build.Build.Secondary;
            this.Skill0 = build.Build.Skills[0];
            this.Skill1 = build.Build.Skills[1];
            this.Skill2 = build.Build.Skills[2];
            this.Skill3 = build.Build.Skills[3];
            this.Skill4 = build.Build.Skills[4];
            this.Skill5 = build.Build.Skills[5];
            this.Skill6 = build.Build.Skills[6];
            this.Skill7 = build.Build.Skills[7];
            this.suppressBuildChanged = false;
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
                this.SkillBrowser.Width = 400;
                this.SkillsListView.Width = 0;
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
            this.SkillBrowser.Width = 0;
            this.SkillsListView.Width = 400;
        }

        private void HideSkillListView()
        {
            this.SkillsListView.Width = 0;
        }

        private void HelpButtonPrimary_Clicked(object sender, System.EventArgs e)
        {
            if (this.PrimaryProfession == Profession.None)
            {
                return;
            }

            this.BrowseToInfo(this.PrimaryProfession.Name);
            if (e is RoutedEventArgs routedEventArgs)
            {
                routedEventArgs.Handled = true;
            }
        }

        private void HelpButtonSecondary_Clicked(object sender, System.EventArgs e)
        {
            if (this.SecondaryProfession == Profession.None)
            {
                return;
            }

            this.BrowseToInfo(this.SecondaryProfession.Name);
            if (e is RoutedEventArgs routedEventArgs)
            {
                routedEventArgs.Handled = true;
            }
        }

        private void AttributeTemplate_HelpClicked(object sender, AttributeEntry e)
        {
            this.BrowseToInfo(e.Attribute.Name);
        }

        private void AttributeTemplate_AttributeChanged(object sender, AttributeEntry e)
        {
            this.BuildChanged?.Invoke(this, new EventArgs());
        }

        private void SkillTemplate_Clicked(object sender, RoutedEventArgs e)
        {
            var skill = sender.As<SkillTemplate>().DataContext.As<Skill>();
            if (skill == Skill.NoSkill)
            {
                this.ShowSkillListView();
                this.selectingSkillTemplate = sender.As<SkillTemplate>();
            }
            else
            {
                this.BrowseToInfo(skill.Name);
            }

            e.Handled = true;
        }

        private void SkillTemplate_RemoveClicked(object sender, System.EventArgs e)
        {
            sender.As<SkillTemplate>().DataContext = Skill.NoSkill;
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.selectingSkillTemplate is null)
            {
                this.HideSkillListView();
                return;
            }

            this.selectingSkillTemplate.DataContext = sender.As<ListView>().SelectedItem;
            this.HideSkillListView();
            this.loadedBuild.Build.Skills[0] = this.Skill0;
            this.loadedBuild.Build.Skills[1] = this.Skill1;
            this.loadedBuild.Build.Skills[2] = this.Skill2;
            this.loadedBuild.Build.Skills[3] = this.Skill3;
            this.loadedBuild.Build.Skills[4] = this.Skill4;
            this.loadedBuild.Build.Skills[5] = this.Skill5;
            this.loadedBuild.Build.Skills[6] = this.Skill6;
            this.loadedBuild.Build.Skills[7] = this.Skill7;
        }

        private void ListView_NavigateWithMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (e.Delta > 0)
            {
                sender.As<ListView>().SelectedIndex = sender.As<ListView>().SelectedIndex > 0 ?
                    sender.As<ListView>().SelectedIndex - 1 :
                    0;
            }
            else
            {
                sender.As<ListView>().SelectedIndex = sender.As<ListView>().SelectedIndex < sender.As<ListView>().Items.Count - 1 ?
                    sender.As<ListView>().SelectedIndex + 1 :
                    sender.As<ListView>().Items.Count;
            }
        }
    }
}

using Daybreak.Controls.Buttons;
using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for BuildTemplatesView.xaml
/// </summary>
public partial class TeamBuildTemplateView : UserControl
{
    private const string DisallowedChars = "\r\n/.";

    private readonly INotificationService notificationService;
    private readonly IViewManager viewManager;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly ILogger<TeamBuildTemplateView> logger;

    private bool preventDecode = false;
    private string previousCode = string.Empty;

    [GenerateDependencyProperty(InitialValue = false)]
    private bool saveButtonEnabled;
    [GenerateDependencyProperty]
    private SingleBuildEntry selectedBuild = default!;
    [GenerateDependencyProperty]
    private TeamBuildEntry currentBuild = default!;
    [GenerateDependencyProperty]
    private string currentBuildCode = string.Empty;
    [GenerateDependencyProperty]
    private string currentBuildSource = string.Empty;
    [GenerateDependencyProperty]
    private string currentBuildSubCode = string.Empty;

    public TeamBuildTemplateView(
        INotificationService notificationService,
        IViewManager viewManager,
        IBuildTemplateManager buildTemplateManager,
        ILogger<TeamBuildTemplateView> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
        this.DataContextChanged += (sender, contextArgs) =>
        {
            if (contextArgs.NewValue is TeamBuildEntry buildEntry)
            {
                this.logger.LogInformation("Received data context. Setting current build");
                this.CurrentBuild = buildEntry;
                this.preventDecode = true;
                this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild);
                this.CurrentBuildSource = buildEntry.SourceUrl;
                this.SelectedBuild = this.CurrentBuild.Builds.FirstOrDefault();
                this.preventDecode = false;
                this.CurrentBuild.PropertyChanged += this.CurrentBuild_PropertyChanged;
            }
        };
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == CurrentBuildCodeProperty)
        {
            if (this.preventDecode)
            {
                return;
            }

            // Ignore whitespace changes
            if (this.previousCode.Trim() == this.CurrentBuildCode.Trim())
            {
                return;
            }

            this.logger.LogInformation($"Attempting to decode provided template {this.CurrentBuildCode}");
            try
            {
                var newBuild = this.buildTemplateManager.DecodeTemplate(this.CurrentBuildCode);
                if (newBuild is TeamBuildEntry teamBuildEntry)
                {
                    teamBuildEntry.Name = this.CurrentBuild.Name;
                    teamBuildEntry.PreviousName = this.CurrentBuild.PreviousName;
                    if (string.IsNullOrEmpty(this.CurrentBuild.PreviousName))
                    {
                        newBuild.Metadata = this.CurrentBuild.Metadata;
                    }

                    var indexOfSelectedBuild = 0;
                    if (this.CurrentBuild is not null &&
                        this.SelectedBuild is not null)
                    {
                        indexOfSelectedBuild = this.CurrentBuild.Builds.IndexOf(this.SelectedBuild);
                    }

                    this.CurrentBuild = teamBuildEntry;
                    this.SelectedBuild = this.CurrentBuild.Builds.Skip(indexOfSelectedBuild).FirstOrDefault();
                    this.previousCode = this.CurrentBuildCode;
                    this.logger.LogInformation($"Template {this.CurrentBuildCode} decoded");
                }
                else if (newBuild is SingleBuildEntry singleBuildEntry)
                {
                    this.CurrentBuild.Builds = [singleBuildEntry];
                    this.SelectedBuild = singleBuildEntry;
                    this.logger.LogInformation($"Template {this.CurrentBuildCode} decoded into {nameof(SingleBuildEntry)}");
                }
            }
            catch
            {
                this.logger.LogWarning($"Failed to decode {this.CurrentBuildCode}. Reverting to default build");
                var newBuild = this.buildTemplateManager.CreateTeamBuild();
                newBuild.Name = this.CurrentBuild.Name;
                newBuild.PreviousName = this.CurrentBuild.PreviousName;
                this.CurrentBuild = newBuild;
                this.previousCode = this.CurrentBuildCode;
            }

            this.CurrentBuild.PropertyChanged += this.CurrentBuild_PropertyChanged;
        }
        else if (e.Property == SelectedBuildProperty)
        {
            this.preventDecode = true;
            this.CurrentBuildSubCode = this.SelectedBuild is not null ? this.buildTemplateManager.EncodeTemplate(this.SelectedBuild) : string.Empty;
            this.preventDecode = false;
        }
        else if (e.Property == CurrentBuildSubCodeProperty)
        {
            if (this.preventDecode)
            {
                return;
            }

            if (this.CurrentBuild is null)
            {
                this.CurrentBuildSubCode = string.Empty;
                return;
            }

            try
            {
                var newSelectedBuild = this.buildTemplateManager.DecodeTemplate(this.CurrentBuildSubCode);
                if (newSelectedBuild is not SingleBuildEntry newSingleBuildEntry)
                {
                    return;
                }

                // Manually write all properties to trigger bindings
                this.SelectedBuild.Primary = newSingleBuildEntry.Primary;
                this.SelectedBuild.Secondary = newSingleBuildEntry.Secondary;
                this.SelectedBuild.Attributes = newSingleBuildEntry.Attributes;
                this.SelectedBuild.FirstSkill = newSingleBuildEntry.FirstSkill;
                this.SelectedBuild.SecondSkill = newSingleBuildEntry.SecondSkill;
                this.SelectedBuild.ThirdSkill = newSingleBuildEntry.ThirdSkill;
                this.SelectedBuild.FourthSkill = newSingleBuildEntry.FourthSkill;
                this.SelectedBuild.FifthSkill = newSingleBuildEntry.FifthSkill;
                this.SelectedBuild.SixthSkill = newSingleBuildEntry.SixthSkill;
                this.SelectedBuild.SeventhSkill = newSingleBuildEntry.SeventhSkill;
                this.SelectedBuild.EigthSkill = newSingleBuildEntry.EigthSkill;
            }
            catch
            {
                this.SelectedBuild.Primary = Profession.None;
                this.SelectedBuild.Secondary = Profession.None;
                this.SelectedBuild.Attributes = [];
                this.SelectedBuild.Skills = [ Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill ];
            }
        }
    }

    private void CurrentBuild_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BuildTemplate_BuildChanged(sender!, e);
    }

    private void BuildTemplate_BuildChanged(object sender, EventArgs e)
    {
        try
        {
            this.preventDecode = true;
            this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild);
            this.previousCode = this.CurrentBuildCode;
            this.CurrentBuildSubCode = this.SelectedBuild is not null ? this.buildTemplateManager.EncodeTemplate(this.SelectedBuild) : string.Empty;
            this.preventDecode = false;
        }
        finally
        {
        }
    }
    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<BuildsListView>();
    }
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (this.CurrentBuild is null)
        {
            this.viewManager.ShowView<BuildsListView>();
            return;
        }

        this.CurrentBuild.SourceUrl = this.CurrentBuildSource;
        this.buildTemplateManager.SaveBuild(this.CurrentBuild);
        this.viewManager.ShowView<BuildsListView>();
    }

    private void BrowserButton_Clicked(object sender, EventArgs e)
    {
        if (this.CurrentBuildSource.IsNullOrWhiteSpace())
        {
            return;
        }

        this.BuildTemplate.BrowseToUrl(this.CurrentBuildSource);
    }

    private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (DisallowedChars.ToCharArray().Where(e.Text.Contains).Any())
        {
            e.Handled = true;
        }
    }
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(sender.As<TextBox>()?.Text))
        {
            this.SaveButtonEnabled = false;
        }
        else
        {
            this.SaveButtonEnabled = true;
        }
    }

    private void BinButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not BinButton binButton)
        {
            return;
        }

        if (binButton.DataContext is not SingleBuildEntry singleBuildEntry)
        {
            return;
        }

        // We need to reset the list to trigger the binding. Simply removing the item from the list will not trigger the binding
        if (this.SelectedBuild == singleBuildEntry)
        {
            // We need to perform the check before we change the build collection, otherwise SelectedBuild will be null
            this.CurrentBuild.Builds = this.CurrentBuild.Builds.Where(b => b != singleBuildEntry).ToList();
            this.SelectedBuild = this.CurrentBuild.Builds.FirstOrDefault();
        }
        else
        {
            this.CurrentBuild.Builds = this.CurrentBuild.Builds.Where(b => b != singleBuildEntry).ToList();
        }
    }

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        var newBuild = this.buildTemplateManager.CreateSingleBuild();
        // We need to reset the list to trigger the binding. Simply adding the item from the list will not trigger the binding
        this.CurrentBuild.Builds = [.. this.CurrentBuild.Builds, newBuild];
    }

    private void CopyButton_SubCodeClicked(object sender, EventArgs e)
    {
        if (this.CurrentBuildSubCode.IsNullOrWhiteSpace())
        {
            return;
        }

        Clipboard.SetText(this.CurrentBuildSubCode);
        this.notificationService.NotifyInformation(
            "Copied sub-build code",
            $"Copied {this.CurrentBuildSubCode} code to clipboard");
    }

    private void CopyButton_CodeClicked(object sender, EventArgs e)
    {
        if (this.CurrentBuildCode.IsNullOrWhiteSpace())
        {
            return;
        }

        Clipboard.SetText(this.CurrentBuildCode);
        this.notificationService.NotifyInformation(
            "Copied team build code",
            $"Copied {this.CurrentBuildCode} code to clipboard");
    }
}


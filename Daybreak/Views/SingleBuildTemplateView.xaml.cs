using Daybreak.Models.Builds;
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
public partial class SingleBuildTemplateView : UserControl
{
    private const string DisallowedChars = "\r\n/.";

    private readonly INotificationService notificationService;
    private readonly IViewManager viewManager;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly ILogger<SingleBuildTemplateView> logger;

    private bool preventDecode = false;

    [GenerateDependencyProperty(InitialValue = false)]
    private bool saveButtonEnabled;
    [GenerateDependencyProperty]
    private SingleBuildEntry currentBuild = default!;
    [GenerateDependencyProperty]
    private string currentBuildCode = string.Empty;
    [GenerateDependencyProperty]
    private string currentBuildSource = string.Empty;

    public SingleBuildTemplateView(
        INotificationService notificationService,
        IViewManager viewManager,
        IBuildTemplateManager buildTemplateManager,
        ILogger<SingleBuildTemplateView> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
        this.DataContextChanged += (sender, contextArgs) =>
        {
            if (contextArgs.NewValue is SingleBuildEntry buildEntry)
            {
                this.logger.LogInformation("Received data context. Setting current build");
                this.CurrentBuild = buildEntry;
                this.preventDecode = true;
                this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild);
                this.CurrentBuildSource = buildEntry.SourceUrl;
                this.preventDecode = false;
            }
        };
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == CurrentBuildCodeProperty &&
            this.preventDecode is false)
        {
            this.logger.LogInformation($"Attempting to decode provided template {this.CurrentBuildCode}");
            try
            {
                var newBuild = this.buildTemplateManager.DecodeTemplate(this.CurrentBuildCode);
                if (newBuild is not SingleBuildEntry singleBuildEntry)
                {
                    throw new InvalidOperationException();
                }

                newBuild.Name = this.CurrentBuild.Name;
                newBuild.PreviousName = this.CurrentBuild.PreviousName;
                if (string.IsNullOrEmpty(this.CurrentBuild.PreviousName))
                {
                    newBuild.Metadata = this.CurrentBuild.Metadata;
                }

                this.CurrentBuild = singleBuildEntry;
                this.logger.LogInformation($"Template {this.CurrentBuildCode} decoded");
            }
            catch
            {
                this.logger.LogWarning($"Failed to decode {this.CurrentBuildCode}. Reverting to default build");
                var newBuild = this.buildTemplateManager.CreateSingleBuild();
                newBuild.Name = this.CurrentBuild.Name;
                newBuild.PreviousName = this.CurrentBuild.PreviousName;
                this.CurrentBuild = newBuild;
            }
        }
    }

    private void BuildTemplate_BuildChanged(object sender, EventArgs e)
    {
        try
        {
            this.preventDecode = true;
            this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild);
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

    private void CircularButton_Clicked(object sender, EventArgs e)
    {
        if (this.CurrentBuild is null)
        {
            return;
        }

        var newTeamBuild = this.buildTemplateManager.CreateTeamBuild();
        newTeamBuild.Name = this.CurrentBuild.Name;
        newTeamBuild.PreviousName = this.CurrentBuild.PreviousName;
        newTeamBuild.Builds.ClearAnd().Add(this.CurrentBuild);
        this.viewManager.ShowView<TeamBuildTemplateView>(newTeamBuild);
    }

    private void CopyButton_Clicked(object sender, EventArgs e)
    {
        if (this.CurrentBuildCode.IsNullOrWhiteSpace())
        {
            return;
        }

        Clipboard.SetText(this.CurrentBuildCode);
        this.notificationService.NotifyInformation(
            "Copied build code",
            $"Copied {this.CurrentBuildCode} code to clipboard");
    }
}

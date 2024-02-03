using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Navigation;
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
public partial class BuildTemplateView : UserControl
{
    private const string DisallowedChars = "\r\n/.";

    private readonly IViewManager viewManager;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly ILogger<BuildTemplateView> logger;

    private bool preventDecode = false;

    [GenerateDependencyProperty(InitialValue = false)]
    private bool saveButtonEnabled;
    [GenerateDependencyProperty]
    private IBuildEntry currentBuild = default!;
    [GenerateDependencyProperty]
    private string currentBuildCode = string.Empty;
    [GenerateDependencyProperty]
    private string currentBuildSource = string.Empty;

    public BuildTemplateView(
        IViewManager viewManager,
        IBuildTemplateManager buildTemplateManager,
        ILogger<BuildTemplateView> logger)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
        this.DataContextChanged += (sender, contextArgs) =>
        {
            if (contextArgs.NewValue is IBuildEntry buildEntry)
            {
                this.logger.LogInformation("Received data context. Setting current build");
                this.CurrentBuild = buildEntry;
                this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild);
                this.CurrentBuildSource = buildEntry.SourceUrl;
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
                newBuild.Name = this.CurrentBuild.Name;
                newBuild.PreviousName = this.CurrentBuild.PreviousName;
                this.CurrentBuild = newBuild;
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
}

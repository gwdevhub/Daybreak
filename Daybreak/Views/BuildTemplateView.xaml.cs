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
    private readonly IAttributePointCalculator attributePointCalculator;
    private readonly ILogger<BuildTemplateView> logger;

    private bool preventDecode = false;

    [GenerateDependencyProperty(InitialValue = false)]
    private bool saveButtonEnabled;
    [GenerateDependencyProperty]
    private BuildEntry currentBuild = default!;
    [GenerateDependencyProperty]
    private string currentBuildCode = string.Empty;
    [GenerateDependencyProperty]
    private int attributePoints;

    public BuildTemplateView(
        IViewManager viewManager,
        IBuildTemplateManager buildTemplateManager,
        IAttributePointCalculator attributePointCalculator,
        ILogger<BuildTemplateView> logger)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.attributePointCalculator = attributePointCalculator.ThrowIfNull();
        this.InitializeComponent();
        this.DataContextChanged += (sender, contextArgs) =>
        {
            if (contextArgs.NewValue is BuildEntry buildEntry)
            {
                this.logger.LogInformation("Received data context. Setting current build");
                this.CurrentBuild = buildEntry;
                this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild.Build!);
                this.AttributePoints = this.attributePointCalculator.GetRemainingFreePoints(this.CurrentBuild.Build!);
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
                this.CurrentBuild = new BuildEntry
                {
                    Name = this.CurrentBuild.Name,
                    PreviousName = this.CurrentBuild.PreviousName,
                    Build = this.buildTemplateManager.DecodeTemplate(this.CurrentBuildCode)
                };

                this.logger.LogInformation($"Template {this.CurrentBuildCode} decoded");
            }
            catch
            {
                this.logger.LogWarning($"Failed to decode {this.CurrentBuildCode}. Reverting to default build");
                this.CurrentBuild = new BuildEntry
                {
                    Name = this.CurrentBuild.Name,
                    PreviousName = this.CurrentBuild.PreviousName,
                    Build = new Build()
                };
            }

            this.AttributePoints = this.attributePointCalculator.GetRemainingFreePoints(this.CurrentBuild.Build!);
        }
    }

    private void BuildTemplate_BuildChanged(object sender, EventArgs e)
    {
        try
        {
            this.preventDecode = true;
            this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild.Build!);
            this.preventDecode = false;
            this.AttributePoints = this.attributePointCalculator.GetRemainingFreePoints(this.CurrentBuild.Build!);
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
        this.buildTemplateManager.SaveBuild(this.CurrentBuild);
        this.viewManager.ShowView<BuildsListView>();
    }

    private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (DisallowedChars.ToCharArray().Where(c => e.Text.Contains(c)).Any())
        {
            e.Handled = true;
        }
    }
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(sender.As<TextBox>().Text))
        {
            this.SaveButtonEnabled = false;
        }
        else
        {
            this.SaveButtonEnabled = true;
        }
    }
}

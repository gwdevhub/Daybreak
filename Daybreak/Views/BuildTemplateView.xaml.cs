using Daybreak.Controls;
using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Configuration;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.ViewManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for BuildTemplatesView.xaml
    /// </summary>
    public partial class BuildTemplateView : UserControl
    {
        private const string DisallowedChars = "\r\n\\/.";

        private readonly IViewManager viewManager;
        private readonly IBuildTemplateManager buildTemplateManager;
        private readonly ILogger<BuildTemplateView> logger;

        [GenerateDependencyProperty(InitialValue = false)]
        private bool saveButtonEnabled;
        [GenerateDependencyProperty]
        private BuildEntry currentBuild;
        [GenerateDependencyProperty]
        private string currentBuildCode;

        public BuildTemplateView(
            IViewManager viewManager,
            IBuildTemplateManager buildTemplateManager,
            IIconRetriever iconRetriever,
            IConfigurationManager configurationManager,
            ILogger<ChromiumBrowserWrapper> chromiumLogger,
            ILogger<BuildTemplateView> logger)
        {
            this.buildTemplateManager = buildTemplateManager.ThrowIfNull(nameof(buildTemplateManager));
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.BuildTemplate.InitializeTemplate(iconRetriever, configurationManager, buildTemplateManager, chromiumLogger);
            this.DataContextChanged += (sender, contextArgs) =>
            {
                if (contextArgs.NewValue is BuildEntry)
                {
                    this.logger.LogInformation("Received data context. Setting current build");
                    this.CurrentBuild = contextArgs.NewValue.As<BuildEntry>();
                    this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild.Build);
                }
            };
        }

        private void BuildCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.logger.LogInformation($"Attempting to decode provided template {sender.As<TextBox>().Text}");
                try
                {
                    this.CurrentBuild = new BuildEntry
                    {
                        Name = this.CurrentBuild.Name,
                        PreviousName = this.CurrentBuild.PreviousName,
                        Build = this.buildTemplateManager.DecodeTemplate(sender.As<TextBox>().Text)
                    };

                    this.logger.LogInformation($"Template {sender.As<TextBox>().Text} decoded");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to decode template {sender.As<TextBox>().Text}", ex);
                }
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
}

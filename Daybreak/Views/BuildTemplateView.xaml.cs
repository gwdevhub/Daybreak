using Daybreak.Configuration;
using Daybreak.Controls;
using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for BuildTemplatesView.xaml
    /// </summary>
    public partial class BuildTemplateView : UserControl
    {
        private const string DisallowedChars = "\r\n/.";

        private bool supressDecode = false;

        private readonly IViewManager viewManager;
        private readonly IBuildTemplateManager buildTemplateManager;
        private readonly ILogger<BuildTemplateView> logger;

        [GenerateDependencyProperty(InitialValue = false)]
        private bool saveButtonEnabled;
        [GenerateDependencyProperty]
        private BuildEntry currentBuild = default!;
        [GenerateDependencyProperty]
        private string currentBuildCode = string.Empty;

        public BuildTemplateView(
            IViewManager viewManager,
            IBuildTemplateManager buildTemplateManager,
            IIconCache iconRetriever,
            IIconBrowser iconBrowser,
            ILiveOptions<ApplicationConfiguration> liveOptions,
            ILogger<ChromiumBrowserWrapper> chromiumLogger,
            ILogger<BuildTemplateView> logger)
        {
            this.buildTemplateManager = buildTemplateManager.ThrowIfNull(nameof(buildTemplateManager));
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.InitializeComponent();
            this.BuildTemplate.InitializeTemplate(iconRetriever, iconBrowser, liveOptions, buildTemplateManager, chromiumLogger);
            this.DataContextChanged += (sender, contextArgs) =>
            {
                if (contextArgs.NewValue is BuildEntry)
                {
                    this.logger.LogInformation("Received data context. Setting current build");
                    this.CurrentBuild = contextArgs.NewValue.As<BuildEntry>();
                    this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild.Build!);
                }
            };
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == CurrentBuildCodeProperty && this.supressDecode is false)
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
            }
        }

        private void BuildTemplate_BuildChanged(object sender, EventArgs e)
        {
            try
            {
                this.supressDecode = true;
                this.CurrentBuildCode = this.buildTemplateManager.EncodeTemplate(this.CurrentBuild.Build!);
            }
            finally
            {
                this.supressDecode = false;
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

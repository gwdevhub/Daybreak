using Daybreak.Models.Builds;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.ViewManagement;
using Microsoft.Extensions.Logging;
using Models;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views
{
    /// <summary>
    /// Interaction logic for UpdateView.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class IconDownloadView : UserControl
    {
        private static bool CompleteIcons = false;

        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly IIconRetriever iconRetriever;
        private readonly IIconBrowser iconBrowser;
        private readonly IViewManager viewManager;
        private readonly ILogger<IconDownloadView> logger;

        [GenerateDependencyProperty]
        private string description;
        [GenerateDependencyProperty]
        private double progressValue;

        public IconDownloadView(
            IIconRetriever iconRetriever,
            IIconBrowser iconBrowser,
            IViewManager viewManager,
            ILogger<IconDownloadView> logger)
        {
            this.iconRetriever = iconRetriever.ThrowIfNull();
            this.iconBrowser = iconBrowser.ThrowIfNull();
            this.viewManager = viewManager.ThrowIfNull();
            this.logger = logger.ThrowIfNull();
            this.InitializeComponent();
            this.Description = "Verifying icons";
        }

        private async void IconDownloadView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await this.WebView.WebBrowser.EnsureCoreWebView2Async();
            this.DownloadSkills();
        }

        private void IconDownloadView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.cancellationTokenSource.Cancel();
        }

        private void OpaqueButton_Clicked(object sender, System.EventArgs e)
        {
            this.viewManager.ShowView<BuildsListView>();
        }

        private async void DownloadSkills()
        {
            if (CompleteIcons)
            {
                this.logger.LogInformation("All icons were detected. Skipping check");
                this.viewManager.ShowView<BuildsListView>();
                return;
            }

            this.logger.LogInformation("Beginning icon download");
            this.iconBrowser.InitializeWebView(this.WebView.WebBrowser, this.cancellationTokenSource.Token);
            var progressIncrement = 100d / Skill.Skills.Count();
            var incomplete = false;
            foreach(var skill in Skill.Skills.OrderBy(s => s.Name))
            {
                if (skill == Skill.NoSkill)
                {
                    this.ProgressValue += progressIncrement;
                    continue;
                }

                var logger = this.logger.CreateScopedLogger(nameof(this.DownloadSkills), skill.Name);
                logger.LogInformation("Verifying if icon exists");
                this.Description = $"Checking [{skill.Name}] icon";
                if ((await this.iconRetriever.GetIconUri(skill)).ExtractValue() is not null)
                {
                    logger.LogInformation("Icon exists");
                    this.ProgressValue += progressIncrement;
                    await Task.Delay(1);
                    continue;
                }

                this.Description = $"Downloading [{skill.Name}] icon";
                logger.LogInformation("Downloading icon");
                var request = new IconRequest { Skill = skill };
                this.iconBrowser.QueueIconRequest(request);

                while(request.Finished is false)
                {
                    await Task.Delay(1000);
                }

                if (request.IconBase64.IsNullOrWhiteSpace())
                {
                    logger.LogWarning("Failed to download icon");
                    incomplete = true;
                }
                else
                {
                    logger.LogInformation("Downloaded icon");
                }

                this.ProgressValue += progressIncrement;
            }

            CompleteIcons = !incomplete;
            this.viewManager.ShowView<BuildsListView>();
        }
    }
}

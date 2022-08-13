using Daybreak.Configuration;
using Daybreak.Controls;
using Daybreak.Models;
using Daybreak.Models.Builds;
using Daybreak.Models.Progress;
using Daybreak.Services.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.IconRetrieve
{
    public sealed class IconDownloader : IIconDownloader, IApplicationLifetimeService
    {
        private readonly IIconBrowser iconBrowser;
        private readonly IIconCache iconCache;
        private readonly IConfigurationManager configurationManager;
        private readonly ILogger<IconDownloader> logger;
        private readonly ILiveOptions<ApplicationConfiguration> liveOptions;
        private readonly ILogger<ChromiumBrowserWrapper> browserLogger;
        private ChromiumBrowserWrapper browserWrapper;
        private CancellationTokenSource cancellationTokenSource;
        private IconDownloadStatus iconDownloadStatus;

        public bool DownloadComplete { get; private set; }
        public bool Downloading => this.cancellationTokenSource is not null;

        public IconDownloader(
            IIconBrowser iconBrowser,
            IIconCache iconCache,
            IConfigurationManager configurationManager,
            ILogger<IconDownloader> logger,
            ILiveOptions<ApplicationConfiguration> liveOptions,
            ILogger<ChromiumBrowserWrapper> browserLogger)
        {
            this.iconBrowser = iconBrowser.ThrowIfNull();
            this.iconCache = iconCache.ThrowIfNull();
            this.configurationManager = configurationManager.ThrowIfNull();
            this.logger = logger.ThrowIfNull();
            this.liveOptions = liveOptions.ThrowIfNull();
            this.browserLogger = browserLogger.ThrowIfNull();
            this.HookIntoConfigurationChanges();
        }

        public void SetBrowser(ChromiumBrowserWrapper chromiumBrowserWrapper)
        {
            if (this.browserWrapper is not null)
            {
                throw new InvalidOperationException("Browser is already set");
            }

            this.browserWrapper = chromiumBrowserWrapper;
        }

        public async Task<IconDownloadStatus> StartIconDownload()
        {
            while(this.browserWrapper is null)
            {
                await Task.Delay(100);
            }

            await this.browserWrapper.InitializeBrowser(this.liveOptions, null, this.browserLogger);

            if (this.browserWrapper.BrowserSupported is false ||
                this.browserWrapper.BrowserEnabled is false)
            {
                this.logger.LogError("Browser is not supported. Cannot download icons");
                if (this.iconDownloadStatus is null)
                {
                    this.iconDownloadStatus = new IconDownloadStatus();
                }

                this.iconDownloadStatus.CurrentStep = IconDownloadStatus.BrowserNotSupported;
                return this.iconDownloadStatus;
            }

            this.logger.LogInformation("Starting download");
            if (this.Downloading)
            {
                this.logger.LogInformation("Download already running");
                return this.iconDownloadStatus;
            }

            this.cancellationTokenSource = new();
            this.iconDownloadStatus = new IconDownloadStatus();
            Task.Run(this.DownloadIcons);
            return this.iconDownloadStatus;
        }

        public void CancelIconDownload()
        {
            this.cancellationTokenSource?.Cancel();
            this.cancellationTokenSource?.Dispose();
            this.cancellationTokenSource = null;
        }

        private async Task DownloadIcons()
        {
            this.logger.LogInformation("Beginning icon download");
            this.iconBrowser.InitializeWebView(this.browserWrapper, this.cancellationTokenSource.Token);
            var progressIncrement = 100d / Skill.Skills.Count();
            var incomplete = false;
            var progressValue = 0d;
            foreach (var skill in Skill.Skills.OrderBy(s => s.Name))
            {
                if (this.cancellationTokenSource?.IsCancellationRequested is null or true)
                {
                    this.iconDownloadStatus.CurrentStep = IconDownloadStatus.Stopped(progressValue);
                    return;
                }

                if (skill == Skill.NoSkill)
                {
                    progressValue += progressIncrement;
                    continue;
                }

                var logger = this.logger.CreateScopedLogger(nameof(this.DownloadIcons), skill.Name);
                logger.LogInformation("Verifying if icon exists");
                this.iconDownloadStatus.CurrentStep = IconDownloadStatus.Checking(skill.Name, progressValue);
                if ((await this.iconCache.GetIconUri(skill)).ExtractValue() is not null)
                {
                    logger.LogInformation("Icon exists");
                    progressValue += progressIncrement;
                    continue;
                }

                logger.LogInformation("Downloading icon");
                this.iconDownloadStatus.CurrentStep = IconDownloadStatus.Downloading(skill.Name, progressValue);
                var request = new IconRequest { Skill = skill };
                this.iconBrowser.QueueIconRequest(request);

                while (request.Finished is false)
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

                progressValue += progressIncrement;
            }

            if (incomplete)
            {
                this.logger.LogError("Failed to download all icons. Retrying");
                await this.DownloadIcons();
            }
            else
            {
                this.DownloadComplete = true;
            }
        }

        private void HookIntoConfigurationChanges()
        {
            this.configurationManager.ConfigurationChanged += async (_, _) =>
            {
                var configuration = this.configurationManager.GetConfiguration();
                if (configuration.ExperimentalFeatures.DownloadIcons)
                {
                    await this.StartIconDownload();
                }
                else
                {
                    this.CancelIconDownload();
                }
            };
        }

        public async void OnStartup()
        {
            var configuration = this.configurationManager.GetConfiguration();
            if (configuration.ExperimentalFeatures.DownloadIcons)
            {
                await this.StartIconDownload();
            }
        }

        public void OnClosing()
        {
            this.cancellationTokenSource?.Cancel();
        }
    }
}

using Daybreak.Configuration;
using Daybreak.Controls;
using Daybreak.Models;
using Daybreak.Models.Builds;
using Daybreak.Models.Progress;
using Daybreak.Services.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

            this.logger.LogInformation("Starting download");
            if (this.Downloading)
            {
                this.logger.LogInformation("Download already running");
                return this.iconDownloadStatus;
            }

            this.cancellationTokenSource = new();
            this.iconDownloadStatus = new IconDownloadStatus();
            _ = Task.Run(this.DownloadIcons);
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
            if (await TestBrowserSupported() is false)
            {
                this.logger.LogError("Browser not supported. Icon downloading stopped");
                this.iconDownloadStatus.CurrentStep = IconDownloadStatus.BrowserNotSupported;
                return;
            }

            var progressIncrement = 100d / Skill.Skills.Count();
            var progressValue = 0d;
            var skillsToDownload = new List<Skill>();
            foreach(var skill in Skill.Skills.OrderBy(s => s.Name))
            {
                if (skill == Skill.NoSkill)
                {
                    continue;
                }

                var logger = this.logger.CreateScopedLogger(nameof(this.DownloadIcons), skill.Name);
                logger.LogInformation("Verifying if icon exists");
                this.iconDownloadStatus.CurrentStep = IconDownloadStatus.Checking(skill.Name, progressValue);
                if ((await this.iconCache.GetIconUri(skill)).ExtractValue() is not null)
                {
                    progressValue += progressIncrement;
                    await Task.Delay(1);
                    continue;
                }

                skillsToDownload.Add(skill);
            }

            if (skillsToDownload.Count == 0)
            {
                this.logger.LogInformation("No icons missing. Stopping download");
                this.iconDownloadStatus.CurrentStep = IconDownloadStatus.Finished;
                return;
            }

            this.iconBrowser.InitializeWebView(this.browserWrapper, this.cancellationTokenSource.Token);
            
            var incomplete = false;
            foreach (var skill in skillsToDownload)
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

                this.logger.LogInformation("Downloading icon");
                this.iconDownloadStatus.CurrentStep = IconDownloadStatus.Downloading(skill.Name, progressValue);
                var request = new IconRequest { Skill = skill };
                this.iconBrowser.QueueIconRequest(request);

                while (request.Finished is false)
                {
                    await Task.Delay(1000);
                }

                if (request.IconBase64.IsNullOrWhiteSpace())
                {
                    this.logger.LogWarning("Failed to download icon");
                    incomplete = true;
                }
                else
                {
                    this.logger.LogInformation("Downloaded icon");
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.browserWrapper.IsEnabled = false;
                    this.browserWrapper.WebBrowser.Dispose();
                });
                this.iconDownloadStatus.CurrentStep = IconDownloadStatus.Finished;
                this.DownloadComplete = true;
            }
        }

        private static async Task<bool> TestBrowserSupported()
        {
            CoreWebView2Environment coreWebView2Environment;
            try
            {
                var task = await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    return await CoreWebView2Environment.CreateAsync().ConfigureAwait(true);
                });

                coreWebView2Environment = await task;
            }
            catch (Exception)
            {
                return false;
            }

            return coreWebView2Environment is not null;
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

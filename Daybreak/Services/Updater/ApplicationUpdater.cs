using Daybreak.Models;
using Daybreak.Services.Logging;
using Daybreak.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Daybreak.Services.Updater
{
    public sealed class ApplicationUpdater : IApplicationUpdater
    {
        private const string ExtractAndRunPs1 = "ExtractAndRun.ps1";
        private const string TempFile = "tempfile.zip";
        private const string VersionTag = "{VERSION}";
        private const string InputFileTag = "{INPUTFILE}";
        private const string OutputPathTag = "{OUTPUTPATh}";
        private const string Url = "https://github.com/AlexMacocian/Daybreak/releases/latest";
        private const string DownloadUrl = $"https://github.com/AlexMacocian/Daybreak/releases/download/v{VersionTag}/Daybreakv{VersionTag}.zip";
        private const string DelayCommand = "Start-Sleep -m 3000";
        private const string ExtractCommandTemplate = $"Expand-Archive -Path '{InputFileTag}' -DestinationPath '{OutputPathTag}' -Force";
        private const string RunClientCommand = @".\Daybreak.exe";
        private const string RemoveTempFile = $"Remove-item {TempFile}";
        private const string RemovePs1 = $"Remove-item {ExtractAndRunPs1}";

        private readonly ILogger logger;
        private readonly HttpClient httpClient = new();

        public string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public ApplicationUpdater(ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
        }

        public async Task<bool> DownloadUpdate(UpdateStatus updateStatus)
        {
            updateStatus.CurrentStep = UpdateStatus.CheckingLatestVersion;
            var latestVersion = (await this.GetLatestVersion()).ExtractValue();
            if (latestVersion is null)
            {
                this.logger.LogWarning("Failed to retrieve latest version. Aborting update");
                return false;
            }

            using var downloadLatestResponse = await this.httpClient.GetAsync(
                DownloadUrl.Replace(VersionTag, latestVersion));

            if (downloadLatestResponse.IsSuccessStatusCode is false)
            {
                this.logger.LogWarning("Failed to download latest version. Aborting udpate");
                return false;
            }

            this.logger.LogInformation("Beginning update download");
            var downloadStream = await downloadLatestResponse.Content.ReadAsStreamAsync();
            var fileStream = File.OpenWrite(TempFile);
            var downloadSize = (double)downloadStream.Length;
            var buffer = new byte[1024];
            var length = 0;
            double downloaded = 0;
            var tickTime = DateTime.Now;
            while (downloadStream.CanRead && (length = await downloadStream.ReadAsync(buffer)) > 0)
            {
                downloaded += length;
                await fileStream.WriteAsync(buffer, 0, length);
                if ((DateTime.Now - tickTime).TotalMilliseconds > 50)
                {
                    tickTime = DateTime.Now;
                    updateStatus.CurrentStep = UpdateStatus.Downloading(downloaded / downloadSize);
                }
            }

            updateStatus.CurrentStep = UpdateStatus.Downloading(1);
            updateStatus.CurrentStep = UpdateStatus.DownloadFinished;
            fileStream.Close();
            this.logger.LogInformation("Downloaded update");
            return true;
        }

        public async Task<bool> UpdateAvailable()
        {
            var version = string.Join('.', this.CurrentVersion.Split('.').Take(3));
            var maybeLatestVersion = await this.GetLatestVersion();
            return maybeLatestVersion.Switch(
                onSome: latestVersion => string.Compare(version, latestVersion, true) < 0,
                onNone: () => 
                {
                    this.logger.LogWarning("Failed to retrieve latest version");
                    return false;
                }).ExtractValue();
        }

        public void FinalizeUpdate()
        {
            File.WriteAllLines(ExtractAndRunPs1, new List<string>()
            { 
                DelayCommand,
                ExtractCommandTemplate
                    .Replace(InputFileTag, Path.GetFullPath(TempFile))
                    .Replace(OutputPathTag, Directory.GetCurrentDirectory()),
                RemoveTempFile,
                RemovePs1,
                RunClientCommand
            });
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $@"{Directory.GetCurrentDirectory()}\{ExtractAndRunPs1}",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Maximized,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                },
            };
            if (process.Start() is false)
            {
                throw new InvalidOperationException("Failed to create and start powershell script");
            }
        }

        private async Task<Optional<string>> GetLatestVersion()
        {
            using var response = await this.httpClient.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                var versionTag = response.RequestMessage.RequestUri.ToString().Split('/').Last().TrimStart('v');
                return versionTag;
            }

            return Optional.None<string>();
        }
    }
}

using Daybreak.Models.Progress;
using Daybreak.Services.Privilege;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Daybreak.Services.Downloads;

public sealed class DownloadService : IDownloadService
{
    private const string DownloadUri = "https://cloudfront.guildwars2.com/client/GwSetup.exe";
    private const string InstallationFileName = "GwSetup.exe";

    private readonly IPrivilegeManager privilegeManager;
    private readonly IHttpClient<DownloadService> httpClient;
    private readonly ILogger<DownloadService> logger;

    public DownloadService(
        IPrivilegeManager privilegeManager,
        IHttpClient<DownloadService> httpClient,
        ILogger<DownloadService> logger)
    {
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<bool> DownloadGuildwars(string destinationPath, DownloadStatus downloadStatus)
    {
        if (this.privilegeManager.AdminPrivileges is false)
        {
            this.privilegeManager.RequestAdminPrivileges<LauncherView>("Daybreak needs admin privileges to download and install Guildwars");
            return false;
        }

        var exePath = Path.Combine(destinationPath, InstallationFileName);

        downloadStatus.CurrentStep = DownloadStatus.InitializingDownload;
        using var response = await this.httpClient.GetAsync(DownloadUri, HttpCompletionOption.ResponseHeadersRead);
        if (response.IsSuccessStatusCode is false)
        {
            downloadStatus.CurrentStep = DownloadStatus.FailedDownload;
            this.logger.LogError($"Failed to download installer. Details: {await response.Content.ReadAsStringAsync()}");
            return false;
        }

        using var downloadStream = await this.httpClient.GetStreamAsync(DownloadUri);
        this.logger.LogInformation("Beginning download");
        var fileStream = File.OpenWrite(exePath);
        var downloadSize = (double)response.Content!.Headers!.ContentLength!;
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
                downloadStatus.CurrentStep = DownloadStatus.Downloading(downloaded / downloadSize);
            }
        }

        downloadStatus.CurrentStep = DownloadStatus.Downloading(1);
        downloadStatus.CurrentStep = DownloadStatus.DownloadFinished;
        fileStream.Close();
        this.logger.LogInformation("Downloaded update");

        var installationProcess = Process.Start(exePath);
        while(installationProcess.HasExited is false)
        {
            await Task.Delay(1000);
        }

        downloadStatus.CurrentStep = DownloadStatus.Finished;
        this.logger.LogInformation($"Installation finished with status code {installationProcess.ExitCode}");
        return true;
    }
}

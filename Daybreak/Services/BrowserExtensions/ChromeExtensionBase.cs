using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace Daybreak.Services.BrowserExtensions;
public abstract class ChromeExtensionBase<TExtension> : IBrowserExtension
    where TExtension : IBrowserExtension
{
    private const string ExtensionDownloadLink = "https://clients2.google.com/service/update2/crx?response=redirect&prodversion=[BROWSER_VERSION]&x=id%3D[EXTENSION_ID]%26installsource%3Dondemand%26uc";
    private const string BrowserVersionPlaceholder = "[BROWSER_VERSION]";
    private const string ExtensionIdPlaceholder = "[EXTENSION_ID]";

    private readonly IDownloadService downloadService;
    private readonly ILogger<TExtension> logger;

    public abstract string ExtensionId { get; }
    public abstract string ExtensionName { get; }
    public virtual string InstallationPath { get; } = "BrowserExtensions";

    public ChromeExtensionBase(
        IDownloadService downloadService,
        ILogger<TExtension> logger)
    {
        this.downloadService = downloadService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task CheckAndUpdate(string browserVersion)
    {
        var installationPath = Path.Combine(
            Path.GetFullPath(this.InstallationPath),
            this.ExtensionName);
        var downloadFilePath = Path.Combine(
            Path.GetFullPath(this.InstallationPath),
            $"{this.ExtensionName}.crx");
        var downloadUrl = ExtensionDownloadLink.Replace(BrowserVersionPlaceholder, browserVersion).Replace(ExtensionIdPlaceholder, this.ExtensionId);
        await this.downloadService.DownloadFile(downloadUrl, downloadFilePath, new UpdateStatus());

    }

    public Task<string> GetExtensionPath()
    {
        return Task.FromResult(Path.Combine(
            Path.GetFullPath(this.InstallationPath),
            this.ExtensionName));
    }
}

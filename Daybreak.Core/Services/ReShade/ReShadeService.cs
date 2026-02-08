using Daybreak.Configuration.Options;
using Daybreak.Services.Downloads;
using Daybreak.Utils;
using Daybreak.Services.ReShade.Utils;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Models.ReShade;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.ReShade;
using Daybreak.Shared.Utils;
using Daybreak.Views.Mods;
using HtmlAgilityPack;
using IniParser.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Data;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.IO.Compression;
using TrailBlazr.Services;

namespace Daybreak.Services.ReShade;
internal sealed class ReShadeService(
    IOptionsProvider optionsProvider,
    INotificationService notificationService,
    IProcessInjector processInjector,
    IOptionsMonitor<ReShadeOptions> liveUpdateableOptions,
    IHttpClient<ReShadeService> httpClient,
    IDownloadService downloadService,
    IViewManager viewManager,
    ILogger<ReShadeService> logger) : IReShadeService
{
    private const string PackagesIniUrl = "https://raw.githubusercontent.com/crosire/reshade-shaders/list/EffectPackages.ini";
    private const string ReShadeHomepageUrl = "https://reshade.me";
    private const string ReShadeSubPath = "ReShade";
    private const string ReShadeInstallerName = "ReShade_Setup.exe";
    private const string DllName = "ReShade32.dll";
    private const string ConfigIni = "ReShade.ini";
    private const string ReShadePreset = "ReShadePreset.ini";
    private const string ReShadeLog = "ReShade.log";
    private const string PresetsFolder = "reshade-shaders";

    private static readonly ProgressUpdate ProgressRetrieveLatestUrl = new(0, "Retrieving latest ReShade version");
    private static readonly ProgressUpdate ProgressInstalling = new(1, "Installing ReShade files");
    private static readonly ProgressUpdate ProgressFailed = new(1, "Failed to install ReShade");
    private static readonly ProgressUpdate ProgressFinished = new(1, "Finished ReShade installation");

    private static readonly string ReShadePath = PathUtils.GetAbsolutePathFromRoot(ReShadeSubPath);
    private static readonly string ReShadeDllPath = Path.Combine(ReShadePath, DllName);
    private static readonly string ConfigIniPath = Path.Combine(ReShadePath, ConfigIni);
    private static readonly string ReShadePresetPath = Path.Combine(ReShadePath, ReShadePreset);
    private static readonly string ReShadeLogPath = Path.Combine(ReShadePath, ReShadeLog);
    private static readonly string SourcePresetsFolderPath = Path.Combine(ReShadePath, PresetsFolder);
    private static readonly string[] TextureExtensions = [".png", ".jpg", ".jpeg"];
    private static readonly string[] FxExtensions = [".fx",];
    private static readonly string[] FxHeaderExtensions = [".fxh"];

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IProcessInjector processInjector = processInjector.ThrowIfNull();
    private readonly IOptionsMonitor<ReShadeOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly IHttpClient<ReShadeService> httpClient = httpClient.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly ILogger<ReShadeService> logger = logger.ThrowIfNull();

    public string Name => "ReShade";
    public string Description => "ReShade is an advanced, fully generic post-processing injector for games and video software developed by crosire.";
    public bool IsVisible => true;
    public bool CanCustomManage => true;
    public bool IsEnabled
    {
        get => this.liveUpdateableOptions.CurrentValue.Enabled;
        set
        {
            var options = this.liveUpdateableOptions.CurrentValue;
            options.Enabled = value;
            this.optionsProvider.SaveOption(options);
        }
    }
    public bool AutoUpdate
    {
        get => this.liveUpdateableOptions.CurrentValue.AutoUpdate;
        set
        {
            var options = this.liveUpdateableOptions.CurrentValue;
            options.AutoUpdate = value;
            this.optionsProvider.SaveOption(options);
        }
    }
    public bool IsInstalled => File.Exists(ReShadeDllPath) &&
                               File.Exists(ReShadePresetPath) &&
                               File.Exists(ReShadeLogPath) &&
                               File.Exists(ConfigIniPath);
    public bool CanUninstall => true;

    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create<bool>(progress =>
        {
            progress.Report(new ProgressUpdate(0, "Uninstalling ReShade"));
            if (!this.IsInstalled)
            {
                progress.Report(new ProgressUpdate(1, "ReShade is not installed"));
                return Task.FromResult(true);
            }

            if (!Directory.Exists(ReShadePath))
            {
                progress.Report(new ProgressUpdate(1, "ReShade is not installed"));
                return Task.FromResult(true);
            }

            Directory.Delete(ReShadePath, true);
            progress.Report(new ProgressUpdate(1, "ReShade uninstalled successfully"));
            return Task.FromResult(true);
        }, cancellationToken);
    }

    public async Task<bool> IsUpdateAvailable(CancellationToken cancellationToken)
    {
        if (this.GetCurrentVersion() is not Version currentVersion)
        {
            return false;
        }

        if (await this.GetLatestVersion(cancellationToken) is not Version latestVersion)
        {
            return false;
        }

        return latestVersion.CompareTo(currentVersion) > 0;
    }

    public async Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        return await this.PerformInstallation(cancellationToken);
    }

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            return await this.SetupReShade(progress, cancellationToken);
        }, cancellationToken);
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        this.viewManager.ShowView<ReShadeManagementView>();
        return Task.CompletedTask;
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: guildWarsCreatedContext.ApplicationLauncherContext.ExecutablePath ?? string.Empty);
        if (await this.processInjector.Inject(guildWarsCreatedContext.ApplicationLauncherContext.Process, ReShadeDllPath, cancellationToken))
        {
            scopedLogger.LogDebug("Injected ReShade dll");
            this.notificationService.NotifyInformation(
                title: "ReShade started",
                description: "ReShade has been injected");
        }
        else
        {
            scopedLogger.LogError("Failed to inject ReShade dll");
            this.notificationService.NotifyError(
                title: "ReShade failed to start",
                description: "Failed to inject ReShade");
        }
    }

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        var destinationDirectory = Path.GetFullPath(new FileInfo(guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath).DirectoryName!);
        EnsureFileExistsInGuildwarsDirectory(ReShadeLog, destinationDirectory);
        EnsureFileExistsInGuildwarsDirectory(ReShadePreset, destinationDirectory);
        EnsureFileExistsInGuildwarsDirectory(ConfigIni, destinationDirectory);
        Directory.CreateDirectory(SourcePresetsFolderPath);
        EnsureSymbolicLinkExists(destinationDirectory);
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
    {
        var destinationDirectory = Path.GetFullPath(new FileInfo(guildWarsStartingDisabledContext.ApplicationLauncherContext.ExecutablePath).DirectoryName!);
        var destination = Path.Combine(Path.GetFullPath(destinationDirectory), PresetsFolder);
        if (Directory.Exists(destination))
        {
            Directory.Delete(destination);
        }

        return Task.CompletedTask;
    }

    public void OpenReShadeFolder()
    {
        Process.Start("explorer.exe", ReShadePath);
    }

    public async Task<IEnumerable<ShaderPackage>> GetStockPackages(CancellationToken cancellationToken)
    {
        var response = await this.httpClient.GetAsync(PackagesIniUrl, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Unable to retrieve stock packages");
        }

        var iniContents = await response.Content.ReadAsStringAsync(cancellationToken);
        var parser = new IniDataParser();
        var config = parser.Parse(iniContents);
        return config.Sections.Select(section =>
        {
            return new ShaderPackage
            {
                Enabled = section.Keys.FirstOrDefault(k => k.KeyName == "Enabled")?.Value == "1",
                Required = section.Keys.FirstOrDefault(k => k.KeyName == "Required")?.Value == "1",
                Name = section.Keys.FirstOrDefault(k => k.KeyName == "PackageName")?.Value,
                Description = section.Keys.FirstOrDefault(k => k.KeyName == "PackageDescription")?.Value,
                InstallPath = section.Keys.FirstOrDefault(k => k.KeyName == "InstallPath")?.Value,
                TextureInstallPath = section.Keys.FirstOrDefault(k => k.KeyName == "TextureInstallPath")?.Value,
                DownloadUrl = section.Keys.FirstOrDefault(k => k.KeyName == "DownloadUrl")?.Value,
                RepositoryUrl = section.Keys.FirstOrDefault(k => k.KeyName == "RepositoryUrl")?.Value,
                EffectFiles = section.Keys.FirstOrDefault(k => k.KeyName == "EffectFiles")?.Value?.Split(',').ToList()
            };
        });
    }

    public async Task<bool> SetupReShade(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Retrieving ReShade latest version");

        progress.Report(ProgressRetrieveLatestUrl);
        var downloadUrl = await this.GetLatestDownloadUrl(cancellationToken);

        if (downloadUrl is null)
        {
            progress.Report(ProgressFailed);
            scopedLogger.LogError("Unable to retrieve latest version url");
            return false;
        }

        scopedLogger.LogDebug($"Downloading installer from {downloadUrl}");
        var destinationPath = Path.Combine(Path.GetFullPath(ReShadePath), ReShadeInstallerName);
        var downloadResult = await this.downloadService.DownloadFile(downloadUrl, destinationPath, progress, cancellationToken);
        if (!downloadResult)
        {
            scopedLogger.LogError($"Failed to download {downloadUrl}. Check {nameof(DownloadService)} logs for errors");
            return false;
        }

        progress.Report(ProgressInstalling);
        var setupResult = await this.SetupReshadeDllFromInstaller(destinationPath, cancellationToken);
        if (!setupResult)
        {
            scopedLogger.LogError($"Failed to setup reshade dll from installer {destinationPath}");
            return false;
        }

        var versionString = downloadUrl.Replace(ReShadeHomepageUrl + "/downloads/ReShade_Setup_", "").Replace(".exe", "");
        File.Delete(destinationPath);
        progress.Report(ProgressFinished);
        return true;
    }

    public async Task<bool> InstallPackage(ShaderPackage package, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: package.Name ?? string.Empty);
        if (package.DownloadUrl!.IsNullOrWhiteSpace() is not false)
        {
            scopedLogger.LogError("Unable to install. Package has no download url");
            return false;
        }

        using var result = await this.httpClient.GetAsync(package.DownloadUrl!, cancellationToken);
        if (!result.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Unable to install. Download returned status code {result.StatusCode}");
            return false;
        }

        using var archive = new ZipArchive(result.Content.ReadAsStream(cancellationToken));
        if (archive is null)
        {
            scopedLogger.LogError("Unable to open archive");
            return false;
        }

        await this.InstallPackageInternal(archive, cancellationToken, package.InstallPath, package.TextureInstallPath, package.EffectFiles);
        this.notificationService.NotifyInformation(
            "ReShade package installed",
            $"Installation of {package.Name} has been successful");
        return true;
    }

    public async Task<bool> InstallPackage(string pathToZip, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: pathToZip ?? string.Empty);
        var fullPath = pathToZip is not null ? Path.GetFullPath(pathToZip) : default;
        if (fullPath?.IsNullOrWhiteSpace() is not false)
        {
            scopedLogger.LogError("Unable to install. Path is null");
            return false;
        }

        if (!File.Exists(fullPath))
        {
            scopedLogger.LogError("Unable to install. File does not exist");
            return false;
        }

        using var archive = ZipFile.OpenRead(fullPath);
        if (archive is null)
        {
            scopedLogger.LogError($"Unable to open archive");
            return false;
        }

        await this.InstallPackageInternal(archive, cancellationToken);
        this.notificationService.NotifyInformation(
            "ReShade package installed",
            $"Installation of {pathToZip} has been successful");
        return true;
    }

    public async Task<string> GetConfig(CancellationToken cancellationToken)
    {
        return await File.ReadAllTextAsync(ConfigIniPath, cancellationToken);
    }

    public async Task<bool> SaveConfig(string config, CancellationToken cancellationToken)
    {
        await File.WriteAllTextAsync(ConfigIniPath, config, cancellationToken);
        this.notificationService.NotifyInformation(
            "ReShade config saved",
            "Saved changes to ReShade config");
        return true;
    }

    public async Task<string> GetPreset(CancellationToken cancellationToken)
    {
        return await File.ReadAllTextAsync(ReShadePresetPath, cancellationToken);
    }

    public async Task<bool> SavePreset(string config, CancellationToken cancellationToken)
    {
        await File.WriteAllTextAsync(ReShadePresetPath, config, cancellationToken);
        this.notificationService.NotifyInformation(
            "ReShade preset saved",
            "Saved changes to ReShade preset");
        return true;
    }

    public async Task<bool> UpdateIniFromPath(string pathToIni, CancellationToken cancellationToken)
    {
        pathToIni = Path.GetFullPath(pathToIni);
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: pathToIni);
        if (!File.Exists(pathToIni))
        {
            scopedLogger.LogError("File doesn't exist");
            return false;
        }

        var config = await File.ReadAllTextAsync(pathToIni, cancellationToken);
        if (pathToIni.Contains(ConfigIni))
        {
            await this.SaveConfig(config, cancellationToken);
        }

        if (pathToIni.Contains(ReShadePreset))
        {
            await this.SavePreset(config, cancellationToken);
        }

        return true;
    }

    public async Task CheckUpdates(CancellationToken cancellationToken)
    {
        if (!this.IsInstalled)
        {
            return;
        }

        if (!this.liveUpdateableOptions.CurrentValue.AutoUpdate)
        {
            return;
        }

        var scopedLogger = this.logger.CreateScopedLogger();
        var version = await this.GetLatestVersion(cancellationToken);
        if (this.GetCurrentVersion() is not Version currentVersion ||
            currentVersion.CompareTo(version) >= 0)
        {
            scopedLogger.LogDebug("No update available. Current version is up to date");
            return;
        }

        scopedLogger.LogDebug($"Found an update for ReShade. Latest version: {version}");
        var notificationToken = this.notificationService.NotifyInformation(
                "Updating ReShade",
                $"Updating ReShade to version {version}");

        if (await this.SetupReShade(new Progress<ProgressUpdate>(), cancellationToken))
        {
            notificationToken.Cancel();
            scopedLogger.LogDebug($"ReShade updated to version {version}");
            this.notificationService.NotifyInformation(
                "ReShade updated",
                $"ReShade has been updated to version {version}");
            return;
        }

        notificationToken.Cancel();
        scopedLogger.LogError("Failed to update ReShade");
        this.notificationService.NotifyInformation(
                "Failed to update ReShade",
                $"Could not update ReShade to version {version}. Check logs for details");
    }

    private async Task InstallPackageInternal(
        ZipArchive archive,
        CancellationToken cancellationToken,
        string? desiredInstallationPath = default,
        string? desiredTextureInstallationPath = default,
        List<string>? fxFilter = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var basePath = Path.GetFullPath(ReShadePath);
        desiredInstallationPath = desiredInstallationPath?.Replace(".\\", basePath + "\\");
        desiredTextureInstallationPath = desiredTextureInstallationPath?.Replace(".\\", basePath + "\\");
        foreach (var entry in archive.Entries)
        {
            if (FxExtensions.Any(entry.FullName.EndsWith))
            {
                scopedLogger.LogDebug($"[{entry.FullName}] Processing fx file");
                var fileName = StripAfterToken(entry.FullName, "shaders/");
                if (fxFilter is not null &&
                    !fxFilter.Contains(fileName))
                {
                    scopedLogger.LogDebug($"[{entry.FullName}] File not found in effects list. Skipping");
                    continue;
                }

                var destination = Path.Combine(basePath, PresetsFolder, "Shaders", fileName);
                if (desiredInstallationPath?.IsNullOrWhiteSpace() is false)
                {
                    destination = Path.Combine(desiredInstallationPath, fileName);
                }

                scopedLogger.LogDebug($"[{entry.FullName}] Installing fx at {destination}");
                new FileInfo(destination).Directory?.Create();
                using var fs = new FileStream(destination, FileMode.Create);
                using var reader = entry.Open();
                await reader.CopyToAsync(fs, cancellationToken);

                scopedLogger.LogDebug($"[{entry.FullName}] Installed fx at {destination}");
            }
            else if (FxHeaderExtensions.Any(entry.FullName.EndsWith))
            {
                scopedLogger.LogDebug($"[{entry.FullName}] Processing fxh file");
                var fileName = StripAfterToken(entry.FullName, "shaders/");
                var destination = Path.Combine(basePath, PresetsFolder, "Shaders", fileName);
                if (desiredInstallationPath?.IsNullOrWhiteSpace() is false)
                {
                    destination = Path.Combine(desiredInstallationPath, fileName);
                }

                scopedLogger.LogDebug($"[{entry.FullName}] Installing fxh at {destination}");
                new FileInfo(destination).Directory?.Create();
                using var fs = new FileStream(destination, FileMode.Create);
                using var reader = entry.Open();
                await reader.CopyToAsync(fs, cancellationToken);

                scopedLogger.LogDebug($"[{entry.FullName}] Installed fxh at {destination}");
            }
            else if (TextureExtensions.Any(entry.FullName.EndsWith))
            {
                scopedLogger.LogDebug($"Processing texture file {entry.FullName}");
                var fileName = StripAfterToken(entry.FullName, "Textures/");
                var destination = Path.Combine(basePath, PresetsFolder, "Textures", fileName);
                if (desiredTextureInstallationPath?.IsNullOrWhiteSpace() is false)
                {
                    destination = Path.Combine(desiredTextureInstallationPath, fileName);
                }

                scopedLogger.LogDebug($"[{entry.FullName}] Installing texture at {destination}");
                new FileInfo(destination).Directory?.Create();
                using var fs = new FileStream(destination, FileMode.Create);
                using var reader = entry.Open();
                await reader.CopyToAsync(fs, cancellationToken);

                scopedLogger.LogDebug($"[{entry.FullName}] Installed texture at {destination}");
            }
        }
    }

    private async Task<string?> GetLatestDownloadUrl(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var homePageResult = await this.httpClient.GetAsync(ReShadeHomepageUrl, cancellationToken);
        if (!homePageResult.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received unsuccessful status code {homePageResult.StatusCode} from {ReShadeHomepageUrl}");
            return default;
        }

        scopedLogger.LogDebug("Scrapping latest download url");
        var html = await homePageResult.Content.ReadAsStringAsync(cancellationToken);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        if (doc.DocumentNode.Descendants("a")
            .Select(node =>
            {
                if (node.GetAttributeValue<string>("href", string.Empty) is string href &&
                    href.Contains("/downloads/", StringComparison.OrdinalIgnoreCase))
                {
                    return href;
                }

                return null;
            }).FirstOrDefault(href => href?.IsNullOrWhiteSpace() is false) is not string href)
        {
            scopedLogger.LogWarning("Unable to find the download url on the ReShade homepage");
            return default;
        }

        var downloadUrl = ReShadeHomepageUrl + href;
        return downloadUrl;
    }

    private async Task<bool> SetupReshadeDllFromInstaller(string pathToInstaller, CancellationToken cancellationToken)
    {
        var scoppedLogger = this.logger.CreateScopedLogger(flowIdentifier: pathToInstaller);
        try
        {
            using var fs = File.OpenRead(pathToInstaller);
            await SeekZipStart(fs, cancellationToken);
            using var zipStream = new OffsetStream(fs);
            using var zip = new ZipArchive(zipStream);

            // Validate archive contains the ReShade DLLs
            if (zip.Entries.FirstOrDefault(e => e.FullName == DllName) is not ZipArchiveEntry entry)
            {
                throw new InvalidOperationException("File does not contain desired ReShade dll");
            }

            var reshadePath = Path.GetFullPath(ReShadePath);
            if (!Directory.Exists(reshadePath))
            {
                Directory.CreateDirectory(reshadePath);
            }

            var dllDestination = Path.Combine(reshadePath, DllName);
            using var reader = entry.Open();
            using var output = File.OpenWrite(dllDestination);
            await reader.CopyToAsync(output, cancellationToken);
            SetupAdditionalFiles();
            return true;
        }
        catch (Exception e)
        {
            scoppedLogger.LogError(e, "Encountered exception while extracting ReShade dll from installer");
            throw;
        }
    }

    private static void SetupAdditionalFiles()
    {
        if (!File.Exists(ReShadePresetPath))
        {
            File.WriteAllText(ReShadePresetPath, ReShadeFileDefaultContents.ReShadePresetsIni);
        }

        if (!File.Exists(ReShadeLogPath))
        {
            File.WriteAllText(ReShadeLogPath, ReShadeFileDefaultContents.ReShadeLog);
        }

        if (!File.Exists(ConfigIniPath))
        {
            File.WriteAllText(ConfigIniPath, ReShadeFileDefaultContents.ReShadeIni);
        }
    }

    private static void EnsureFileExistsInGuildwarsDirectory(string fileName, string destinationDirectoryName)
    {
        var sourcePath = Path.Combine(ReShadePath, fileName);
        var destinationPath = Path.Combine(destinationDirectoryName, fileName);
        File.Copy(sourcePath, destinationPath, true);
    }

    private static void EnsureSymbolicLinkExists(string destinationPath)
    {
        var destination = Path.Combine(Path.GetFullPath(destinationPath), PresetsFolder);
        if (Directory.Exists(destination))
        {
            Directory.Delete(destination, true);
        }

        Directory.CreateSymbolicLink(destination, SourcePresetsFolderPath);
    }
    
    private async Task<Version?> GetLatestVersion(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var downloadUrl = await this.GetLatestDownloadUrl(cancellationToken);
        if (downloadUrl?.IsNullOrWhiteSpace() is not false)
        {
            scopedLogger.LogError("Unable to retrieve latest download url");
            return default;
        }

        var versionString = downloadUrl.Replace(ReShadeHomepageUrl + "/downloads/ReShade_Setup_", "").Replace(".exe", "");
        if (!Version.TryParse(versionString, out var version))
        {
            scopedLogger.LogError("Unable to parse latest version");
            return default;
        }

        return version;
    }

    private Version? GetCurrentVersion()
    {
        if (File.Exists(ReShadeDllPath) &&
            Version.TryParse(PeVersionReader.GetProductVersion(ReShadeDllPath), out var currentVersion))
        {
            return currentVersion;
        }

        return default;
    }

    /// <summary>
    /// This method helps find the starting point of the SFX/Zip. Self-extracting zip files contain
    /// an executable stub at the beginning of the file and append the zip contents at the end.
    /// Because of this, many Zip libraries fail to parse the files.
    /// The solution is to find the header of the zip file by its signature and drop the exe stub,
    /// before attempting to un-zip the file.
    /// 
    /// Offset calculation is based on this:
    /// https://github.com/crosire/reshade/blob/304ae76174dd9a390d4c4410d3e2309b550d6f72/setup/MainWindow.xaml.cs#L792
    /// </summary>
    private static async Task SeekZipStart(FileStream fs, CancellationToken cancellationToken)
    {
        var buffer = new byte[512];
        var zipSignature = new byte[] { 0x50, 0x4B, 0x03, 0x04 }; // This is the ZIP Local File Header signature

        while(await fs.ReadAsync(buffer.AsMemory(0, 512), cancellationToken) >= zipSignature.Length)
        {
            if (buffer.Take(zipSignature.Length).SequenceEqual(zipSignature) && buffer.Skip(zipSignature.Length).Take(26).Max() > 0)
            {
                fs.Position -= 512;
                return;
            }
        }

        throw new InvalidOperationException("Unable to find starting position of zip file");
    }

    private static string StripAfterToken(string s, string token)
    {
        var indexOfToken = s.IndexOf(token, StringComparison.OrdinalIgnoreCase);
        return s[(indexOfToken + token.Length)..];
    }
}

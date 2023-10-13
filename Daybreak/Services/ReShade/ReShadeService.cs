using Daybreak.Configuration.Options;
using Daybreak.Models.Progress;
using Daybreak.Models.ReShade;
using Daybreak.Services.Downloads;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using Daybreak.Services.ReShade.Utils;
using Daybreak.Services.Scanner;
using HtmlAgilityPack;
using IniParser.Parser;
using Ionic.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.ReShade;
public sealed class ReShadeService : IReShadeService, IApplicationLifetimeService
{
    private const string PackagesIniUrl = "https://raw.githubusercontent.com/crosire/reshade-shaders/list/EffectPackages.ini";
    private const string ReShadeHomepageUrl = "https://reshade.me";
    private const string ReShadePath = "ReShade";
    private const string ReShadeInstallerName = "ReShade_Setup.exe";
    private const string DllName = "ReShade32.dll";
    private const string ConfigIni = "ReShade.ini";
    private const string ReShadePreset = "ReShadePreset.ini";
    private const string ReShadeLog = "ReShade.log";
    private const string PresetsFolder = "reshade-shaders";

    private static readonly string ReShadeDllPath = Path.Combine(Path.GetFullPath(ReShadePath), DllName);
    private static readonly string ConfigIniPath = Path.Combine(Path.GetFullPath(ReShadePath), ConfigIni);
    private static readonly string ReShadePresetPath = Path.Combine(Path.GetFullPath(ReShadePath), ReShadePreset);
    private static readonly string ReShadeLogPath = Path.Combine(Path.GetFullPath(ReShadePath), ReShadeLog);
    private static readonly string SourcePresetsFolderPath = Path.Combine(Path.GetFullPath(ReShadePath), PresetsFolder);
    private static readonly string[] TextureExtensions = new string[] { ".png", ".jpg", ".jpeg" };
    private static readonly string[] FxExtensions = new string[] { ".fx", };
    private static readonly string[] FxHeaderExtensions = new string[] { ".fxh" };

    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly INotificationService notificationService;
    private readonly IProcessInjector processInjector;
    private readonly ILiveUpdateableOptions<ReShadeOptions> liveUpdateableOptions;
    private readonly IHttpClient<ReShadeService> httpClient;
    private readonly IDownloadService downloadService;
    private readonly ILogger<ReShadeService> logger;

    public string Name => "ReShade";
    public bool IsEnabled
    {
        get => this.liveUpdateableOptions.Value.Enabled;
        set
        {
            this.liveUpdateableOptions.Value.Enabled = value;
            this.liveUpdateableOptions.UpdateOption();
        }
    }
    public bool AutoUpdate
    {
        get => this.liveUpdateableOptions.Value.AutoUpdate;
        set
        {
            this.liveUpdateableOptions.Value.AutoUpdate = value;
            this.liveUpdateableOptions.UpdateOption();
        }
    }
    public bool IsInstalled => File.Exists(ReShadeDllPath) &&
                               File.Exists(ReShadePresetPath) &&
                               File.Exists(ReShadeLogPath) &&
                               File.Exists(ConfigIniPath);

    public ReShadeService(
        IGuildwarsMemoryCache guildwarsMemoryCache,
        INotificationService notificationService,
        IProcessInjector processInjector,
        ILiveUpdateableOptions<ReShadeOptions> liveUpdateableOptions,
        IHttpClient<ReShadeService> httpClient,
        IDownloadService downloadService,
        ILogger<ReShadeService> logger)
    {
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.processInjector = processInjector.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async void OnStartup()
    {
        while (true)
        {
            try
            {
                await this.CheckUpdates();
            }
            catch(Exception e)
            {
                this.logger.LogError(e, "Encountered exception while checking for updates");
            }

            await Task.Delay(TimeSpan.FromMinutes(60));
        }
    }

    public void OnClosing()
    {
    }

    public IEnumerable<string> GetCustomArguments() => Enumerable.Empty<string>();

    public Task OnGuildWarsCreated(Process process, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var scopedLogger = this.logger.CreateScopedLogger(nameof(this.OnGuildWarsCreated), process?.MainModule?.FileName ?? string.Empty);
            if (this.processInjector.Inject(process!, ReShadeDllPath))
            {
                scopedLogger.LogInformation("Injected ReShade dll");
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
        }, cancellationToken);
    }

    public Task OnGuildwarsStarted(Process process, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildwarsStarting(Process process, CancellationToken cancellationToken)
    {
        var destinationDirectory = Path.GetFullPath(new FileInfo(process.StartInfo.FileName).DirectoryName!);
        EnsureFileExistsInGuildwarsDirectory(ReShadeLog, destinationDirectory);
        EnsureFileExistsInGuildwarsDirectory(ReShadePreset, destinationDirectory);
        EnsureFileExistsInGuildwarsDirectory(ConfigIni, destinationDirectory);
        EnsureSymbolicLinkExists(destinationDirectory);
        return Task.CompletedTask;
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

    public async Task<bool> LoadReShadeFromDisk(CancellationToken cancellationToken)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "ReShade Installer (ReShade_Setup_*.exe)|ReShade_Setup_*.exe",
            Multiselect = false
        };

        if (dialog.ShowDialog() is not true)
        {
            return false;
        }

        var selectedPath = dialog.FileName;
        return await this.SetupReshadeDllFromInstaller(selectedPath, cancellationToken);
    }

    public async Task<bool> SetupReShade(ReShadeInstallationStatus reShadeInstallationStatus, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.SetupReShade), string.Empty);
        scopedLogger.LogInformation("Retrieving ReShade homepage");
        reShadeInstallationStatus.CurrentStep = ReShadeInstallationStatus.RetrievingLatestVersionUrl;
        var downloadUrl = await this.GetLatestDownloadUrl(cancellationToken);
        if (downloadUrl?.IsNullOrWhiteSpace() is not false)
        {
            scopedLogger.LogError("Unable to retrieve latest download url");
            return false;
        }

        scopedLogger.LogInformation($"Downloading installer from {downloadUrl}");
        var destinationPath = Path.Combine(Path.GetFullPath(ReShadePath), ReShadeInstallerName);
        var downloadResult = await this.downloadService.DownloadFile(downloadUrl, destinationPath, reShadeInstallationStatus);
        if (!downloadResult)
        {
            scopedLogger.LogError($"Failed to download {downloadUrl}. Check {nameof(DownloadService)} logs for errors");
            return false;
        }

        reShadeInstallationStatus.CurrentStep = ReShadeInstallationStatus.Installing;
        var setupResult = await this.SetupReshadeDllFromInstaller(destinationPath, cancellationToken);
        if (!setupResult)
        {
            scopedLogger.LogError($"Failed to setup reshade dll from installer {destinationPath}");
            return false;
        }

        var versionString = downloadUrl.Replace(ReShadeHomepageUrl + "/downloads/ReShade_Setup_", "").Replace(".exe", "");
        this.liveUpdateableOptions.Value.InstalledVersion = versionString;
        this.liveUpdateableOptions.UpdateOption();

        File.Delete(destinationPath);
        reShadeInstallationStatus.CurrentStep = ReShadeInstallationStatus.Finished;
        return true;
    }

    public async Task<bool> InstallPackage(ShaderPackage package, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.InstallPackage), package.Name ?? string.Empty);
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

        using var archive = ZipFile.Read(result.Content.ReadAsStream(cancellationToken));
        await this.InstallPackageInternal(archive, cancellationToken, package.InstallPath, package.TextureInstallPath, package.EffectFiles);
        this.notificationService.NotifyInformation(
            "ReShade package installed",
            $"Installation of {package.Name} has been successful");
        return true;
    }

    public async Task<bool> InstallPackage(string pathToZip, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.InstallPackage), pathToZip ?? string.Empty);
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

        using var fs = new FileStream(fullPath, FileMode.Open);
        using var archive = ZipFile.Read(fs);
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

    private async Task InstallPackageInternal(
        ZipFile archive,
        CancellationToken cancellationToken,
        string? desiredInstallationPath = default,
        string? desiredTextureInstallationPath = default,
        List<string>? fxFilter = default)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.InstallPackageInternal), archive.Name ?? string.Empty);
        var basePath = Path.GetFullPath(ReShadePath);
        desiredInstallationPath = desiredInstallationPath?.Replace(".\\", basePath + "\\");
        desiredTextureInstallationPath = desiredTextureInstallationPath?.Replace(".\\", basePath + "\\");
        foreach (var entry in archive.Entries)
        {
            if (FxExtensions.Any(entry.FileName.EndsWith))
            {
                scopedLogger.LogInformation($"[{entry.FileName}] Processing fx file");
                var fileName = StripAfterToken(entry.FileName, "shaders/");
                if (fxFilter is not null &&
                    !fxFilter.Contains(fileName))
                {
                    scopedLogger.LogInformation($"[{entry.FileName}] File not found in effects list. Skipping");
                    continue;
                }

                var destination = Path.Combine(basePath, PresetsFolder, "Shaders", fileName);
                if (desiredInstallationPath?.IsNullOrWhiteSpace() is false)
                {
                    destination = Path.Combine(desiredInstallationPath, fileName);
                }

                scopedLogger.LogInformation($"[{entry.FileName}] Installing fx at {destination}");
                new FileInfo(destination).Directory?.Create();
                using var fs = new FileStream(destination, FileMode.Create);
                using var reader = entry.OpenReader();
                await reader.CopyToAsync(fs, cancellationToken);

                scopedLogger.LogInformation($"[{entry.FileName}] Installed fx at {destination}");
            }
            else if (FxHeaderExtensions.Any(entry.FileName.EndsWith))
            {
                scopedLogger.LogInformation($"[{entry.FileName}] Processing fxh file");
                var fileName = StripAfterToken(entry.FileName, "shaders/");
                var destination = Path.Combine(basePath, PresetsFolder, "Shaders", fileName);
                if (desiredInstallationPath?.IsNullOrWhiteSpace() is false)
                {
                    destination = Path.Combine(desiredInstallationPath, fileName);
                }

                scopedLogger.LogInformation($"[{entry.FileName}] Installing fxh at {destination}");
                new FileInfo(destination).Directory?.Create();
                using var fs = new FileStream(destination, FileMode.Create);
                using var reader = entry.OpenReader();
                await reader.CopyToAsync(fs, cancellationToken);

                scopedLogger.LogInformation($"[{entry.FileName}] Installed fxh at {destination}");
            }
            else if (TextureExtensions.Any(entry.FileName.EndsWith))
            {
                scopedLogger.LogInformation($"Processing texture file {entry.FileName}");
                var fileName = StripAfterToken(entry.FileName, "Textures/");
                var destination = Path.Combine(basePath, PresetsFolder, "Textures", fileName);
                if (desiredTextureInstallationPath?.IsNullOrWhiteSpace() is false)
                {
                    destination = Path.Combine(desiredTextureInstallationPath, fileName);
                }

                scopedLogger.LogInformation($"[{entry.FileName}] Installing texture at {destination}");
                new FileInfo(destination).Directory?.Create();
                using var fs = new FileStream(destination, FileMode.Create);
                using var reader = entry.OpenReader();
                await reader.CopyToAsync(fs, cancellationToken);

                scopedLogger.LogInformation($"[{entry.FileName}] Installed texture at {destination}");
            }
        }
    }

    private async Task<string?> GetLatestDownloadUrl(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestDownloadUrl), string.Empty);
        var homePageResult = await this.httpClient.GetAsync(ReShadeHomepageUrl, cancellationToken);
        if (!homePageResult.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received unsuccessful status code {homePageResult.StatusCode} from {ReShadeHomepageUrl}");
            return default;
        }

        scopedLogger.LogInformation("Scrapping latest download url");
        var html = await homePageResult.Content.ReadAsStringAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        if (doc.DocumentNode.Descendants("a")
            .Select(node =>
            {
                if (node.GetAttributeValue<string>("href", string.Empty) is string href &&
                    href.ToLower().Contains("/downloads/"))
                {
                    return href;
                }

                return null;
            }).FirstOrDefault(href => href?.IsNullOrWhiteSpace() is false) is not string href)
        {
            scopedLogger.LogInformation("Unable to find the download url on the ReShade homepage");
            return default;
        }

        var downloadUrl = ReShadeHomepageUrl + href;
        return downloadUrl;
    }

    private async Task<bool> SetupReshadeDllFromInstaller(string pathToInstaller, CancellationToken cancellationToken)
    {
        var scoppedLogger = this.logger.CreateScopedLogger(nameof(this.SetupReshadeDllFromInstaller), pathToInstaller);
        try
        {
            using var fs = File.OpenRead(pathToInstaller);
            await SeekZipStart(fs, cancellationToken);
            using var zipStream = new OffsetStream(fs);
            using var zip = ZipFile.Read(zipStream);

            // Validate archive contains the ReShade DLLs
            if (zip.Entries.FirstOrDefault(e => e.FileName == DllName) is not ZipEntry entry)
            {
                throw new InvalidOperationException("File does not contain desired ReShade dll");
            }

            var reshadePath = Path.GetFullPath(ReShadePath);
            if (!Directory.Exists(reshadePath))
            {
                Directory.CreateDirectory(reshadePath);
            }

            var dllDestination = Path.Combine(reshadePath, DllName);
            using var reader = entry.OpenReader();
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

    private async Task CheckUpdates()
    {
        if (!this.IsInstalled)
        {
            return;
        }

        if (!this.liveUpdateableOptions.Value.AutoUpdate)
        {
            return;
        }

        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckUpdates), string.Empty);
        var downloadUrl = await this.GetLatestDownloadUrl(CancellationToken.None);
        if (downloadUrl?.IsNullOrWhiteSpace() is not false)
        {
            scopedLogger.LogError("Unable to retrieve latest download url");
            return;
        }

        var versionString = downloadUrl.Replace(ReShadeHomepageUrl + "/downloads/ReShade_Setup_", "").Replace(".exe", "");
        if (!Models.Versioning.Version.TryParse(versionString, out var version))
        {
            scopedLogger.LogError("Unable to parse latest version");
            return;
        }

        if (!Models.Versioning.Version.TryParse(this.liveUpdateableOptions.Value.InstalledVersion, out var installedVersion))
        {
            scopedLogger.LogError("Unable to parse installed version");
            return;
        }

        if (version.CompareTo(installedVersion) > 0)
        {
            scopedLogger.LogInformation($"Found an update for ReShade. Current version: {installedVersion}. Latest version: {version}");
            if (await this.SetupReShade(new ReShadeInstallationStatus(), CancellationToken.None) is not true)
            {
                scopedLogger.LogError("Failed to update ReShade");
            }
        }
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
        byte[] buffer = new byte[512];
        byte[] zipSignature = new byte[] { 0x50, 0x4B, 0x03, 0x04 }; // This is the ZIP Local File Header signature

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

using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.IconRetrieve;
using Daybreak.Shared.Utils;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve;

internal sealed class IconCache : IIconCache
{
    private const string HighResolutionGalleryUrl = $"https://wiki.guildwars.com/wiki/File:{NamePlaceholder}_(large).jpg";
    private const string WikiUrl = "https://wiki.guildwars.com";
    private const string NamePlaceholder = "[NAME]";
    private const string IconsDirectoryNameSubpath = "Icons";
    private readonly static string IconsDirectory = PathUtils.GetAbsolutePathFromRoot(IconsDirectoryNameSubpath);
    private readonly static string IconsLocation = PathUtils.GetAbsolutePathFromRoot(IconsDirectoryNameSubpath, $"{NamePlaceholder}.jpg");

    private readonly SemaphoreSlim diskSemaphore = new(1, 1);
    private readonly IHttpClient<IconCache> httpClient;
    private readonly ILiveOptions<LauncherOptions> options;
    private readonly ILogger<IconCache> logger;

    public IconCache(
        IHttpClient<IconCache> httpClient,
        ILiveOptions<LauncherOptions> options,
        ILogger<IconCache> logger)
    {
        this.httpClient = httpClient.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        if (Directory.Exists(IconsDirectory) is false)
        {
            Directory.CreateDirectory(IconsDirectory);
        }
    }

    public async Task<string?> GetIconUri(Skill skill, bool prefHighQuality = true)
    {
        if (skill is null ||
            skill.Name!.IsNullOrWhiteSpace())
        {
            return default;
        }

        var curedSkillName = CureSkillName(skill);
        var highQFileName = SkillFileSafeName(skill, false);
        var lowQFileName = SkillFileSafeName(skill, false);
        if (curedSkillName!.IsNullOrWhiteSpace() ||
            lowQFileName!.IsNullOrWhiteSpace() ||
            highQFileName!.IsNullOrWhiteSpace())
        {
            return default;
        }

        if (prefHighQuality)
        {
            var highResWikiUri = $"{WikiUrl}/wiki/File:{curedSkillName}_(large).jpg";
            var highResResult = await this.GetIconUriInternal(curedSkillName!, highQFileName!, highResWikiUri, false);
            if (highResResult is not null)
            {
                return highResResult;
            }
        }


        var wikiUri = $"{WikiUrl}/wiki/{curedSkillName}";
        return await this.GetIconUriInternal(curedSkillName!, lowQFileName!, wikiUri, false);
    }

    public async Task<string?> GetIconUri(ItemBase itemBase)
    {
        if (itemBase is null ||
            itemBase.Name!.IsNullOrWhiteSpace())
        {
            return default;
        }

        var curedName = CureName(itemBase.Name);
        var fileName = FileSafeName(itemBase.Name, false);
        if (curedName!.IsNullOrWhiteSpace() ||
            fileName!.IsNullOrWhiteSpace())
        {
            return default;
        }

        var wikiUri = $"{WikiUrl}/wiki/{curedName}";
        var directLink = false;
        if (itemBase is IIconUrlEntity iconUrlEntity)
        {
            wikiUri = iconUrlEntity.IconUrl;
            directLink = true;
        }

        return await this.GetIconUriInternal(curedName!, fileName!, wikiUri!, directLink);
    }

    private async Task<string?> GetIconUriInternal(string curedName, string fileName, string wikiUri, bool directLink)
    {
        await this.diskSemaphore.WaitAsync();
        try
        {
            var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetIconUriInternal), string.Empty);
            var maybeIconUri = this.GetLocalIcon(fileName);
            if (maybeIconUri is string uri)
            {
                return uri;
            }

            if (!this.options.Value.DownloadIcons)
            {
                scopedLogger.LogWarning("Icon not found and download disabled. Returning empty icon uri");

                return default;
            }

            var localUri = await this.DownloadAndRetrieveIcon(curedName, fileName, wikiUri, directLink);
            return localUri;
        }
        finally
        {
            this.diskSemaphore.Release();
        }
    }

    private async Task<string?> DownloadAndRetrieveIcon(string curedName, string fileName, string wikiUri, bool directLink)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(DownloadAndRetrieveIcon), curedName!);
        if (curedName is null)
        {
            return default;
        }

        var remoteIconUri = new Uri(wikiUri);
        if (!directLink)
        {
            remoteIconUri = await this.GetRemoteIconUri(curedName, wikiUri);
        }

        if (remoteIconUri is null)
        {
            return default;
        }

        var response = await this.httpClient.GetAsync(remoteIconUri);
        if (!response.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received [{response.StatusCode}]");
            return default!;
        }

        return await this.SaveLocalIcon(await response.Content.ReadAsByteArrayAsync(), fileName);
    }

    private async Task<Uri?> GetRemoteIconUri(string curedName, string wikiUri)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(GetRemoteIconUri), curedName);
        var skillPageResponse = await this.httpClient.GetAsync(wikiUri);
        if (!skillPageResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received [{skillPageResponse.StatusCode}]");
            return default;
        }

        var skillPageString = await skillPageResponse.Content.ReadAsStringAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(skillPageString);
        var imgElements = doc.DocumentNode.Descendants("img");
        foreach (var imgElement in imgElements)
        {
            if (imgElement.GetAttributeValue<string>("src", string.Empty) is string src &&
                src.ToLower().Contains(curedName.ToLower()) &&
                (Uri.TryCreate(src, UriKind.Absolute, out var iconUri) ||
                 Uri.TryCreate(new Uri(WikiUrl), src, out iconUri)))
            {
                return iconUri;
            }
        }

        return default;
    }

    private string? GetLocalIcon(string name)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLocalIcon), name);
        var filePath = IconsLocation.Replace(NamePlaceholder, name);
        scopedLogger.LogDebug("Checking local icon cache");
        if (!File.Exists(filePath))
        {
            scopedLogger.LogInformation("No local icon cache found");
            return default;
        }

        scopedLogger.LogDebug("Local icon cache found. Retrieving icon");
        return Path.GetFullPath(IconsLocation.Replace(NamePlaceholder, name));
    }

    private async Task<string?> SaveLocalIcon(byte[] bytes, string name)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.SaveLocalIcon), name!);
        var filePath = IconsLocation.Replace(NamePlaceholder, name);
        scopedLogger.LogDebug("Checking local icon cache");
        if (File.Exists(filePath))
        {
            scopedLogger.LogInformation("Local icon found, replacing");
        }

        await File.WriteAllBytesAsync(filePath, bytes);
        return Path.GetFullPath(filePath);
    }

    private static string? CureSkillName(Skill? skill)
    {
        if (skill is null)
        {
            return default;
        }

        return CureName(skill.AlternativeName!.IsNullOrWhiteSpace() ? skill.Name : skill.AlternativeName);
    }

    private static string? SkillFileSafeName(Skill? skill, bool highQuality)
    {
        if (skill is null)
        {
            return default;
        }

        return FileSafeName(skill.AlternativeName!.IsNullOrWhiteSpace() ? skill.Name : skill.AlternativeName, highQuality);
    }

    private static string? CureName(string? name)
    {
        return name?
            .Replace(" ", "_")
            .Replace("'", "%27")
            .Replace("!", "%21")
            .Replace("\"", "%22");
    }

    private static string? FileSafeName(string? name, bool highQuality)
    {
        return $"{name?.Replace("\"", string.Empty)}{(highQuality ? "-HQ" : string.Empty)}";
    }
}

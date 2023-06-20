using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using HtmlAgilityPack;
using MahApps.Metro.Controls;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve;

public sealed class IconCache : IIconCache
{
    private const string WikiUrl = "https://wiki.guildwars.com";
    private const string NamePlaceholder = "[NAME]";
    private const string IconsDirectoryName = "Icons";
    private const string IconsLocation = $"{IconsDirectoryName}/{NamePlaceholder}.jpg";

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

        if (Directory.Exists(IconsDirectoryName) is false)
        {
            Directory.CreateDirectory(IconsDirectoryName);
        }
    }

    public async Task<string?> GetIconUri(Skill skill)
    {
        if (skill is null ||
            skill.Name!.IsNullOrWhiteSpace())
        {
            return default;
        }

        var curedSkillName = CureSkillName(skill);
        var fileName = SkillFileSafeName(skill);
        if (curedSkillName!.IsNullOrWhiteSpace() ||
            fileName!.IsNullOrWhiteSpace())
        {
            return default;
        }

        var wikiUri = $"{WikiUrl}/wiki/{curedSkillName}";
        return await this.GetIconUriInternal(curedSkillName!, fileName!, wikiUri, false);
    }

    public async Task<string?> GetIconUri(ItemBase itemBase)
    {
        if (itemBase is null ||
            itemBase.Name!.IsNullOrWhiteSpace())
        {
            return default;
        }

        var curedName = CureName(itemBase.Name);
        var fileName = FileSafeName(itemBase.Name);
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

        return await this.DownloadAndRetrieveIcon(curedName, fileName, wikiUri, directLink);
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
        scopedLogger.LogInformation("Checking local icon cache");
        if (!File.Exists(filePath))
        {
            scopedLogger.LogWarning("No local icon cache found");
            return default;
        }

        scopedLogger.LogInformation("Local icon cache found. Retrieving icon");
        return Path.GetFullPath(IconsLocation.Replace(NamePlaceholder, name));
    }

    private async Task<string?> SaveLocalIcon(byte[] bytes, string name)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.SaveLocalIcon), name!);
        var filePath = IconsLocation.Replace(NamePlaceholder, name);
        scopedLogger.LogInformation("Checking local icon cache");
        if (File.Exists(filePath))
        {
            scopedLogger.LogWarning("Local icon found, replacing");
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

    private static string? SkillFileSafeName(Skill? skill)
    {
        if (skill is null)
        {
            return default;
        }

        return FileSafeName(skill.AlternativeName!.IsNullOrWhiteSpace() ? skill.Name : skill.AlternativeName);
    }

    private static string? CureName(string? name)
    {
        return name?
            .Replace(" ", "_")
            .Replace("'", "%27")
            .Replace("!", "%21")
            .Replace("\"", "%22");
    }

    private static string? FileSafeName(string? name)
    {
        return name?
            .Replace("\"", string.Empty);
    }
}

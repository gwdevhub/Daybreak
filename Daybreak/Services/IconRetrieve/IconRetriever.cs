using Daybreak.Configuration;
using Daybreak.Models.Builds;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve
{
    public sealed class IconRetriever : IIconRetriever
    {
        private const string NamePlaceholder = "[SKILLNAME]";
        private const string BaseUrl = "https://wiki.guildwars.com";
        private const string QueryUrl = $"wiki/File:{NamePlaceholder}.jpg";
        private const string IconsDirectoryName = "Icons";
        private const string IconsLocation = $"{IconsDirectoryName}/{NamePlaceholder}.jpg";

        private readonly IHttpClient<IconRetriever> httpClient;
        private readonly ILogger<IconRetriever> logger;
        private readonly ILiveOptions<ApplicationConfiguration> liveOptions;

        public IconRetriever(
            ILogger<IconRetriever> logger,
            IHttpClient<IconRetriever> httpClient,
            ILiveOptions<ApplicationConfiguration> liveOptions)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.httpClient = httpClient.ThrowIfNull(nameof(httpClient));
            this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
            this.httpClient.BaseAddress = new Uri(BaseUrl);
            if (Directory.Exists(IconsDirectoryName) is false)
            {
                Directory.CreateDirectory(IconsDirectoryName);
            }
        }

        public async Task<Optional<Stream>> GetIcon(Skill skill)
        {
            if (this.liveOptions.Value.KeepLocalIconCache)
            {
                this.logger.LogInformation($"{nameof(IconRetriever)} configured to look first in cache before downloading icons");
                var maybeIcon = await this.GetLocalIcon(skill);
                if (maybeIcon.ExtractValue() is Stream stream)
                {
                    return stream;
                }
            }
            else
            {
                this.logger.LogInformation($"{nameof(IconRetriever)} configured to skip local cache. Downloading icon");
            }

            return await this.DownloadIcon(skill);
        }

        private async Task<Optional<Stream>> DownloadIcon(Skill skill)
        {
            var curedSkillName = skill.Name
                .Replace(" ", "_");
            var skillIconUrl = QueryUrl.Replace(NamePlaceholder, curedSkillName);
            this.logger.LogInformation($"Looking up icon for skill '{skill.Name}' at url {skillIconUrl}");
            using var response = await this.httpClient.GetAsync(skillIconUrl).ConfigureAwait(false);
            if (response.IsSuccessStatusCode is false)
            {
                this.logger.LogError($"Client returned status code {response.StatusCode}");
                return Optional.None<Stream>();
            }

            this.logger.LogInformation("Crawling through response for href to latest icon url");
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            var url = GetHref(doc);
            if (url is null)
            {
                this.logger.LogError("Failed to find latest icon url");
                return Optional.None<Stream>();
            }

            this.logger.LogInformation($"Found latest icon url at {BaseUrl + "/" + url}. Requesting stream");
            using var iconResponse = await this.httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                this.logger.LogInformation("Retrieved latest icon stream");
                var iconData = await iconResponse.Content.ReadAsByteArrayAsync();
                if (this.liveOptions.Value.KeepLocalIconCache)
                {
                    await SaveIconLocally(skill, iconData);
                }

                return new MemoryStream(iconData);
            }

            this.logger.LogError($"Failed to retrieve icon from {BaseUrl + "/" + url}");
            return Optional.None<Stream>();
        }

        private async Task<Optional<Stream>> GetLocalIcon(Skill skill)
        {
            var curedSkillName = skill.Name
                .Replace(" ", "_")
                .Replace("\"", "");
            this.logger.LogInformation("Checking local icon cache");
            if (File.Exists(IconsLocation.Replace(NamePlaceholder, curedSkillName)))
            {
                this.logger.LogInformation("Local icon cache found. Retrieving icon");
                return new MemoryStream(await File.ReadAllBytesAsync(IconsLocation.Replace(NamePlaceholder, curedSkillName)));
            }

            this.logger.LogWarning("No local icon cache found");
            return Optional.None<Stream>();
        }

        private static async Task SaveIconLocally(Skill skill, byte[] data)
        {
            var curedSkillName = skill.Name
                .Replace(" ", "_")
                .Replace("\"", "");
            await File.WriteAllBytesAsync(IconsLocation.Replace(NamePlaceholder, curedSkillName), data);
        }

        private static string GetHref(HtmlDocument doc)
        {
            foreach (var child in doc.DocumentNode.Descendants("a"))
            {
                var targetAttribute = child.Attributes.Where(a => a.Name == "href" && a.Value.Contains("images")).FirstOrDefault();
                if (targetAttribute is not null)
                {
                    return targetAttribute.Value;
                }
            }

            return null;
        }
    }
}

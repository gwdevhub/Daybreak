using Daybreak.Models.Builds;
using Daybreak.Services.Logging;
using Daybreak.Utils;
using HtmlAgilityPack;
using System;
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

        private readonly HttpClient httpClient = new();
        private readonly ILogger logger;

        public IconRetriever(
            ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<Optional<Stream>> GetIcon(Skill skill)
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
                return new MemoryStream(await iconResponse.Content.ReadAsByteArrayAsync());
            }

            this.logger.LogError($"Failed to retrieve icon from {BaseUrl + "/" + url}");
            return Optional.None<Stream>();
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

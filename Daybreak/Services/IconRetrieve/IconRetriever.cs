using Daybreak.Configuration;
using Daybreak.Models.Builds;
using Microsoft.Extensions.Logging;
using Models;
using Services.IconRetrieve;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve
{
    public sealed class IconRetriever : IIconRetriever
    {
        private const string NamePlaceholder = "[SKILLNAME]";
        private const string IconsDirectoryName = "Icons";
        private const string IconsLocation = $"{IconsDirectoryName}/{NamePlaceholder}.jpg";

        private readonly IIconBrowser iconBrowser;
        private readonly ILogger<IconRetriever> logger;
        private readonly ILiveOptions<ApplicationConfiguration> liveOptions;

        public IconRetriever(
            ILogger<IconRetriever> logger,
            IIconBrowser iconBrowser,
            ILiveOptions<ApplicationConfiguration> liveOptions)
        {
            this.iconBrowser = iconBrowser.ThrowIfNull();
            this.logger = logger.ThrowIfNull();
            this.liveOptions = liveOptions.ThrowIfNull();
            if (Directory.Exists(IconsDirectoryName) is false)
            {
                Directory.CreateDirectory(IconsDirectoryName);
            }
        }

        public async Task<Optional<Uri>> GetIconUri(Skill skill)
        {
            var maybeIconUri = this.GetLocalIcon(skill);
            if (maybeIconUri.ExtractValue() is Uri uri)
            {
                return uri;
            }

            if (this.liveOptions.Value.ExperimentalFeatures.DownloadIcons)
            {
                this.logger.LogInformation("Application is configured to download icons");
                return await this.DownloadIcon(skill);
            }

            return Optional.None<Uri>();
        }

        private async Task<Optional<Uri>> DownloadIcon(Skill skill)
        {
            var maybeBase64 = await this.GetBase64ForSkill(skill);
            if (maybeBase64.ExtractValue() is not string base64)
            {
                return Optional.None<Uri>();
            }

            var bytes = Convert.FromBase64String(base64);
            var uri = await SaveIconLocally(skill, bytes);
            return new Uri(AppDomain.CurrentDomain.BaseDirectory + "/" + IconsLocation.Replace(NamePlaceholder, uri), UriKind.Absolute);
        }

        private Optional<Uri> GetLocalIcon(Skill skill)
        {
            var curedSkillName = skill.Name
                .Replace(" ", "_")
                .Replace("\"", "");
            this.logger.LogInformation("Checking local icon cache");
            if (File.Exists(IconsLocation.Replace(NamePlaceholder, curedSkillName)))
            {
                this.logger.LogInformation("Local icon cache found. Retrieving icon");
                return new Uri(AppDomain.CurrentDomain.BaseDirectory + "/" + IconsLocation.Replace(NamePlaceholder, curedSkillName), UriKind.Absolute);
            }

            this.logger.LogWarning("No local icon cache found");
            return Optional.None<Uri>();
        }

        private async Task<Optional<string>> GetBase64ForSkill(Skill skill)
        {
            var request = new IconRequest { Skill = skill };
            this.iconBrowser.QueueIconRequest(request);

            // Wait for the request to be served
            for (var i = 0; i < 20; i++)
            {
                if (request.Finished)
                {
                    return request.IconBase64;
                }

                await Task.Delay(1000);
            }

            return Optional.None<string>();
        }

        private static async Task<string> SaveIconLocally(Skill skill, byte[] data)
        {
            var curedSkillName = skill.Name
                .Replace(" ", "_")
                .Replace("\"", "");
            await File.WriteAllBytesAsync(IconsLocation.Replace(NamePlaceholder, curedSkillName), data);
            return curedSkillName;
        }
    }
}

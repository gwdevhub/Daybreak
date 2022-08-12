using Daybreak.Configuration;
using Daybreak.Models.Builds;
using Microsoft.Extensions.Logging;
using Models;
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

        private readonly ILogger<IconRetriever> logger;

        public IconRetriever(
            ILogger<IconRetriever> logger)
        {
            this.logger = logger.ThrowIfNull();
            if (Directory.Exists(IconsDirectoryName) is false)
            {
                Directory.CreateDirectory(IconsDirectoryName);
            }
        }

        public Task<Optional<Uri>> GetIconUri(Skill skill)
        {
            var maybeIconUri = this.GetLocalIcon(skill);
            if (maybeIconUri.ExtractValue() is Uri uri)
            {
                return Task.FromResult(Optional.FromValue(uri));
            }

            return Task.FromResult(Optional.None<Uri>());
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
    }
}

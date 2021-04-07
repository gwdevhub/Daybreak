using Daybreak.Services.Bloogum.Models;
using Daybreak.Services.Logging;
using Daybreak.Utils;
using System;
using System.Extensions;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Daybreak.Services.Bloogum
{
    public sealed class BloogumClient : IBloogumClient
    {
        private const string BaseAddress = "http://bloogum.net/guildwars";
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private readonly Random random = new();

        public BloogumClient(ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));

            this.httpClient = new HttpClient();
        }

        public async Task<Optional<Stream>> GetRandomScreenShot()
        {
            var location = Location.Locations[this.random.Next(0, Location.Locations.Count)];
            var category = location.Categories[this.random.Next(0, location.Categories.Count)];
            var picture = this.random.Next(0, category.ImageCount) + 1;

            var uri = $"{location.LocationName}/{category.CategoryName}/{picture:00}.jpg";
            return await this.GetImage(uri).ConfigureAwait(false);
        }

        private async Task<Optional<Stream>> GetImage(string url)
        {
            this.logger.LogInformation($"Retrieving image from {BaseAddress}/{url}");
            try
            {
                var response = await httpClient.GetAsync($"{BaseAddress}/{url}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    this.logger.LogInformation("Received success status code");
                    var ms = new MemoryStream(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false));
                    return ms;
                }
                else
                {
                    this.logger.LogError($"Failed to retrive image. Status code {response.StatusCode}. Reason {response.ReasonPhrase}");
                    return Optional.None<Stream>();
                }
            }
            catch(Exception e)
            {
                this.logger.LogError(e);
                return Optional.None<Stream>();
            }
        }
    }
}

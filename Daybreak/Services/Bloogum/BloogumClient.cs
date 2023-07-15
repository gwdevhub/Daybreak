using Daybreak.Services.Bloogum.Models;
using Daybreak.Services.Images;
using Daybreak.Services.Scanner;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Services.Bloogum;

public sealed class BloogumClient : IBloogumClient
{
    private const string CacheFolder = "Bloogum";
    private const string BaseAddress = "http://bloogum.net/guildwars";
    private readonly IImageCache imageCache;
    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly IHttpClient<BloogumClient> httpClient;
    private readonly ILogger logger;

    public BloogumClient(
        IImageCache imageCache,
        IGuildwarsMemoryCache guildwarsMemoryCache,
        ILogger<BloogumClient> logger,
        IHttpClient<BloogumClient> httpClient)
    {
        this.imageCache = imageCache.ThrowIfNull();
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
    }

    public async Task<ImageSource?> GetImage(bool localized)
    {
        var uri = await this.GetImageUri(localized);
        var localUri = Path.GetFullPath(Path.Combine(CacheFolder, uri));
        if (!File.Exists(localUri))
        {
            var imageStream = await this.GetRemoteImage(uri);
            if (imageStream is null)
            {
                return default;
            }

            await CacheImage(localUri, imageStream);
            imageStream.Dispose();
        }

        return await this.imageCache.GetImage(localUri);
    }

    private async Task<string> GetImageUri(bool localized)
    {
        if (!localized)
        {
            return GetRandomScreenShot();
        }

        var worldInfo = await this.guildwarsMemoryCache.ReadWorldData(CancellationToken.None);
        if (worldInfo is null)
        {
            return GetRandomScreenShot();
        }

        var validLocations = Location.Locations.Where(l => l.Region == worldInfo.Region).ToList();
        var validCategories = validLocations.SelectMany(l => l.Categories).Where(c => c.Map == worldInfo.Map).ToList();
        if (validCategories.None())
        {
            if (validLocations.None())
            {
                return GetRandomScreenShot();
            }

            var location = validLocations[Random.Shared.Next(0, validLocations.Count)];
            return GetRandomScreenShot(location);
        }

        var selectedCategory = validCategories[Random.Shared.Next(0, validCategories.Count)];
        if (selectedCategory.ImageCount == 0)
        {
            if (validLocations.None())
            {
                return GetRandomScreenShot();
            }

            var location = validLocations[Random.Shared.Next(0, validLocations.Count)];
            return GetRandomScreenShot(location);
        }

        var selectedLocation = Location.Locations.FirstOrDefault(l => l.Categories.Contains(selectedCategory));
        if (selectedLocation is null)
        {
            if (validLocations.None())
            {
                return GetRandomScreenShot();
            }

            var location = validLocations[Random.Shared.Next(0, validLocations.Count)];
            return GetRandomScreenShot(location);
        }

        return GetScreenshotName(selectedLocation, selectedCategory, Random.Shared.Next(0, selectedCategory.ImageCount));
    }

    private async Task<Stream?> GetRemoteImage(string url)
    {
        this.logger.LogInformation($"Retrieving image from {BaseAddress}/{url}");
        try
        {
            var response = await this.httpClient.GetAsync($"{BaseAddress}/{url}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                this.logger.LogInformation("Received success status code");
                if (response.Headers.TryGetValues("Content-Length", out var values) &&
                    values.FirstOrDefault() is string responseLengthString &&
                    int.TryParse(responseLengthString, out var responseLength) &&
                    responseLength < 10e4)
                {
                    this.logger.LogError("Received invalid response. Request was probably blocked by Cloudflare");
                    return default;
                }

                var responseStream = response.Content.ReadAsStream();
                if (responseStream.Length < 10e4)
                {
                    this.logger.LogError("Received invalid response. Request was probably blocked by Cloudflare");
                    return default;
                }

                return responseStream;
            }
            else
            {
                this.logger.LogError($"Failed to retrive image. Status code {response.StatusCode}. Reason {response.ReasonPhrase}");
                return default;
            }
        }
        catch(Exception e)
        {
            this.logger.LogError(e.ToString());
            return default;
        }
    }

    private static string GetRandomScreenShot()
    {
        var location = Location.Locations[Random.Shared.Next(0, Location.Locations.Count)];
        return GetRandomScreenShot(location);
    }

    private static string GetRandomScreenShot(Location location)
    {
        var category = location.Categories[Random.Shared.Next(0, location.Categories.Count)];
        var picture = Random.Shared.Next(0, category.ImageCount) + 1;

        return GetScreenshotName(location, category, picture);
    }

    private static string GetScreenshotName(Location location, Category category, int picture)
    {
        return $"{location.LocationName}/{category.CategoryName}/{picture:00}.jpg";
    }

    private static async Task CacheImage(string uri, Stream imageStream)
    {
        var directoryName = Path.GetDirectoryName(uri);
        Directory.CreateDirectory(directoryName);
        using var fs = File.Create(uri);
        await imageStream.CopyToAsync(fs);
    }
}

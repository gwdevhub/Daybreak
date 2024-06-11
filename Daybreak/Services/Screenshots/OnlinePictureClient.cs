﻿using Daybreak.Configuration.Options;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Images;
using Daybreak.Services.Scanner;
using Daybreak.Services.Screenshots.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Services.Screenshots;

internal sealed class OnlinePictureClient : IOnlinePictureClient
{
    private const string CloudFlareCookieValue = "fcfd523b2470336531e47baff3d2c2d6a0e2412a.1689426482.1";
    private const string CloudFlareCookieKey = "wschkid";
    private const string CacheFolderSubPath = "ImageCache";

    private static readonly string CacheFolder = PathUtils.GetAbsolutePathFromRoot(CacheFolderSubPath);

    private readonly IImageCache imageCache;
    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly IHttpClient<OnlinePictureClient> httpClient;
    private readonly ILiveOptions<ThemeOptions> themeOptions;
    private readonly ILogger logger;

    public OnlinePictureClient(
        IImageCache imageCache,
        IGuildwarsMemoryCache guildwarsMemoryCache,
        ILogger<OnlinePictureClient> logger,
        ILiveOptions<ThemeOptions> themeOptions,
        IHttpClient<OnlinePictureClient> httpClient)
    {
        this.imageCache = imageCache.ThrowIfNull();
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.themeOptions = themeOptions.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();

        this.httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", $"{CloudFlareCookieKey}={CloudFlareCookieValue}");
    }

    public async Task<(ImageSource? Source, string Credit)> GetImage(bool localized)
    {
        (var uri, var credit) = await this.GetImageUri(localized);
        var localUri = Path.GetFullPath(Path.Combine(CacheFolder, uri))
            .Replace("https:\\", "").Replace("http:\\", "")
            .Replace("?", "")
            .Replace("&", "")
            .Replace("=", "");
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

        return (await this.imageCache.GetImage(localUri), credit);
    }

    private async Task<(string Uri, string CreditText)> GetImageUri(bool localized)
    {
        if (this.themeOptions.Value.WintersdayMode)
        {
            return await this.GetWintersdayUri(localized);
        }

        if (!localized)
        {
            return GetRandomScreenShot();
        }

        return await this.GetLocalizedImageUri();
    }

    private async Task<(string Uri, string CreditText)> GetWintersdayUri(bool localized)
    {
        var validEntries = Location.Locations
                .SelectMany(l => l.Entries)
                .Where(e => Models.Event.Wintersday.ValidLocations!.Any(map => e.Map == map));
        if (localized)
        {
            WorldData? worldInfo = default;
            try
            {
                worldInfo = await this.guildwarsMemoryCache.ReadWorldData(CancellationToken.None);
            }
            catch (Exception ex) when (ex is TimeoutException or TaskCanceledException or HttpRequestException)
            {
                this.logger.LogInformation("Could not retrieve world data. Returning random screenshot");
            }

            if (worldInfo?.Map is Map map)
            {
                var localizedEntries = validEntries.Where(e => e.Map == map).ToList();
                if (localizedEntries.Count > 0)
                {
                    var selectedEntry = localizedEntries[Random.Shared.Next(0, localizedEntries.Count)];
                    return GetScreenshotName(selectedEntry, selectedEntry.StartIndex ?? 0 + Random.Shared.Next(0, selectedEntry.Count ?? 0));
                }
            }
        }

        var finalEntries = validEntries.ToList();
        var selectedFinalEntry = finalEntries[Random.Shared.Next(0, finalEntries.Count)];
        return GetScreenshotName(selectedFinalEntry, selectedFinalEntry.StartIndex ?? 0 + Random.Shared.Next(0, selectedFinalEntry.Count ?? 0));
    }

    private async Task<(string Uri, string CreditText)> GetLocalizedImageUri()
    {
        WorldData? worldInfo = default;
        try
        {
            worldInfo = await this.guildwarsMemoryCache.ReadWorldData(CancellationToken.None);
            if (worldInfo is null)
            {
                return GetRandomScreenShot();
            }
        }
        catch (Exception ex) when (ex is TimeoutException or TaskCanceledException or HttpRequestException)
        {
            this.logger.LogInformation("Could not retrieve world data. Returning random screenshot");
            return GetRandomScreenShot();
        }

        var validLocations = Location.Locations
            .Where(l => l.Region == worldInfo!.Region)
            .ToList();
        var validCategories = validLocations
            .SelectMany(l => l.Entries)
            .Where(c => c.Map == worldInfo!.Map)
            .ToList();
        if (validCategories.None())
        {
            if (validLocations.None())
            {
                return GetRandomScreenShot();
            }

            var location = validLocations[Random.Shared.Next(0, validLocations.Count)];
            return GetRandomScreenShot(location);
        }

        return this.GetImageUri(validLocations, validCategories);
    }

    private (string Uri, string CreditText) GetImageUri(List<Location> validLocations, List<Entry> validCategories)
    {
        var selectedCategory = validCategories[Random.Shared.Next(0, validCategories.Count)];
        if (selectedCategory.Count == 0)
        {
            if (validLocations.None())
            {
                return GetRandomScreenShot();
            }

            var location = validLocations[Random.Shared.Next(0, validLocations.Count)];
            return GetRandomScreenShot(location);
        }

        return GetScreenshotName(selectedCategory, Random.Shared.Next(selectedCategory.StartIndex ?? 0, (selectedCategory.Count + selectedCategory.StartIndex) ?? 0));
    }

    private async Task<Stream?> GetRemoteImage(string url)
    {
        this.logger.LogInformation($"Retrieving image from {url}");
        try
        {
            var response = await this.httpClient.GetAsync($"{url}").ConfigureAwait(false);
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
        catch (Exception e)
        {
            this.logger.LogError(e.ToString());
            return default;
        }
    }

    private static (string Uri, string CreditText) GetRandomScreenShot()
    {
        var location = Location.Locations[Random.Shared.Next(0, Location.Locations.Count)];
        return GetRandomScreenShot(location);
    }

    private static (string Uri, string CreditText) GetRandomScreenShot(Location location)
    {
        var category = location.Entries[Random.Shared.Next(0, location.Entries.Count)];
        var picture = Random.Shared.Next(0, category.Count ?? 0) + 1;

        return GetScreenshotName(category, picture);
    }

    private static (string Uri, string CreditText) GetScreenshotName(Entry category, int picture)
    {
        return (category.Url?.Replace("{ID}", picture.ToString(category.IdFormat)) ?? string.Empty, category.Credit ?? string.Empty);
    }

    private static async Task CacheImage(string uri, Stream imageStream)
    {
        var directoryName = Path.GetDirectoryName(uri);
        Directory.CreateDirectory(directoryName!);
        using var fs = File.Create(uri);
        await imageStream.CopyToAsync(fs);
    }
}

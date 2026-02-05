using Daybreak.Configuration.Options;
using Daybreak.Services.Graph.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Daybreak.Services.Graph;

internal sealed class BlazorGraphClient : IGraphClient
{
    private const string GraphBaseUrl = "https://graph.microsoft.com/v1.0/";
    private const string ProfileEndpoint = "me";
    private const string BuildsSyncFileUri = $"me/drive/root:/Daybreak/Builds/daybreak.json:/content";
    private const string SettingsSyncFileUri = $"me/drive/root:/Daybreak/Settings/daybreak.json:/content";

    public const string RedirectUri = "http://localhost:42111";

    public static readonly string[] GraphScopes = [
        "Files.Read", 
        "Files.Read.All", 
        "Files.ReadWrite", 
        "Files.ReadWrite.All", 
        "User.Read", 
        "offline_access"
    ];

    private readonly IOptionsProvider optionsProvider;
    private readonly IPublicClientApplication publicClientApplication;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IOptionsMonitor<SynchronizationOptions> liveUpdateableOptions;
    private readonly IHttpClient<BlazorGraphClient> httpClient;
    private readonly ILogger<BlazorGraphClient> logger;

    private List<BuildFile>? buildsCache;

    public BlazorGraphClient(
        IOptionsProvider optionsProvider,
        IPublicClientApplication publicClientApplication,
        IBuildTemplateManager buildTemplateManager,
        IOptionsMonitor<SynchronizationOptions> liveUpdateableOptions,
        IHttpClient<BlazorGraphClient> httpClient,
        ILogger<BlazorGraphClient> logger)
    {
        this.optionsProvider = optionsProvider.ThrowIfNull();
        this.publicClientApplication = publicClientApplication.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.httpClient.BaseAddress = new Uri(GraphBaseUrl);
        this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<bool> PerformAuthorizationFlow(CancellationToken cancellationToken = default)
    {
        if (await this.GetValidAccessToken() is not null)
        {
            return true;
        }

        try
        {
            var result = await this.publicClientApplication.AcquireTokenInteractive(GraphScopes)
                .WithEmbeddedWebViewOptions(new EmbeddedWebViewOptions { Title = "Daybreak - Sign in with Microsoft" })
                .WithUseEmbeddedWebView(true)
                .ExecuteAsync(cancellationToken);
            return false;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "OAuth flow failed");
            return false;
        }
    }

    public async Task<User?> GetUserProfile<TViewType>()
        where TViewType : ComponentBase
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                scopedLogger.LogError("Client is not authorized");
                return default;
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await this.httpClient.GetAsync(ProfileEndpoint);
            
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to load profile. Status code: {StatusCode}", response.StatusCode);
                return default;
            }

            var profile = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            return profile;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to get user profile");
            return default;
        }
    }

    public async Task<bool> UploadBuilds()
    {
        var builds = await this.buildTemplateManager.GetBuilds().ToListAsync();
        return await this.PutBuilds(builds);
    }

    public async Task<bool> DownloadBuilds()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var retrieveBuildsResponse = await this.RetrieveBuildsList();
        if (retrieveBuildsResponse is null)
        {
            scopedLogger.LogError("Unable to download builds");
            return false;
        }

        this.buildTemplateManager.ClearBuilds();
        var compiledBuilds = this.buildsCache?.Select(buildFile =>
        {
            if (buildFile is null ||
                buildFile.TemplateCode is null)
            {
                return default;
            }

            if (this.buildTemplateManager.TryDecodeTemplate(buildFile.TemplateCode, out var build) is false)
            {
                return default;
            }

            build.SourceUrl = buildFile.SourceUrl;
            build.Name = buildFile.FileName;
            build.PreviousName = buildFile.FileName;
            return build;
        }).Where(entry => entry is not null).ToList();
        
        _ = compiledBuilds?.Do(this.buildTemplateManager.SaveBuild!).ToList();
        return true;
    }

    public async Task<bool> DownloadBuild(string buildName)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var retrieveBuildsResponse = await this.RetrieveBuildsList();
        if (retrieveBuildsResponse is null)
        {
            scopedLogger.LogError("Unable to download builds");
            return false;
        }

        var compiledBuilds = this.buildsCache?
            .Where(b => b.FileName == buildName)
            .Select(buildFile =>
            {
                if (this.buildTemplateManager.TryDecodeTemplate(buildFile.TemplateCode!, out var build) is false)
                {
                    return null;
                }

                build.SourceUrl = buildFile.SourceUrl;
                build.Name = buildFile.FileName;
                build.PreviousName = buildFile.FileName;
                return build;
            }).Where(entry => entry is not null).ToList();
        
        _ = compiledBuilds?.Do(this.buildTemplateManager.SaveBuild!).ToList();
        return true;
    }

    public async Task<bool> UploadBuild(string buildName)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var getBuildResult = await this.buildTemplateManager.GetBuild(buildName);
        if (getBuildResult is null)
        {
            scopedLogger.LogError("Build {BuildName} not found for upload", buildName);
            return false;
        }

        return await this.PutBuild(getBuildResult);
    }

    public async Task<IEnumerable<BuildFile>?> RetrieveBuildsList()
    {
        var maybeBuildsBackup = await this.GetBuildsBackup();
        this.buildsCache = maybeBuildsBackup;
        return maybeBuildsBackup;
    }

    public async Task<bool> UploadSettings(string settings, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                scopedLogger.LogError("Client is not authorized");
                return false;
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var stringContent = new StringContent(settings);
            var response = await this.httpClient.PutAsync(SettingsSyncFileUri, stringContent, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to upload settings");
            return false;
        }
    }

    public async Task<string?> DownloadSettings(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                scopedLogger.LogError("Client is not authorized");
                return default;
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var fileItemResponse = await this.httpClient.GetAsync(SettingsSyncFileUri, cancellationToken);
            
            if (fileItemResponse.IsSuccessStatusCode is false)
            {
                return default;
            }

            var driveItemContent = await fileItemResponse.Content.ReadAsStringAsync(cancellationToken);
            return driveItemContent;
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to download settings");
            return default;
        }
    }

    public Task<bool> LogOut()
    {
        this.ResetAuthorization();
        return Task.FromResult(true);
    }

    public void ResetAuthorization()
    {
        var options = this.liveUpdateableOptions.CurrentValue;
        options.ProtectedGraphAccessToken = null;
        options.ProtectedGraphRefreshToken = null;
        this.optionsProvider.SaveOption(options);
    }

    private async Task<string?> GetValidAccessToken()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var accounts = await this.publicClientApplication.GetAccountsAsync();
        if (accounts?.Any() is not true)
        {
            scopedLogger.LogWarning("No accounts found in token cache");
            return default;
        }

        var firstAccount = accounts.First();
        try
        {
            var result = await this.publicClientApplication.AcquireTokenSilent(GraphScopes, firstAccount)
                .ExecuteAsync();

            return result.AccessToken;
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to acquire token silently");
            return default;
        }
    }

    private async Task<bool> PutBuild(IBuildEntry buildEntry)
    {
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            if (this.buildsCache is null)
            {
                _ = await this.RetrieveBuildsList();
            }

            var buildFile = new BuildFile
            {
                FileName = buildEntry.Name,
                TemplateCode = this.buildTemplateManager.EncodeTemplate(buildEntry),
                SourceUrl = buildEntry.SourceUrl
            };

            var buildList = this.buildsCache ?? [];
            buildList = [.. buildList.Where(b => b.FileName != buildEntry.Name)];
            buildList.Add(buildFile);
            buildList = [.. buildList.OrderBy(b => b.FileName)];

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var stringContent = new StringContent(JsonSerializer.Serialize(buildList));
            var response = await this.httpClient.PutAsync(BuildsSyncFileUri, stringContent);
            
            if (response.IsSuccessStatusCode is false)
            {
                return false;
            }

            this.buildsCache = buildList;
            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to put build");
            return false;
        }
    }

    private async Task<bool> PutBuilds(List<IBuildEntry> buildEntries)
    {
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            var buildFiles = buildEntries.Select(buildEntry => new BuildFile
            {
                FileName = buildEntry.Name,
                TemplateCode = this.buildTemplateManager.EncodeTemplate(buildEntry),
                SourceUrl = buildEntry.SourceUrl
            });

            var buildList = new List<BuildFile>();
            buildList.AddRange(buildFiles);
            buildList = [.. buildList.OrderBy(b => b.FileName)];

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var stringContent = new StringContent(JsonSerializer.Serialize(buildList));
            var response = await this.httpClient.PutAsync(BuildsSyncFileUri, stringContent);
            
            if (response.IsSuccessStatusCode is false)
            {
                return false;
            }

            this.buildsCache = buildList;
            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to put builds");
            return false;
        }
    }

    private async Task<List<BuildFile>?> GetBuildsBackup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                scopedLogger.LogError("Client is not authorized");
                return default;
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var fileItemResponse = await this.httpClient.GetAsync(BuildsSyncFileUri);
            
            if (fileItemResponse.IsSuccessStatusCode is false)
            {
                scopedLogger.LogError("Failed to retrieve builds backup. Status code: {StatusCode}", fileItemResponse.StatusCode);
                return default;
            }

            var driveItemContent = await fileItemResponse.Content.ReadAsStringAsync();
            var backup = JsonSerializer.Deserialize<List<BuildFile>>(driveItemContent);
            
            if (backup is null)
            {
                scopedLogger.LogError("Deserialized builds backup is null");
                return default;
            }

            return backup;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to get builds backup");
            return default;
        }
    }
}

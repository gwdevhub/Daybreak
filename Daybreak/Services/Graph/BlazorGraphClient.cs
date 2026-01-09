using Daybreak.Configuration.Options;
using Daybreak.Services.Graph.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Net.Http.Headers;

namespace Daybreak.Services.Graph;

//TODO: Fix live updateable options usage
internal sealed class BlazorGraphClient : IGraphClient
{
    private const string GraphBaseUrl = "https://graph.microsoft.com/v1.0/";
    private const string ProfileEndpoint = "me";
    private const string BuildsSyncFileUri = $"me/drive/root:/Daybreak/Builds/daybreak.json:/content";
    private const string SettingsSyncFileUri = $"me/drive/root:/Daybreak/Settings/daybreak.json:/content";
    private const string ContentSuffix = ":/content";

    public const string RedirectUri = "http://localhost:42111";

    public static readonly string[] GraphScopes = [
        "Files.Read", 
        "Files.Read.All", 
        "Files.ReadWrite", 
        "Files.ReadWrite.All", 
        "User.Read", 
        "offline_access"
    ];

    private static readonly byte[] Entropy = System.Convert.FromBase64String("R3VpbGR3YXJz");

    private readonly IPublicClientApplication publicClientApplication;
    private readonly IBuildTemplateManager buildTemplateManager;
    //private readonly ILiveUpdateableOptions<SynchronizationOptions> liveUpdateableOptions;
    private readonly IHttpClient<BlazorGraphClient> httpClient;
    private readonly ILogger<BlazorGraphClient> logger;

    private List<BuildFile>? buildsCache;

    public BlazorGraphClient(
        IPublicClientApplication publicClientApplication,
        IBuildTemplateManager buildTemplateManager,
        //ILiveUpdateableOptions<SynchronizationOptions> liveUpdateableOptions,
        IHttpClient<BlazorGraphClient> httpClient,
        ILogger<BlazorGraphClient> logger)
    {
        this.publicClientApplication = publicClientApplication.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        //this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.httpClient.BaseAddress = new Uri(GraphBaseUrl);
        this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<Result<bool, Exception>> PerformAuthorizationFlow(CancellationToken cancellationToken = default)
    {
        if (await this.GetValidAccessToken() is string)
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
            return ex;
        }
    }

    public async Task<Result<User, Exception>> GetUserProfile<TViewType>()
        where TViewType : ComponentBase
    {
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return new InvalidOperationException("Client is not authorized");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await this.httpClient.GetAsync(ProfileEndpoint);
            
            if (!response.IsSuccessStatusCode)
            {
                return new InvalidOperationException($"Failed to load profile. Response status code [{response.StatusCode}]");
            }

            var profile = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            return profile!;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to get user profile");
            return ex;
        }
    }

    public async Task<Result<bool, Exception>> UploadBuilds()
    {
        var builds = await this.buildTemplateManager.GetBuilds().ToListAsync();
        return await this.PutBuilds(builds);
    }

    public async Task<Result<bool, Exception>> DownloadBuilds()
    {
        var retrieveBuildsResponse = await this.RetrieveBuildsList();
        if (retrieveBuildsResponse.TryExtractFailure(out var failure))
        {
            this.logger.LogError(failure, "Unable to download builds");
            return false;
        }

        if (retrieveBuildsResponse.TryExtractSuccess(out var builds) is false)
        {
            this.logger.LogError("Unexpected error occurred");
            return false;
        }

        this.buildTemplateManager.ClearBuilds();
        var compiledBuilds = this.buildsCache?.Select(buildFile =>
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

    public async Task<Result<bool, Exception>> DownloadBuild(string buildName)
    {
        var retrieveBuildsResponse = await this.RetrieveBuildsList();
        if (retrieveBuildsResponse.TryExtractFailure(out var failure))
        {
            this.logger.LogError(failure, "Unable to download builds");
            return false;
        }

        if (retrieveBuildsResponse.TryExtractSuccess(out var builds) is false)
        {
            this.logger.LogError("Unexpected error occurred");
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

    public async Task<Result<bool, Exception>> UploadBuild(string buildName)
    {
        var getBuildResult = await this.buildTemplateManager.GetBuild(buildName);
        if (getBuildResult.TryExtractSuccess(out var buildEntry) is false)
        {
            return getBuildResult.SwitchAny(onFailure: exception => exception)!;
        }

        return await this.PutBuild(buildEntry!);
    }

    public async Task<Result<IEnumerable<BuildFile>, Exception>> RetrieveBuildsList()
    {
        var maybeBuildsBackup = await this.GetBuildsBackup();
        var buildsBackup = maybeBuildsBackup.ExtractValue();
        this.buildsCache = buildsBackup;
        return buildsBackup!;
    }

    public async Task<Result<bool, Exception>> UploadSettings(string settings, CancellationToken cancellationToken)
    {
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return new InvalidOperationException("Client is not authorized");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var stringContent = new StringContent(settings);
            var response = await this.httpClient.PutAsync(SettingsSyncFileUri, stringContent, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Failed to upload settings");
            return e;
        }
    }

    public async Task<Result<string, Exception>> DownloadSettings(CancellationToken cancellationToken)
    {
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return new InvalidOperationException("Client is not authorized");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var fileItemResponse = await this.httpClient.GetAsync(SettingsSyncFileUri, cancellationToken);
            
            if (fileItemResponse.IsSuccessStatusCode is false)
            {
                return string.Empty;
            }

            var driveItemContent = await fileItemResponse.Content.ReadAsStringAsync(cancellationToken);
            return driveItemContent;
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Failed to download settings");
            return e;
        }
    }

    public Task<Result<bool, Exception>> LogOut()
    {
        this.ResetAuthorization();
        return Task.FromResult(Result<bool, Exception>.Success(true));
    }

    public void ResetAuthorization()
    {
        //this.liveUpdateableOptions.Value.ProtectedGraphAccessToken = null;
        //this.liveUpdateableOptions.Value.ProtectedGraphRefreshToken = null;
        //this.liveUpdateableOptions.UpdateOption();
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
            using var stringContent = new StringContent(JsonConvert.SerializeObject(buildList));
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
            using var stringContent = new StringContent(JsonConvert.SerializeObject(buildList));
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

    private async Task<Optional<List<BuildFile>>> GetBuildsBackup()
    {
        try
        {
            var accessToken = await this.GetValidAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                return Optional.None<List<BuildFile>>();
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var fileItemResponse = await this.httpClient.GetAsync(BuildsSyncFileUri);
            
            if (fileItemResponse.IsSuccessStatusCode is false)
            {
                return Optional.None<List<BuildFile>>();
            }

            var driveItemContent = await fileItemResponse.Content.ReadAsStringAsync();
            var backup = JsonConvert.DeserializeObject<List<BuildFile>>(driveItemContent);
            
            if (backup is null)
            {
                return Optional.None<List<BuildFile>>();
            }

            return backup;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to get builds backup");
            return Optional.None<List<BuildFile>>();
        }
    }
}

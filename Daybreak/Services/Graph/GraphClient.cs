using Daybreak.Configuration;
using Daybreak.Configuration.Options;
using Daybreak.Controls;
using Daybreak.Services.Graph.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls;
using Convert = System.Convert;

namespace Daybreak.Services.Graph;

internal sealed class GraphClient : IGraphClient
{
    private const int MaxBrowserInitializationRetries = 5;

    private const string Scopes = "Files.Read Files.Read.All Files.ReadWrite Files.ReadWrite.All User.Read offline_access";
    private const string RedirectUri = "http://localhost:42111";

    private const string QueryStateKey = "state";
    private const string QueryCodeKey = "code";
    private const string ClientIdPlaceholder = "[ClientID]";
    private const string RedirectUriPlaceholder = "[RedirectUri]";
    private const string ScopesPlaceholder = "[Scopes]";
    private const string StatePlaceholder = "[State]";
    private const string ProfileEndpoint = "me";
    private const string GraphBaseUrl = "https://graph.microsoft.com/v1.0/";
    private const string TokenUrlPlaceholder = $"https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
    private const string AuthorizationUrlPlaceholder = $"https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize?client_id={ClientIdPlaceholder}&response_type=code&redirect_uri={RedirectUriPlaceholder}&response_mode=query&scope={ScopesPlaceholder}&state={StatePlaceholder}";
    private const string BuildsSyncFileUri = $"me/drive/root:/Daybreak/Builds/daybreak.json{ContentSuffix}";
    private const string SettingsSyncFileUri = $"me/drive/root:/Daybreak/Settings/daybreak.json{ContentSuffix}";
    private const string ContentSuffix = ":/content";

    private static readonly byte[] Entropy = Convert.FromBase64String("R3VpbGR3YXJz");
    private static readonly string ApplicationId = SecretManager.GetSecret(SecretKeys.AadApplicationId);

    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IViewManager viewManager;
    private readonly ILiveUpdateableOptions<SynchronizationOptions> liveUpdateableOptions;
    private readonly IHttpClient<GraphClient> httpClient;
    private readonly ILogger<GraphClient> logger;

    private List<BuildFile>? buildsCache;

    public GraphClient(
        IBuildTemplateManager buildTemplateManager,
        IViewManager viewManager,
        ILiveUpdateableOptions<SynchronizationOptions> liveUpdateableOptions,
        IHttpClient<GraphClient> httpClient,
        ILogger<GraphClient> logger)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.httpClient.BaseAddress = new Uri(GraphBaseUrl);
        this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<Result<bool, Exception>> PerformAuthorizationFlow(
        ChromiumBrowserWrapper chromiumBrowserWrapper,
        CancellationToken cancellationToken = default)
    {
        var maybeAuthCode = await RetrieveAuthorizationCode(chromiumBrowserWrapper, cancellationToken);
        if (maybeAuthCode.TryExtractSuccess(out var authCode) is false)
        {
            return maybeAuthCode.SwitchAny(onFailure: exception => exception)!;
        }

        var maybeAccessToken = await this.RetrieveAccessToken(authCode!);

        return maybeAccessToken.Switch(
            onSuccess: token =>
            {
                this.SaveTokenResponse(token);
                return true;
            },
            onFailure: exception => exception);
    }

    public Task<Result<bool, Exception>> LogOut()
    {
        this.ResetAccessToken();
        this.ResetRefreshToken();
        this.ResetAuthorization();

        //TODO: Currently revoking only one refresh token is not supported. Follow up when MS Graph implements refresh token revocation.
        return Task.FromResult(Result<bool, Exception>.Success(true));
    }

    public async Task<Result<User, Exception>> GetUserProfile<TViewType>()
        where TViewType : UserControl
    {
        var authCode = this.LoadAccessToken();
        if (authCode.ExtractValue() is not AccessToken accessToken)
        {
            this.viewManager.ShowView<GraphAuthorizationView>(new ViewRedirectContext { CallingView = typeof(TViewType) });
            return new InvalidOperationException("Client is not authorized");
        }

        if (DateTime.Now > accessToken.ExpirationDate)
        {
            var maybeToken = await this.RefreshAccessToken();
            if (maybeToken.ExtractValue() is not TokenResponse tokenResponse)
            {
                this.viewManager.ShowView<GraphAuthorizationView>(new ViewRedirectContext { CallingView = typeof(TViewType) });
                return new InvalidOperationException("Client authorization expired");
            }

            (var newAccessToken, _) = this.SaveTokenResponse(tokenResponse);
            accessToken = newAccessToken;
        }

        this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);
        var response = await this.httpClient.GetAsync(ProfileEndpoint);
        if (response.IsSuccessStatusCode is false)
        {
            this.viewManager.ShowView<GraphAuthorizationView>(new ViewRedirectContext { CallingView = typeof(TViewType) });
            return new InvalidOperationException($"Failed to load profile. Response status code [{response.StatusCode}]");
        }

        try
        {
            var profile = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            return profile!;
        }
        catch(Exception e)
        {
            return e;
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
            this.logger.LogError("Unexpected error occured");
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
            this.logger.LogError("Unexpected error occured");
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
            return getBuildResult.SwitchAny(
                onFailure: exception => exception)!;
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

    public void ResetAuthorization()
    {
        this.ResetAccessToken();
        this.ResetRefreshToken();
    }

    public async Task<Result<bool, Exception>> UploadSettings(string settings, CancellationToken cancellationToken)
    {
        using var stringContent = new StringContent(settings);
        try
        {
            var response = await this.httpClient.PutAsync(SettingsSyncFileUri, stringContent, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch(Exception e)
        {
            return e;
        }
    }

    public async Task<Result<string, Exception>> DownloadSettings(CancellationToken cancellationToken)
    {
        try
        {
            var fileItemResponse = await this.httpClient.GetAsync(SettingsSyncFileUri, cancellationToken);
            if (fileItemResponse.IsSuccessStatusCode is false)
            {
                return string.Empty;
            }

            var driveItemContent = await fileItemResponse.Content.ReadAsStringAsync(cancellationToken);
            return driveItemContent;
        }
        catch(Exception e)
        {
            return e;
        }
    }

    private async Task<Optional<TokenResponse>> RefreshAccessToken()
    {
        var maybeRefreshToken = this.LoadRefreshToken();
        if (maybeRefreshToken.ExtractValue() is not RefreshToken refreshToken)
        {
            return Optional.None<TokenResponse>();
        }

        using var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", ApplicationId },
            { "refresh_token", refreshToken.Token! },
            { "grant_type", "refresh_token" }
        });
        using var response = await this.httpClient.PostAsync(TokenUrlPlaceholder, httpContent);
        if (response.IsSuccessStatusCode is false)
        {
            return Optional.None<TokenResponse>();
        }

        return JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync())!;
    }

    private async Task<bool> PutBuild(IBuildEntry buildEntry)
    {
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
        // Remove the previous version of the build
        buildList = buildList.Where(b => b.FileName != buildEntry.Name).ToList();
        // Add new version of the build
        buildList.Add(buildFile);
        // Order by name
        buildList = [.. buildList.OrderBy(b => b.FileName)];

        using var stringContent = new StringContent(JsonConvert.SerializeObject(buildList));
        var response = await this.httpClient.PutAsync(BuildsSyncFileUri, stringContent);
        if (response.IsSuccessStatusCode is false)
        {
            return false;
        }

        this.buildsCache = buildList;
        return true;
    }

    private async Task<bool> PutBuilds(List<IBuildEntry> buildEntries)
    {
        var buildFiles = buildEntries.Select(buildEntry => new BuildFile
        {
            FileName = buildEntry.Name,
            TemplateCode = this.buildTemplateManager.EncodeTemplate(buildEntry),
            SourceUrl = buildEntry.SourceUrl
        });

        var buildList = new List<BuildFile>();
        buildList.AddRange(buildFiles);
        buildList = [.. buildList.OrderBy(b => b.FileName)];

        using var stringContent = new StringContent(JsonConvert.SerializeObject(buildList));
        var response = await this.httpClient.PutAsync(BuildsSyncFileUri, stringContent);
        if (response.IsSuccessStatusCode is false)
        {
            return false;
        }

        this.buildsCache = buildList;
        return true;
    }

    private async Task<Optional<List<BuildFile>>> GetBuildsBackup()
    {
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

    private static async Task<Result<string, Exception>> RetrieveAuthorizationCode(
        ChromiumBrowserWrapper chromiumBrowserWrapper,
        CancellationToken cancellationToken = default)
    {
        chromiumBrowserWrapper.ThrowIfNull();

        await chromiumBrowserWrapper.ReinitializeBrowser();

        var retries = 0;
        while (chromiumBrowserWrapper.WebBrowser.CoreWebView2 is null)
        {
            retries++;
            if (retries > MaxBrowserInitializationRetries)
            {
                throw new InvalidOperationException("Unable to initialize the embedded browser");
            }

            await Task.Delay(1000, cancellationToken);
        }

        var state = GetNewState();

        chromiumBrowserWrapper.Address = AuthorizationUrlPlaceholder
            .Replace(ClientIdPlaceholder, ApplicationId)
            .Replace(RedirectUriPlaceholder, RedirectUri
                .Replace(":", "%3a")
                .Replace("/", "%2f"))
            .Replace(ScopesPlaceholder, Scopes
                .Replace(' ', '+'))
            .Replace(StatePlaceholder, state);

        NameValueCollection query = default!;
        bool finished = false;
        chromiumBrowserWrapper.WebBrowser.CoreWebView2.SourceChanged += (_, sourceArgs) =>
        {
            if (chromiumBrowserWrapper.Address.StartsWith(RedirectUri))
            {
                query = HttpUtility.ParseQueryString(chromiumBrowserWrapper.Address.Split('?').Skip(1).FirstOrDefault()!);
                finished = true;
                chromiumBrowserWrapper.WebBrowser.CoreWebView2.NavigateToString(string.Empty);
            }
        };

        while (finished is false)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new TaskCanceledException();
            }

            await Task.Delay(1000, cancellationToken).ConfigureAwait(true);
        }

        if (query?.GetValues(QueryStateKey) is string[] states is false)
        {
            throw new InvalidOperationException("Response doesn't have state key in response");
        }

        if (states.Length < 1 || states.Length > 1)
        {
            throw new InvalidOperationException("Response contains invalid state");
        }

        if (states.First() != state)
        {
            throw new InvalidOperationException("Response contains incorrect state");
        }

        if (query.GetValues(QueryCodeKey) is string[] codes is false)
        {
            throw new InvalidOperationException("Response doesn't have code key in response");
        }

        if (codes.Length < 1 || codes.Length > 1)
        {
            throw new InvalidOperationException("Response contains invalid code");
        }

        var authCode = codes.First();
        return authCode;
    }

    private async Task<Result<TokenResponse, Exception>> RetrieveAccessToken(string authorizationCode)
    {
        using var formContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "code", authorizationCode },
                    { "redirect_uri", RedirectUri },
                    { "client_id", ApplicationId },
                    { "scope", Scopes }
                });

        using var httpRequest = new HttpRequestMessage
        {
            Content = formContent,
            Method = HttpMethod.Post,
            RequestUri = new Uri(TokenUrlPlaceholder)
        };

        using var response = await this.httpClient.SendAsync(httpRequest);
        if (response.IsSuccessStatusCode is false)
        {
            return new InvalidOperationException($"Invalid access token response. Status code [{response.StatusCode}]");
        }

        return JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync())!;
    }

    private (AccessToken, RefreshToken) SaveTokenResponse(TokenResponse token)
    {
        var accessToken = AccessToken.FromTokenResponse(token);
        var refreshToken = RefreshToken.FromTokenResponse(token);
        this.SaveAccessToken(accessToken);
        this.SaveRefreshToken(refreshToken);

        return (accessToken, refreshToken);
    }

    private void ResetAccessToken()
    {
        this.liveUpdateableOptions.Value.ProtectedGraphAccessToken = null;
        this.liveUpdateableOptions.UpdateOption();
    }

    private void SaveAccessToken(AccessToken token)
    {
        var codeBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(token));
        this.liveUpdateableOptions.Value.ProtectedGraphAccessToken = Convert.ToBase64String(ProtectedData.Protect(codeBytes, Entropy, DataProtectionScope.CurrentUser));
        this.liveUpdateableOptions.UpdateOption();
    }

    private void SaveRefreshToken(RefreshToken refreshToken)
    {
        var codeBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(refreshToken));
        this.liveUpdateableOptions.Value.ProtectedGraphRefreshToken = Convert.ToBase64String(ProtectedData.Protect(codeBytes, Entropy, DataProtectionScope.CurrentUser));
        this.liveUpdateableOptions.UpdateOption();
    }

    private void ResetRefreshToken()
    {
        this.liveUpdateableOptions.Value.ProtectedGraphRefreshToken = null;
        this.liveUpdateableOptions.UpdateOption();
    }

    private Optional<AccessToken> LoadAccessToken()
    {
        var protectedCode = this.liveUpdateableOptions.Value.ProtectedGraphAccessToken;
        if (protectedCode!.IsNullOrWhiteSpace())
        {
            return Optional.None<AccessToken>();
        }

        var codeBytes = ProtectedData.Unprotect(Convert.FromBase64String(protectedCode!), Entropy, DataProtectionScope.CurrentUser);
        try
        {
            return JsonConvert.DeserializeObject<AccessToken>(Encoding.UTF8.GetString(codeBytes))!;
        }
        catch(Exception e)
        {
            this.logger.LogError(e, "Failed to load access token. Resetting access token");
            this.ResetAccessToken();
            return Optional.None<AccessToken>();
        }
    }

    private Optional<RefreshToken> LoadRefreshToken()
    {
        var protectedCode = this.liveUpdateableOptions.Value.ProtectedGraphRefreshToken;
        if (protectedCode!.IsNullOrWhiteSpace())
        {
            return Optional.None<RefreshToken>();
        }

        var codeBytes = ProtectedData.Unprotect(Convert.FromBase64String(protectedCode!), Entropy, DataProtectionScope.CurrentUser);
        try
        {
            return JsonConvert.DeserializeObject<RefreshToken>(Encoding.UTF8.GetString(codeBytes))!;
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Failed to load refresh token. Resetting refresh token");
            this.ResetAccessToken();
            return Optional.None<RefreshToken>();
        }
    }

    private static string GetNewState()
    {
        return Guid.NewGuid().ToString();
    }
}

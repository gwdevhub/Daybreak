using Daybreak.Configuration;
using Daybreak.Controls;
using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Graph.Models;
using Daybreak.Services.ViewManagement;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace Daybreak.Services.Graph;

public sealed class GraphClient : IGraphClient
{
    private const string Scopes = "Files.Read Files.Read.All Files.ReadWrite Files.ReadWrite.All User.Read";
    private const string RedirectUri = "http://localhost";

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
    private const string SyncFolderUri = $"me/drive/root:/Daybreak/Builds{SuffixPlaceholder}";
    private const string SyncFileUri = $"me/drive/root:/Daybreak/Builds/{FilenamePlaceholder}{ContentSuffix}";
    private const string ContentSuffix = ":/content";
    private const string ChildrenSuffix = ":/children";
    private const string SuffixPlaceholder = "[Suffix]";
    private const string FilenamePlaceholder = "[File]";

    private static readonly byte[] Entropy = Convert.FromBase64String("R3VpbGR3YXJz");
    private static readonly string ApplicationId = SecretManager.GetSecret(SecretKeys.AadApplicationId);

    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IViewManager viewManager;
    private readonly ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions;
    private readonly IHttpClient<GraphClient> httpClient;
    private readonly ILogger<GraphClient> logger;

    public GraphClient(
        IBuildTemplateManager buildTemplateManager,
        IViewManager viewManager,
        ILiveUpdateableOptions<ApplicationConfiguration> liveUpdateableOptions,
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
            return maybeAuthCode.SwitchAny(onFailure: exception => exception);
        }

        var maybeAccessToken = await this.RetrieveAccessToken(authCode);

        return maybeAccessToken.Switch(
            onSuccess: token =>
            {
                var accessToken = AccessToken.FromTokenResponse(token);
                this.SaveAccessToken(accessToken);
                return true;
            },
            onFailure: exception => exception);
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
            this.viewManager.ShowView<GraphAuthorizationView>(new ViewRedirectContext { CallingView = typeof(TViewType) });
            return new InvalidOperationException("Client authorization expired");
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
            return profile;
        }
        catch(Exception e)
        {
            return e;
        }
    }

    public async Task<Result<bool, Exception>> UploadBuilds()
    {
        await this.buildTemplateManager.GetBuilds()
            .ForEachAsync(async buildEntry => await this.PutFileItem(buildEntry));
        return true;
    }

    public async Task<Result<bool, Exception>> DownloadBuilds()
    {
        var builds = this.RetrieveBuildsList();

        this.buildTemplateManager.ClearBuilds();
        var compiledBuilds = await builds.Select(buildFile =>
        {
            if (this.buildTemplateManager.TryDecodeTemplate(buildFile.TemplateCode, out var build) is false)
            {
                return null;
            }

            return new BuildEntry
            {
                Build = build,
                Name = buildFile.FileName,
                PreviousName = buildFile.FileName
            };
        }).Where(entry => entry is not null).ToListAsync();
        _ = compiledBuilds.Do(this.buildTemplateManager.SaveBuild).ToList();

        return true;
    }

    public async Task<Result<bool, Exception>> DownloadBuild(string buildName)
    {
        var builds = this.RetrieveBuildsList();

        var compiledBuilds = await builds
            .Where(build => build.FileName == buildName)
            .Select(buildFile =>
        {
            if (this.buildTemplateManager.TryDecodeTemplate(buildFile.TemplateCode, out var build) is false)
            {
                return null;
            }

            return new BuildEntry
            {
                Build = build,
                Name = buildFile.FileName,
                PreviousName = buildFile.FileName
            };
        }).Where(entry => entry is not null).ToListAsync();
        _ = compiledBuilds.Do(this.buildTemplateManager.SaveBuild).ToList();

        return true;
    }

    public async Task<Result<bool, Exception>> UploadBuild(string buildName)
    {
        var getBuildResult = await this.buildTemplateManager.GetBuild(buildName);
        if (getBuildResult.TryExtractSuccess(out var buildEntry) is false)
        {
            return getBuildResult.SwitchAny(
                onFailure: exception => exception);
        }

        return await this.PutFileItem(buildEntry);
    }

    public async IAsyncEnumerable<BuildFile> RetrieveBuildsList()
    {
        var folderResult = await this.GetFolderItem();
        if (folderResult.ExtractValue() is not FolderItem folder)
        {
            folder = new FolderItem { Files = new List<FileItem>() };
        }

        var retrieveContentTasks = folder.Files
            .Where(file => file.Name.EndsWith(".txt"))
            .Select(async file =>
            {
                var response = await this.httpClient.GetAsync(file.DownloadUrl);
                if (response.IsSuccessStatusCode is false)
                {
                    return null;
                }

                return new BuildFile { FileName = file.Name.Replace(".txt", string.Empty), TemplateCode = await response.Content.ReadAsStringAsync() };
            })
            .ToList();
        
        foreach(var task in retrieveContentTasks)
        {
            yield return await task;
        }
    }

    public void ResetAuthorization()
    {
        this.ResetAccessToken();
    }

    private async Task<bool> PutFileItem(BuildEntry buildEntry)
    {
        using var stringContent = new StringContent(this.buildTemplateManager.EncodeTemplate(buildEntry.Build));
        var response = await this.httpClient.PutAsync(SyncFileUri.Replace(FilenamePlaceholder, $"{buildEntry.Name}.txt"), stringContent);
        return response.IsSuccessStatusCode;
    }

    private async Task<Optional<FolderItem>> GetFolderItem()
    {
        var response = await this.httpClient.GetAsync(SyncFolderUri.Replace(SuffixPlaceholder, ChildrenSuffix));
        if (response.IsSuccessStatusCode is false)
        {
            return Optional.None<FolderItem>();
        }

        var driveItemContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<FolderItem>(driveItemContent);
    }

    private static async Task<Result<string, Exception>> RetrieveAuthorizationCode(
        ChromiumBrowserWrapper chromiumBrowserWrapper,
        CancellationToken cancellationToken = default)
    {
        chromiumBrowserWrapper.ThrowIfNull();
        await chromiumBrowserWrapper.InitializeDefaultBrowser();

        var state = GetNewState();

        chromiumBrowserWrapper.Address = AuthorizationUrlPlaceholder
            .Replace(ClientIdPlaceholder, ApplicationId)
            .Replace(RedirectUriPlaceholder, RedirectUri
                .Replace(":", "%3a")
                .Replace("/", "%2f"))
            .Replace(ScopesPlaceholder, Scopes
                .Replace(' ', '+'))
            .Replace(StatePlaceholder, state);

        while (chromiumBrowserWrapper.Address.StartsWith(RedirectUri) is false)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new TaskCanceledException();
            }

            await Task.Delay(1000, cancellationToken).ConfigureAwait(true);
        }

        var query = HttpUtility.ParseQueryString(chromiumBrowserWrapper.Address.Split('?').Skip(1).FirstOrDefault());
        if (query.GetValues(QueryStateKey) is string[] states is false)
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

        return JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());
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

    private Optional<AccessToken> LoadAccessToken()
    {
        var protectedCode = this.liveUpdateableOptions.Value.ProtectedGraphAccessToken;
        if (protectedCode.IsNullOrWhiteSpace())
        {
            return Optional.None<AccessToken>();
        }

        var codeBytes = ProtectedData.Unprotect(Convert.FromBase64String(protectedCode), Entropy, DataProtectionScope.CurrentUser);
        try
        {
            return JsonConvert.DeserializeObject<AccessToken>(Encoding.UTF8.GetString(codeBytes));
        }
        catch(Exception e)
        {
            this.logger.LogError(e, "Failed to load access token. Resetting access token");
            this.ResetAccessToken();
            return Optional.None<AccessToken>();
        }
    }

    private static string GetNewState()
    {
        return Guid.NewGuid().ToString();
    }
}

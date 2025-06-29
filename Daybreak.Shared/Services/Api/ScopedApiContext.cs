using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;
using System.Net.Http;
using System.Net.Http.Json;

namespace Daybreak.Shared.Services.Api;
public sealed class ScopedApiContext(
    ILogger<ScopedApiContext> logger,
    IHttpClient<ScopedApiContext> httpClient,
    DaybreakAPIContext context)
{
    private const string IdentifierPlaceholder = "[IDENTIFIER]";
    private const string CodePlaceholder = "[CODE]";

    private const string GetHealthPath = "/api/v1/rest/health";
    private const string GetCharacterSelectPath = "/api/v1/rest/character-select";
    private const string PostCharacterSelectPath = $"/api/v1/rest/character-select/{IdentifierPlaceholder}";
    private const string GetMainPlayerStatePath = "/api/v1/rest/main-player/state";
    private const string GetMainPlayerQuestLogPath = "/api/v1/rest/main-player/quest-log";
    private const string GetMainPlayerInfoPath = "/api/v1/rest/main-player/info";
    private const string GetMainPlayerInstanceInfoPath = "/api/v1/rest/main-player/instance-info";
    private const string GetMainPlayerBuildPath = "/api/v1/rest/main-player/build";
    private const string GetMainPlayerBuildContextPath = "/api/v1/rest/main-player/build-context";
    private const string PostMainPlayerBuildPath = $"/api/v1/rest/main-player/build?code={CodePlaceholder}";
    private const string PartyLoadoutPath = "/api/v1/rest/party/loadout";
    private const string GetTitleInfoPath = "/api/v1/rest/main-player/title";
    private const string GetLoginInfoPath = "/api/v1/rest/login";

    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

    private readonly ILogger<ScopedApiContext> logger = logger;
    private readonly DaybreakAPIContext context = context;
    private readonly IHttpClient<ScopedApiContext> httpClient = httpClient;

    public async Task<bool> IsAvailable(CancellationToken cancellationToken)
    {
        return await this.Get(GetHealthPath, response =>
        {
            if (response.IsSuccessStatusCode)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }, cancellationToken);
    }

    public Task<CharacterSelectInformation?> GetCharacters(CancellationToken cancellationToken) => this.GetPayload<CharacterSelectInformation>(GetCharacterSelectPath, cancellationToken);

    public async Task<bool> SwitchCharacter(string characterName, CancellationToken cancellationToken)
    {
        var path = PostCharacterSelectPath.Replace(IdentifierPlaceholder, characterName);
        var result = await this.Post(path, request => new StringContent(string.Empty), cancellationToken);
        if (!result)
        {
            this.logger.LogError("Failed to switch character to {characterName}", characterName);
            return false;
        }

        return true;
    }

    public Task<MainPlayerState?> GetMainPlayerState(CancellationToken cancellationToken) => this.GetPayload<MainPlayerState>(GetMainPlayerStatePath, cancellationToken);

    public Task<QuestLogInformation?> GetMainPlayerQuestLog(CancellationToken cancellationToken) => this.GetPayload<QuestLogInformation>(GetMainPlayerQuestLogPath, cancellationToken);

    public Task<MainPlayerInformation?> GetMainPlayerInfo(CancellationToken cancellationToken) => this.GetPayload<MainPlayerInformation>(GetMainPlayerInfoPath, cancellationToken);

    public Task<InstanceInfo?> GetMainPlayerInstanceInfo(CancellationToken cancellationToken) => this.GetPayload<InstanceInfo>(GetMainPlayerInstanceInfoPath, cancellationToken);

    public Task<BuildEntry?> GetMainPlayerBuild(CancellationToken cancellationToken) => this.GetPayload<BuildEntry>(GetMainPlayerBuildPath, cancellationToken);

    public async Task<bool> PostMainPlayerBuild(string code, CancellationToken cancellationToken)
    {
        var path = PostMainPlayerBuildPath.Replace(CodePlaceholder, code);
        using var emptyContent = new StringContent(string.Empty);
        return await this.Post(path, request => emptyContent, cancellationToken);
    }

    public Task<PartyLoadout?> GetPartyLoadout(CancellationToken cancellationToken) => this.GetPayload<PartyLoadout>(PartyLoadoutPath, cancellationToken);

    public Task<TitleInfo?> GetTitleInfo(CancellationToken cancellationToken) => this.GetPayload<TitleInfo>(GetTitleInfoPath, cancellationToken);

    public Task<LoginInfo?> GetLoginInfo(CancellationToken cancellationToken) => this.GetPayload<LoginInfo>(GetLoginInfoPath, cancellationToken);

    public Task<ProcessIdResponse?> GetProcessId(CancellationToken cancellationToken) => this.GetPayload<ProcessIdResponse>(GetHealthPath, cancellationToken);

    public Task<MainPlayerBuildContext?> GetMainPlayerBuildContext(CancellationToken cancellationToken) => this.GetPayload<MainPlayerBuildContext>(GetMainPlayerBuildContextPath, cancellationToken);

    public async Task<bool> PostPartyLoadout(PartyLoadout partyLoadout, CancellationToken cancellationToken) => await this.PostPayload(PartyLoadoutPath, partyLoadout, cancellationToken);

    private async Task<bool> PostPayload<T>(string path, T payload, CancellationToken cancellationToken)
    {
        using var content = JsonContent.Create(payload);
        var result = await this.Post(path, (request) =>
        {
            request.Headers.Add("Accept", "application/json");
            return content;
        }, cancellationToken);

        return result;
    }

    private async Task<bool> Post(string path, Func<HttpRequestMessage, HttpContent> contentBuilder, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(path);
        using var timeoutCts = new CancellationTokenSource(RequestTimeout);
        using var compositeCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
        if (!Uri.TryCreate(this.context.ApiUri, path, out var uri))
        {
            scopedLogger.LogError("Failed to create URI from {baseUri}/{path}", this.context.ApiUri, path);
            return false;
        }

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = contentBuilder(request);
            using var response = await this.httpClient.SendAsync(request, compositeCts.Token);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            scopedLogger.LogError("Failed to post data to {path}: {statusCode} {reasonPhrase}", path, response.StatusCode, response.ReasonPhrase ?? string.Empty);
            return false;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to execute api request");
            return false;
        }
    }

    private async Task<T?> GetPayload<T>(string path, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(path);
        return await this.Get(path, async (response) =>
        {
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to fetch data from {path}: {statusCode} {reasonPhrase}", path, response.StatusCode, response.ReasonPhrase ?? string.Empty);
                return default;
            }

            var payload = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
            return payload;
        }, cancellationToken);
    }

    private async Task<T?> Get<T>(string path, Func<HttpResponseMessage, Task<T?>> responseBuilder, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(path);
        using var timeoutCts = new CancellationTokenSource(RequestTimeout);
        using var compositeCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
        if (!Uri.TryCreate(this.context.ApiUri, path, out var uri))
        {
            scopedLogger.LogError("Failed to create URI from {baseUri}/{path}", this.context.ApiUri, path);
            return default;
        }

        try
        {
            using var response = await this.httpClient.GetAsync(uri, compositeCts.Token);
            return await responseBuilder(response);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to execute api request");
            return default;
        }
    }
}

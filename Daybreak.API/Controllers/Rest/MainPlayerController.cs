using Daybreak.API.Services;
using Daybreak.Shared.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;
using System.Core.Extensions;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/main-player")]
[Tags("Main Player")]
public sealed class MainPlayerController(
    MainPlayerService mainPlayerService,
    ILogger<MainPlayerController> logger)
{
    private readonly MainPlayerService mainPlayerService = mainPlayerService.ThrowIfNull();
    private readonly ILogger<MainPlayerController> logger = logger.ThrowIfNull();

    [GenerateGet("state")]
    [EndpointName("GetMainPlayerState")]
    [EndpointSummary("Get the current MainPlayerState")]
    [EndpointDescription("Get the current MainPlayerState. Returns a json serialized MainPlayerState object. You can use the websocket endpoint to subscribe to MainPlayerState updates on each game thread proc")]
    [ProducesResponseType<MainPlayerState>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetMainPlayerState(CancellationToken cancellationToken)
    {
        var mainPlayerState = await this.mainPlayerService.GetMainPlayerState(cancellationToken);
        return mainPlayerState is not null ? Results.Ok(mainPlayerState) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [GenerateGet("quest-log")]
    [EndpointName("GetQuestLog")]
    [EndpointSummary("Get the current Quest Log")]
    [EndpointDescription("Get the current Quest Log. Returns a json serialized QuestLogInformation object")]
    [ProducesResponseType<QuestLogInformation>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetQuestLog(CancellationToken cancellationToken)
    {
        var questLog = await this.mainPlayerService.GetQuestLog(cancellationToken);
        return questLog is not null ? Results.Ok(questLog) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [GenerateGet("info")]
    [EndpointName("GetMainPlayerInformation")]
    [EndpointSummary("Get the current MainPlayerInformation")]
    [EndpointDescription("Get the current MainPlayerInformation. Returns a json serialized MainPlayerInformation object")]
    [ProducesResponseType<MainPlayerInformation>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetMainPlayerInformation(CancellationToken cancellationToken)
    {
        var mainPlayerInformation = await this.mainPlayerService.GetMainPlayerInformation(cancellationToken);
        return mainPlayerInformation is not null ? Results.Ok(mainPlayerInformation) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [GenerateGet("build")]
    [EndpointName("GetMainPlayerBuild")]
    [EndpointSummary("Get the current build")]
    [EndpointDescription("Get the current build. Returns a json serialized BuildEntry object")]
    [ProducesResponseType<BuildEntry>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetMainPlayerBuild(CancellationToken cancellationToken)
    {
        var build = await this.mainPlayerService.GetCurrentBuild(cancellationToken);
        return build is not null ? Results.Ok(build) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [GeneratePost("build")]
    [EndpointName("SetMainPlayerBuild")]
    [EndpointSummary("Set the current build")]
    [EndpointDescription("Set the current build. Returns 200 is the operation has succeeded")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> SetMainPlayerBuild([FromQuery(Name = "code")] string? code, CancellationToken cancellationToken)
    {
        var result = await this.mainPlayerService.SetCurrentBuild(code ?? string.Empty, cancellationToken);
        return result ? Results.Ok() : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}

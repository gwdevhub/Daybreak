using Daybreak.API.Services;
using Daybreak.Shared.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;
using System.Core.Extensions;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/main-player")]
[Tags("MainPlayer")]
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
}

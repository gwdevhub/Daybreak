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
    public async Task<IResult> GetMainPlayerState(CancellationToken cancellationToken)
    {
        var mainPlayerState = await this.mainPlayerService.GetMainPlayerState(cancellationToken);
        return Results.Ok(mainPlayerState);
    }
}

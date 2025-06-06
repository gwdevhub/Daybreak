using Daybreak.API.Services;
using Daybreak.Shared.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/party")]
public sealed class PartyController(PartyService partyService)
{
    private readonly PartyService partyService = partyService;

    [GenerateGet]
    [EndpointName("GetPartyLoadout")]
    [EndpointSummary("Get the current party loadout")]
    [EndpointDescription("Get the current party loadout. Returns a json serialized PartyLoadout object")]
    [ProducesResponseType<PartyLoadout>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetPartyLoadout(
        CancellationToken cancellationToken)
    {
        var partyLoadout = await this.partyService.GetPartyLoadout(cancellationToken);
        return partyLoadout is not null ? Results.Ok(partyLoadout) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [GeneratePost]
    [EndpointName("SetPartyLoadout")]
    [EndpointSummary("Set the current party loadout")]
    [EndpointDescription("Set the current party loadout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> SetPartyLoadout([FromBody] PartyLoadout partyLoadout, CancellationToken cancellationToken)
    {
        var result = await this.partyService.SetPartyLoadout(partyLoadout, cancellationToken);
        return result ? Results.Ok() : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}

using Daybreak.API.Services;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/party")]
[Tags("Party")]
public sealed class PartyController(PartyService partyService)
{
    private readonly PartyService partyService = partyService;

    [GenerateGet("loadout")]
    [EndpointName("GetPartyLoadout")]
    [EndpointSummary("Get the current party loadout")]
    [EndpointDescription("Get the current party loadout. Returns the encoded party loadout template code")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetPartyLoadout(
        CancellationToken cancellationToken)
    {
        var partyLoadout = await this.partyService.GetPartyLoadout(cancellationToken);
        return partyLoadout is not null ? Results.Ok(partyLoadout) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [GeneratePost("loadout")]
    [EndpointName("SetPartyLoadout")]
    [EndpointSummary("Set the current party loadout")]
    [EndpointDescription("Set the current party loadout from an encoded template code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> SetPartyLoadout([FromQuery(Name = "code")] string? code, CancellationToken cancellationToken)
    {
        var result = await this.partyService.SetPartyLoadout(code ?? string.Empty, cancellationToken);
        return result ? Results.Ok() : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}

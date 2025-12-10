using System.Core.Extensions;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Services;
using Daybreak.Shared.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/character-select")]
[Tags("Character Select")]
public sealed class CharacterSelectController(
    CharacterSelectService characterSelectService)
{
    private readonly CharacterSelectService characterSelectService = characterSelectService.ThrowIfNull();

    [GenerateGet]
    [EndpointName("GetCharacterSelectInformation")]
    [EndpointSummary("Get Character Select Information")]
    [EndpointDescription("Get the character select information for the current player. Returns a serialized CharacterSelectInformation object")]
    [ProducesResponseType<CharacterSelectInformation>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetCharacterSelectInformation(
        CancellationToken cancellationToken)
    {
        var characterSelectInfo = await this.characterSelectService.GetCharacterSelectInformation(cancellationToken);
        return characterSelectInfo is null ?
            Results.StatusCode(StatusCodes.Status503ServiceUnavailable) :
            Results.Ok(characterSelectInfo);
    }

    [GeneratePost("{identifier}")]
    [EndpointName("ChangeCharacter")]
    [EndpointSummary("Change Character by identifier")]
    [EndpointDescription("Logs out from the current character and tries to log in with the desired character. Identifier can be either uuid or character name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> ChangeCharacter(
        [FromRoute] string identifier,
        CancellationToken cancellationToken)
    {
        var isUuid = Uuid.TryParse(identifier, out _);
        var result = isUuid
            ? await this.characterSelectService.ChangeCharacterByUuid(identifier, cancellationToken)
            : await this.characterSelectService.ChangeCharacterByName(identifier, cancellationToken);
        return result ?
            Results.Ok() :
            Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
}

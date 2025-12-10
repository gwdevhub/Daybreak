using System.Core.Extensions;
using Daybreak.API.Services;
using Daybreak.Shared.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Net.Sdk.Web;

namespace Daybreak.API.Controllers.Rest;

[GenerateController("api/v1/rest/login")]
[Tags("Login")]
public sealed class LoginController(
    LoginService loginService)
{
    private readonly LoginService loginService = loginService.ThrowIfNull();

    [GenerateGet]
    [EndpointName("GetLoginInfo")]
    [EndpointSummary("Get Login Information")]
    [EndpointDescription("Get the current login information. Returns a serialized LoginInfo object")]
    [ProducesResponseType<LoginInfo>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IResult> GetLoginInfo(
        CancellationToken cancellationToken)
    {
        var loginInfo = await this.loginService.GetLoginInformation(cancellationToken);
        return loginInfo is null ?
            Results.StatusCode(StatusCodes.Status503ServiceUnavailable) :
            Results.Ok(loginInfo);
    }
}

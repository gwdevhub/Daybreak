using System;

namespace Daybreak.Services.Graph.Models;

public sealed class AccessToken
{
    public string? Token { get; set; }
    public DateTime ExpirationDate { get; set; }

    public static AccessToken FromTokenResponse(TokenResponse tokenResponse)
    {
        return new AccessToken
        {
            Token = tokenResponse.AccessToken,
            ExpirationDate = DateTime.Now + TimeSpan.FromSeconds(tokenResponse.ExpiresIn)
        };
    }
}

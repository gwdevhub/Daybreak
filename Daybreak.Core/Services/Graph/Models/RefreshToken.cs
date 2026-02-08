namespace Daybreak.Services.Graph.Models;

public sealed class RefreshToken
{
    public string? Token { get; set; }

    public static RefreshToken FromTokenResponse(TokenResponse tokenResponse)
    {
        return new RefreshToken
        {
            Token = tokenResponse.RefreshToken,
        };
    }
}

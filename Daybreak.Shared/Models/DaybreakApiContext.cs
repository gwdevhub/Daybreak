namespace Daybreak.Shared.Models;
public readonly struct DaybreakAPIContext(Uri apiUri)
{
    public readonly Uri ApiUri = apiUri;
}

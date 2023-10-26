namespace Daybreak.Services.Scanner.Models;

internal class WorldPlayerPayload : PartyPlayerPayload
{
    public string? Name { get; set; }
    public TitlePayload? Title { get; set; }
}

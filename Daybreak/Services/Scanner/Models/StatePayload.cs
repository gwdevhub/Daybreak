namespace Daybreak.Services.Scanner.Models;
internal sealed class StatePayload
{
    public uint Id { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public uint State { get; set; }
    public float Health { get; set; }
    public float Energy { get; set; }
}

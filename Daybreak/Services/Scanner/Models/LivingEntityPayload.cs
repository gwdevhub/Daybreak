namespace Daybreak.Services.Scanner.Models;

internal class LivingEntityPayload
{
    public uint Id { get; set; }
    public uint EntityState { get; set; }
    public uint EntityAllegiance { get; set; }
    public uint Level { get; set; }
    public uint NpcDefinition { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public uint PrimaryProfessionId { get; set; }
    public uint SecondaryProfessionId { get; set; }
    public uint Timer { get; set; }
    public float Health { get; set; }
    public float Energy { get; set; }
}

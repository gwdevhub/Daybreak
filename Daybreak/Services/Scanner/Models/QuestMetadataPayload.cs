namespace Daybreak.Services.Scanner.Models;

internal class QuestMetadataPayload
{
    public uint FromId { get; set; }
    public uint Id { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public uint ToId { get; set; }
}

namespace Daybreak.Services.Scanner.Models;
internal sealed class PathingTrapezoidPayload
{
    public uint Id { get; set; }
    public uint PathingMapId { get; set; }
    public float XTL { get; set; }
    public float XTR { get; set; }
    public float XBL { get; set; }
    public float XBR { get; set; }
    public float YT { get; set; }
    public float YB { get; set; }
}

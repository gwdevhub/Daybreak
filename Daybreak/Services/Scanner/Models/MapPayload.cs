namespace Daybreak.Services.Scanner.Models;

internal sealed class MapPayload
{
    public bool IsLoaded { get; set; }
    public uint Id { get; set; }
    public uint InstanceType { get; set; }
    public uint Timer { get; set; }
    public uint Campaign { get; set; }
    public uint Continent { get; set; }
    public uint Region { get; set; }
    public uint Type { get; set; }
}

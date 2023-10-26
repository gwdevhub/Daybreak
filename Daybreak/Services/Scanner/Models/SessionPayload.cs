namespace Daybreak.Services.Scanner.Models;
internal sealed class SessionPayload
{
    public uint FoesKilled { get; set; }
    public uint FoesToKill { get; set; }
    public uint MapId { get; set; }
    public uint InstanceTimer { get; set; }
    public uint InstanceType { get; set; }
}

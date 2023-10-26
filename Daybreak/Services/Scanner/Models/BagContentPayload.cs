using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;
internal sealed class BagContentPayload
{
    public uint Id { get; set; }
    public uint Slot { get; set; }
    public uint Count { get; set; }
    public List<uint>? Modifiers { get; set; }
}

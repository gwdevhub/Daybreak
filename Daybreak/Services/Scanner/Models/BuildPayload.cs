using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;

internal class BuildPayload
{
    public List<AttributePayload>? Attributes { get; set; }
    public uint Primary { get; set; }
    public uint Secondary { get; set; }
    public List<uint>? Skills { get; set; }
}

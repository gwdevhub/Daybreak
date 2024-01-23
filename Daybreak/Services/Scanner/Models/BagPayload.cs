using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;
internal class BagPayload
{
    public List<BagContentPayload>? Items { get; set; }
}

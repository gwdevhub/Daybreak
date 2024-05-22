using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;
internal sealed class CartographerPayload
{
    public Size2D? MapSize { get; set; }
    public List<uint>? CartographedAreas { get; set; }
}

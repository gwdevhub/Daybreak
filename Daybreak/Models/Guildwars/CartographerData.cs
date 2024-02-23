using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;
public sealed class CartographerData
{
    public double Width { get; set; }
    public double Height { get; set; }
    public List<uint>? Areas { get; set; }
}

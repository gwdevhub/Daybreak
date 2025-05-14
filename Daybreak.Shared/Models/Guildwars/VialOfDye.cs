using System.Collections.Generic;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class VialOfDye : ItemBase, IItemModHash, IWikiEntity, IIconUrlEntity
{
    public static readonly VialOfDye Black = new() { Id = 146, Name = "Vial of Dye [Black]", Modifiers = [0x24D00A01, 0x24E8000A], ModHash = "24D00A0124E8000A", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/4/4c/Black_Dye.png" };
    public static readonly VialOfDye Blue = new() { Id = 146, Name = "Vial of Dye [Blue]", Modifiers = [0x24D00201, 0x24E80002], ModHash = "24D0020124E80002", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/c/c9/Blue_Dye.png" };
    public static readonly VialOfDye Brown = new() { Id = 146, Name = "Vial of Dye [Brown]", Modifiers = [0x24D00701, 0x24E80007], ModHash = "24D0070124E80007", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/0/07/Brown_Dye.png" };
    public static readonly VialOfDye Gray = new() { Id = 146, Name = "Vial of Dye [Gray]", Modifiers = [0x24D00B01, 0x24E8000B], ModHash = "24D00B0124E8000B", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/b/bc/Gray_Dye.png" };
    public static readonly VialOfDye Green = new() { Id = 146, Name = "Vial of Dye [Green]", Modifiers = [0x24D00301, 0x24E80003], ModHash = "24D0030124E80003", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/b/be/Green_Dye.png" };
    public static readonly VialOfDye Orange = new() { Id = 146, Name = "Vial of Dye [Orange]", Modifiers = [0x24D00801, 0x24E80008], ModHash = "24D0080124E80008", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/3/3a/Orange_Dye.png" };
    public static readonly VialOfDye Pink = new() { Id = 146, Name = "Vial of Dye [Pink]", Modifiers = [0x24D00D01, 0x24E8000D], ModHash = "24D00D0124E8000D", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/6/6a/Pink_Dye.png" };
    public static readonly VialOfDye Purple = new() { Id = 146, Name = "Vial of Dye [Purple]", Modifiers = [0x24D00401, 0x24E80004], ModHash = "24D0040124E80004", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/9/99/Purple_Dye.png" };
    public static readonly VialOfDye Red = new() { Id = 146, Name = "Vial of Dye [Red]", Modifiers = [0x24D00501, 0x24E80005], ModHash = "24D0050124E80005", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/c/c1/Red_Dye.png" };
    public static readonly VialOfDye Silver = new() { Id = 146, Name = "Vial of Dye [Silver]", Modifiers = [0x24D00901, 0x24E80009], ModHash = "24D0090124E80009", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/9/90/Silver_Dye.png" };
    public static readonly VialOfDye White = new() { Id = 146, Name = "Vial of Dye [White]", Modifiers = [0x24D00C01, 0x24E8000C], ModHash = "24D00C0124E8000C", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/9/91/White_Dye.png" };
    public static readonly VialOfDye Yellow = new() { Id = 146, Name = "Vial of Dye [Yellow]", Modifiers = [0x24D00601, 0x24E80006], ModHash = "24D0060124E80006", WikiUrl = "https://wiki.guildwars.com/wiki/Dye", IconUrl = "https://wiki.guildwars.com/images/4/46/Yellow_Dye.png" };

    public static IReadOnlyList<VialOfDye> Vials { get; } =
    [
        Black,
        Blue,
        Brown,
        Gray,
        Green,
        Orange,
        Pink,
        Purple,
        Red,
        Silver,
        White,
        Yellow
    ];

    public required string ModHash { get; init; }
    public required string IconUrl { get; init; }
    public required string? WikiUrl { get; init; }
}

namespace Daybreak.Shared.Models.Guildwars;

public enum ItemUpgradeFlag : uint
{
    Default     = 0x00,
    Base        = 0x08,
    Major       = 0x09,
    Superior    = 0x0A,

    // Not a real flag, just used for the stat runes
    Stat        = 0x02
}
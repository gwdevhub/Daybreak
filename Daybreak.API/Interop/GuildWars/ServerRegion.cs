namespace Daybreak.API.Interop.GuildWars;

[GWCAEquivalent("ServerRegion")]
public enum ServerRegion
{
    International = -2,
    America = 0,
    Korea,
    Europe,
    China,
    Japan,
    Unknown = 0xff
}

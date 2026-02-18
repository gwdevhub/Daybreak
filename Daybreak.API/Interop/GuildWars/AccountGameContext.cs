using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0xC)]
public readonly struct AccountUnlockedCount
{
    [FieldOffset(0x0000)]
    public readonly uint Id;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x138)]
[GWCAEquivalent("AccountContext")]
public readonly struct AccountGameContext
{
    [FieldOffset(0x0000)]
    public readonly GuildWarsArray<AccountUnlockedCount> AccountUnlockedCounts;

    [FieldOffset(0x00b4)]
    public readonly GuildWarsArray<uint> UnlockedPvpHeroes;

    [FieldOffset(0x0124)]
    public readonly GuildWarsArray<uint> UnlockedAccountSkills;

    [FieldOffset(0x0134)]
    public readonly uint AccountFlags;
}

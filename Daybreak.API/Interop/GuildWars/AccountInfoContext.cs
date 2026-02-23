using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x1C)]
[GWCAEquivalent("AccountInfo")]
public readonly unsafe struct AccountInfoContext
{
    public readonly char* AccountName;
    public readonly uint Wins;
    public readonly uint Losses;
    public readonly uint Rating;
    public readonly uint QualifierPoints;
    public readonly uint Rank;
    public readonly uint TournamentRewardPoints;
}

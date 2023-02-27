using System.Runtime.InteropServices;

namespace Daybreak.Models.Guildwars;

[StructLayout(LayoutKind.Explicit)]
public readonly struct WorldContext
{
    public const int BaseOffset = 0x0528;

    [FieldOffset(0x0000)]
    public readonly uint QuestId;

    [FieldOffset(0x015C)]
    public readonly uint HardModeUnlocked;

    [FieldOffset(0x0218)]
    public readonly uint Experience;

    [FieldOffset(0x0220)]
    public readonly uint CurrentKurzick;

    [FieldOffset(0x0228)]
    public readonly uint TotalKurzick;

    [FieldOffset(0x0230)]
    public readonly uint CurrentLuxon;
    
    [FieldOffset(0x0238)]
    public readonly uint TotalLuxon;
    
    [FieldOffset(0x0240)]
    public readonly uint CurrentImperial;
    
    [FieldOffset(0x0248)]
    public readonly uint TotalImperial;
    
    [FieldOffset(0x0260)]
    public readonly uint Level;
    
    [FieldOffset(0x0268)]
    public readonly uint Morale;
    
    [FieldOffset(0x0270)]
    public readonly uint CurrentBalthazar;
    
    [FieldOffset(0x0278)]
    public readonly uint TotalBalthazar;
    
    [FieldOffset(0x0280)]
    public readonly uint CurrentSkillPoints;
    
    [FieldOffset(0x0288)]
    public readonly uint TotalSkillPoints;
    
    [FieldOffset(0x0290)]
    public readonly uint MaxKurzick;
    
    [FieldOffset(0x0294)]
    public readonly uint MaxLuxon;
    
    [FieldOffset(0x0298)]
    public readonly uint MaxBalthazar;
    
    [FieldOffset(0x029C)]
    public readonly uint MaxImperial;
    
    [FieldOffset(0x0324)]
    public readonly uint FoesKilled;
    
    [FieldOffset(0x0328)]
    public readonly uint FoesToKill;
}

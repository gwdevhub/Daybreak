using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[GWCAEquivalent("Profession")]
public enum Profession : uint
{
    None,
    Warrior,
    Ranger,
    Monk,
    Necromancer,
    Mesmer,
    Elementalist,
    Assassin,
    Ritualist,
    Paragon,
    Dervish
}

public enum ProfessionByte : byte
{
    None,
    Warrior,
    Ranger,
    Monk,
    Necromancer,
    Mesmer,
    Elementalist,
    Assassin,
    Ritualist,
    Paragon,
    Dervish
}

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x0014)]
public readonly struct ProfessionsContext
{
    public readonly uint AgentId;
    public readonly Profession CurrentPrimary;
    public readonly Profession CurrentSecondary;
    public readonly uint UnlockedProfessionsFlags;

    public bool ProfessionUnlocked(int professionId)
    {
        return (this.UnlockedProfessionsFlags & (1U << professionId)) != 0;
    }
}

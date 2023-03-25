using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Sequential)]
public readonly struct ProfessionsContext
{
    public readonly uint AgentId;

    public readonly uint CurrentPrimary;

    public readonly uint CurrentSecondary;

    public readonly uint UnlockedProfessionsFlags;

    //Ignore this field. Added so that the struct will have the proper size and be marshaled properly into the array.
    private readonly uint H0010;

    public bool ProfessionUnlocked(int professionId)
    {
        return (this.UnlockedProfessionsFlags & (1U << professionId)) != 0;
    }
}

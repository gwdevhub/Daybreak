using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

public readonly struct PathingTrapezoid
{
    public readonly uint Id;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
    public readonly int[] AdjacentPathingTrapezoids;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used for proper padding")]
    private readonly uint H0014;

    public readonly float XTL;

    public readonly float XTR;

    public readonly float YT;

    public readonly float XBL;

    public readonly float XBR;

    public readonly float YB;
}

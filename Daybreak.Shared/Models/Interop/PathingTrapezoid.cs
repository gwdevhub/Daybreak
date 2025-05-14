namespace Daybreak.Shared.Models.Interop;

public readonly struct PathingTrapezoid
{
    public readonly uint Id;

    public readonly uint AdjacentPathingTrapezoid1;

    public readonly uint AdjacentPathingTrapezoid2;

    public readonly uint AdjacentPathingTrapezoid3;

    public readonly uint AdjacentPathingTrapezoid4;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used for proper padding")]
    private readonly uint H0014;

    public readonly float XTL;

    public readonly float XTR;

    public readonly float YT;

    public readonly float XBL;

    public readonly float XBR;

    public readonly float YB;
}

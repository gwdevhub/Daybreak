namespace Daybreak.Models.Interop;

public readonly struct TitleTierContext
{
    public readonly uint Props;

    public readonly uint TierNumber;

    public readonly int TierNamePtr;

    public bool IsPercentageBased => (this.Props & 1) != 0;
}

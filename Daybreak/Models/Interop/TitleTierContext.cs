using System;

namespace Daybreak.Models.Interop;

public readonly struct TitleTierContext
{
    public readonly uint Props;

    public readonly uint TierNumber;

    public readonly IntPtr TierNamePtr;

    public bool IsPercentageBased => (this.Props & 1) != 0;
}

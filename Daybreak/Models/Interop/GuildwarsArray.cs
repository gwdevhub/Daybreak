using Daybreak.Utils;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Sequential)]
public readonly struct GuildwarsArray<T>
{
    public readonly GuildwarsPointer<uint> Buffer;

    public readonly uint Capacity;

    public readonly uint Size;

    public readonly uint Param;

    public bool IsValidArray(bool sizeCheck)
    {
        if (!this.Buffer.IsValid())
        {
            return false;
        }

        if (!sizeCheck)
        {
            return true;
        }

        if (this.Capacity == this.Size)
        {
            return true;
        }

        var expectedCapacity = MathUtils.RoundUpToTheNextHighestPowerOf2(this.Size);
        if (expectedCapacity != this.Capacity)
        {
            return false;
        }

        return true;
    }
}

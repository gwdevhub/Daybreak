using System;
using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Sequential)]
public readonly struct GuildwarsArray
{
    public readonly int Buffer;

    public readonly uint Capacity;

    public readonly uint Size;

    public readonly uint Param;
}

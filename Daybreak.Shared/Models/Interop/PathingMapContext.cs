﻿using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct PathingMapContext
{
    [FieldOffset(0x0018)]
    public readonly GuildwarsArray<PathingMap> PathingMapArray;
}

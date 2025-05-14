using System.Numerics;
using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct QuestContext
{
    [FieldOffset(0x0000)]
    public readonly uint QuestId;

    [FieldOffset(0x0014)]
    public readonly uint MapFrom;

    [FieldOffset(0x0018)]
    public readonly Vector3 Marker;

    [FieldOffset(0x0028)]
    public readonly uint MapTo;

    [FieldOffset(0x0030)]
    //Ignore this field. Added so that the struct will have the proper size and be marshaled properly into the array.
    private readonly uint HH30;
}

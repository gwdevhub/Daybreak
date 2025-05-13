using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct NpcContext
{
    [FieldOffset(0x0000)]
    public readonly uint ModelId;

    [FieldOffset(0x000C)]
    public readonly uint Sex;

    [FieldOffset(0x0010)]
    public readonly uint Flags;

    [FieldOffset(0x0014)]
    public readonly uint Primary;

    [FieldOffset(0x001C)]
    public readonly byte DefaultLevel;

    [FieldOffset(0x0020)]
    public readonly int EncName; //Ptr to enc name

    [FieldOffset(0x0028)]
    public readonly int FilesCount;

    [FieldOffset(0x002C)]
    public readonly int FilesCapacity;
}

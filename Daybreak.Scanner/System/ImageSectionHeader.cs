using System.Runtime.InteropServices;
using System.Text;

namespace Daybreak.Scanner.System;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct ImageSectionHeader
{
    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] Name;

    [FieldOffset(8)]
    public uint VirtualSize;

    [FieldOffset(12)]
    public uint VirtualAddress;

    [FieldOffset(16)]
    public uint SizeOfRawData;

    [FieldOffset(20)]
    public uint PointerToRawData;

    [FieldOffset(24)]
    public uint PointerToRelocations;

    [FieldOffset(28)] 
    public uint PointerToLinenumbers;

    [FieldOffset(32)]
    public ushort NumberOfRelocations;

    [FieldOffset(34)]
    public ushort NumberOfLinenumbers;

    [FieldOffset(36)]
    public DataSectionFlags Characteristics;

    public string Section => Encoding.ASCII.GetString(this.Name);
}

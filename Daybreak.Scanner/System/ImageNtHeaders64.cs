using System.Runtime.InteropServices;
using System.Text;

namespace Daybreak.Scanner.System;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct ImageNtHeaders64
{
    [FieldOffset(0)]
    public fixed byte Signature[4];

    [FieldOffset(4)]
    public ImageFileHeader FileHeader;

    [FieldOffset(24)]
    public ImageOptionalHeader64 OptionalHeader;

    public bool IsValid
    {
        get
        {
            var sb = new StringBuilder();
            fixed (byte* ptr = this.Signature)
            {
                for (int i = 0; i < 4; i++)
                {
                    var c = ptr[i];
                    sb.Append(c);
                }
            }

            return sb.ToString() == "PE\0\0" && this.OptionalHeader.Magic == MagicType.IMAGE_NT_OPTIONAL_HDR64_MAGIC;
        }
    }
}

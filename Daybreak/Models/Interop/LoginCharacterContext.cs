using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public readonly struct LoginCharacterContext
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
    public readonly byte[] ContextBytes;

    public string CharacterName =>
        new(Encoding.Unicode.GetString(this.ContextBytes, 4, 40).TakeWhile(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
}

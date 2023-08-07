using System.Runtime.InteropServices;

namespace Daybreak.Models.Interop;

[StructLayout(LayoutKind.Explicit)]
public readonly struct PreGameContext
{
    [FieldOffset(0x0000)]
    public readonly uint FrameId;

    [FieldOffset(0x0124)]
    public readonly uint ChosenCharacterIndex;

    [FieldOffset(0x0140)]
    public readonly uint LoginSelectionIndex;

    [FieldOffset(0x0144)]
    public readonly uint Index2;

    [FieldOffset(0x0148)]
    public readonly GuildwarsArray<LoginCharacterContext> LoginCharacters;
}

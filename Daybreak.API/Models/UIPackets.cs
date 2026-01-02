using System.Runtime.InteropServices;

namespace Daybreak.API.Models;

public static class UIPackets
{
    public enum MouseButtons
    {
        Left = 0x0,
        Middle = 0x1,
        Right = 0x2
    }

    public enum ActionState
    {
        MouseDown = 0x6,
        MouseUp = 0x7,
        MouseClick = 0x8,
        MouseDoubleClick = 0x9,
        DragRelease = 0xb,
        KeyDown = 0xe
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct LogChatMessage(string message, Channel channel)
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public readonly string Message = message;
        public readonly Channel Channel = channel;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct UIChatMessage(Channel channel, string message, Channel channel2)
    {
        public readonly Channel Channel = channel;
        [MarshalAs(UnmanagedType.LPWStr)]
        public readonly string Message = message;
        public readonly Channel Channel2 = channel2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct KeyAction(uint key, uint wParam = 0x4000, uint lParam = 0x0006)
    {
        public readonly uint Key = key;
        public readonly uint WParam = wParam;
        public readonly uint LParam = lParam;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct MouseClick(MouseButtons mouseButton, uint isDoubleClick, uint unknownTypeScreenPos)
    {
        public readonly MouseButtons MouseButton = mouseButton;
        public readonly uint IsDoubleClick = isDoubleClick;
        public readonly uint UnknownTypeScreenPos = unknownTypeScreenPos;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct MouseAction(uint frameId, uint childOffsetId, ActionState currentState, nuint wparam = 0, nuint lparam = 0)
    {
        public readonly uint FrameId = frameId;
        public readonly uint ChildOffsetId = childOffsetId;
        public readonly ActionState CurrentState = currentState;
        public readonly nuint WParam = wparam;
        public readonly nuint LParam = lparam;
    }
}

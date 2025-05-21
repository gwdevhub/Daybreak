using System.Runtime.InteropServices;

namespace Daybreak.API.Models;

public static class UIPackets
{
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
}

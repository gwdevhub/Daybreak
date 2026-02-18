using Daybreak.API.Interop;

namespace Daybreak.API.Models;

[GWCAEquivalent("UIMessage")]
public enum UIMessage : uint
{
    None = 0x0,
    InitFrame = 0x9,
    DestroyFrame = 0xb,
    KeyDown = 0x20, // wparam = UIPacket::kKeyAction*
    KeyUp = 0x22, // wparam = UIPacket::kKeyAction*
    MouseClick = 0x24, // wparam = UIPacket::kMouseClick*
    MouseClick2 = 0x31, // wparam = UIPacket::kMouseAction*
    MouseAction = 0x32, // wparam = UIPacket::kMouseAction*
    FrameMessage_QuerySelectedIndex = 0x59, // Used to query selected index in character selector
    WriteToChatLog = 0x10000000 | 0x7F, // wparam = UIPacket::kWriteToChatLog*. Triggered by the game when it wants to add a new message to chat.
    WriteToChatLogWithSender = 0x10000000 | 0x80, // wparam = UIPacket::kWriteToChatLogWithSender*. Triggered by the game when it wants to add a new message to chat.
    CheckUIState = 0x10000000 | 0x118, // lparam = uint32_t* out state (2 = char select ready)
    Logout = 0x10000000 | 0x9D, // wparam = { bool unknown, bool character_select }
}

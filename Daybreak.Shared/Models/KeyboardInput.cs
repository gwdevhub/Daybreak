using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Daybreak.Models;

[StructLayout(LayoutKind.Sequential)]
public struct KeyboardInput
{
    /// <summary>
    /// A virtual-key code. The code must be a value in the range 1 to 254.
    /// </summary>
    public int VirtualCode;

    // EDT: added a conversion from VirtualCode to Keys.
    /// <summary>
    /// The VirtualCode converted to typeof(Keys) for higher usability.
    /// </summary>
    public Keys Key { get { return (Keys)this.VirtualCode; } }

    /// <summary>
    /// A hardware scan code for the key. 
    /// </summary>
    public int HardwareScanCode;

    /// <summary>
    /// The extended-key flag, event-injected Flags, context code, and transition-state flag. This member is specified as follows. An application can use the following values to test the keystroke Flags. Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected. If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level.
    /// </summary>
    public int Flags;

    /// <summary>
    /// The time stamp stamp for this message, equivalent to what GetMessageTime would return for this message.
    /// </summary>
    public int TimeStamp;

    /// <summary>
    /// Additional information associated with the message. 
    /// </summary>
    public IntPtr AdditionalInformation;
}

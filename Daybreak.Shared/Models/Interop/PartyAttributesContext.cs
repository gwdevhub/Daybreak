﻿using System.Runtime.InteropServices;

namespace Daybreak.Shared.Models.Interop;

public readonly struct PartyAttributesContext
{
    public readonly uint AgentId;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x36)]
    public readonly AttributeContext[] Attributes;
}

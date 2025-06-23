﻿using System.Extensions;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly struct PreGameContext
{
    [FieldOffset(0x0000)]
    public readonly uint FrameId;

    [FieldOffset(0x0124)]
    public readonly uint ChosenCharacterIndex;

    [FieldOffset(0x0140)]
    public readonly uint Index1;

    [FieldOffset(0x0144)]
    public readonly uint Index2;

    [FieldOffset(0x00148)]
    public readonly GuildWarsArray<LoginCharacterContext> LoginCharacters;
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly struct LoginCharacterContext
{
    [FieldOffset(0x0004)]
    public readonly Array20Char CharacterName;
}

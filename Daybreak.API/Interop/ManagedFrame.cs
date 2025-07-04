﻿using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Interop;

public readonly struct ManagedFrame(
    WrappedPointer<Frame> frame,
    Action closeFrame) : IDisposable
{
    private readonly Action closeFrame = closeFrame;

    public static ManagedFrame Null => new(null, () => { });

    public readonly WrappedPointer<Frame> Frame = frame;

    public void Dispose()
    {
        this.closeFrame();
    }
}

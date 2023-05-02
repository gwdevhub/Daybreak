﻿using System;
using System.Windows;
using System.Windows.Interop;

namespace Daybreak.Models;

public sealed class Win32Window : System.Windows.Forms.IWin32Window
{
    public Win32Window(Window window)
    {
        this.Handle = new WindowInteropHelper(window).Handle;
    }

    public IntPtr Handle { get; init; }
}

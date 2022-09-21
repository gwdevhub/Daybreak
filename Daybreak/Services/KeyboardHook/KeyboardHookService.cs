using Daybreak.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
using System.Runtime.InteropServices;

namespace Daybreak.Services.KeyboardHook
{
    // Based on https://gist.github.com/Stasonix
    // https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application
    public sealed class KeyboardHookService : IKeyboardHookService, IDisposable
    {
        private readonly ILogger<KeyboardHookService> logger;
        private IntPtr windowsHookHandle;
        private IntPtr user32LibraryHandle;
        private NativeMethods.HookProc hookProc;

        public event EventHandler<KeyboardHookEventArgs> KeyboardPressed;

        public KeyboardHookService(
            ILogger<KeyboardHookService> logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.Setup();
        }

        public void OnStartup()
        {
        }

        public void OnClosing()
        {
            this.Dispose();
        }

        private void Setup()
        {
            this.windowsHookHandle = IntPtr.Zero;
            this.hookProc = this.LowLevelKeyboardProc; // we must keep alive _hookProc, because GC is not aware about SetWindowsHookEx behaviour.

            this.user32LibraryHandle = NativeMethods.LoadLibrary("User32");
            if (this.user32LibraryHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                this.logger.LogError($"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            this.windowsHookHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL, this.hookProc, this.user32LibraryHandle, 0);
            if (this.windowsHookHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                this.logger.LogError($"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }
        }

        private IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var handled = false;
            var wparamTyped = wParam.ToInt32();
            if (Enum.IsDefined(typeof(KeyboardState), wparamTyped))
            {
                var p = Marshal.PtrToStructure(lParam, typeof(KeyboardInput)).Cast<KeyboardInput>();
                var eventArguments = new KeyboardHookEventArgs(wparamTyped.Cast<KeyboardState>(), p);
                this.KeyboardPressed?.Invoke(this, eventArguments);
                handled = eventArguments.Handled;
            }

            return handled ? (IntPtr)1 : NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // because we can unhook only in the same thread, not in garbage collector thread
                if (this.windowsHookHandle != IntPtr.Zero)
                {
                    if (NativeMethods.UnhookWindowsHookEx(this.windowsHookHandle) is false)
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        this.logger.LogCritical($"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                    }

                    this.windowsHookHandle = IntPtr.Zero;
                    this.hookProc -= this.LowLevelKeyboardProc;
                }
            }

            if (this.user32LibraryHandle != IntPtr.Zero)
            {
                if (NativeMethods.FreeLibrary(this.user32LibraryHandle) is false) // reduces reference to library by 1.
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    this.logger.LogCritical($"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                }

                this.user32LibraryHandle = IntPtr.Zero;
            }
        }

        ~KeyboardHookService()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

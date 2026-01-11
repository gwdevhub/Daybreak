using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Keyboard;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Photino.Blazor;
using System.Runtime.InteropServices;

namespace Daybreak.Services.Keyboard;

public sealed class KeyboardHookService : IHostedService, IKeyboardHookService, IDisposable
{
    private readonly IServiceProvider serviceProvider;
    
    private nint targetWindowHandle;
    private NativeMethods.HookProc keyboardProc;
    private nint hookId = nint.Zero;

    public event EventHandler<KeyboardEventArgs>? KeyDown;
    public event EventHandler<KeyboardEventArgs>? KeyUp;

    public KeyboardHookService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.keyboardProc = this.HookCallback;
    }

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        var app = this.serviceProvider.GetRequiredService<PhotinoBlazorApp>();
        this.targetWindowHandle = app.MainWindow.WindowHandle;
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Start()
    {
        if (this.hookId != nint.Zero)
        {
            return;
        }

        using var curProcess = System.Diagnostics.Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule!;
        this.hookId = NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_KEYBOARD_LL,
            this.keyboardProc,
            NativeMethods.GetModuleHandle(curModule.ModuleName),
            0);
    }

    public void Stop()
    {
        if (this.hookId != nint.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(this.hookId);
            this.hookId = nint.Zero;
        }
    }

    private nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode >= 0 && NativeMethods.GetForegroundWindow() == this.targetWindowHandle)
        {
            var vkCode = Marshal.ReadInt32(lParam);
            var key = (VirtualKey)vkCode;

            if (wParam == NativeMethods.WM_KEYDOWN)
            {
                this.KeyDown?.Invoke(this, new KeyboardEventArgs(key));
            }
            else if (wParam == NativeMethods.WM_KEYUP)
            {
                this.KeyUp?.Invoke(this, new KeyboardEventArgs(key));
            }
        }

        return NativeMethods.CallNextHookEx(this.hookId, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        this.Stop();
    }
}

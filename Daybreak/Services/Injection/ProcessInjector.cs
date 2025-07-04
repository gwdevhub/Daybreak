﻿using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Daybreak.Services.Injection;
internal sealed class ProcessInjector(
    ILogger<ProcessInjector> logger) : IProcessInjector
{
    private readonly ILogger<ProcessInjector> logger = logger.ThrowIfNull();

    public Task<bool> Inject(Process process, string pathToDll, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() => this.InjectWithApi(process, pathToDll), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    private bool InjectWithApi(Process process, string pathToDll)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.InjectWithApi), pathToDll);
        var modulefullpath = Path.GetFullPath(pathToDll);

        if (!File.Exists(modulefullpath))
        {
            scopedLogger.LogError("Dll to inject not found");
            return false;
        }

        var hKernel32 = NativeMethods.GetModuleHandle("kernel32.dll");
        if (hKernel32 == IntPtr.Zero)
        {
            scopedLogger.LogError("Unable to get a handle of kernel32.dll");
            return false;
        }

        var hLoadLib = NativeMethods.GetProcAddress(hKernel32, "LoadLibraryW");
        if (hLoadLib == IntPtr.Zero)
        {
            scopedLogger.LogError("Unable to get the address of LoadLibraryW");
            return false;
        }

        var hStringBuffer = NativeMethods.VirtualAllocEx(process.Handle, IntPtr.Zero, new IntPtr(2 * (modulefullpath.Length + 1)),
            0x3000 /* MEM_COMMIT | MEM_RESERVE */, 0x4 /* PAGE_READWRITE */);
        if (hStringBuffer == IntPtr.Zero)
        {
            scopedLogger.LogError("Unable to allocate memory for module path");
            return false;
        }

        WriteWString(process, hStringBuffer, modulefullpath);
        if (ReadWString(process, hStringBuffer, 260) != modulefullpath)
        {
            scopedLogger.LogError("Module path string is not correct");
            return false;
        }

        var hThread = NativeMethods.CreateRemoteThread(process.Handle, IntPtr.Zero, 0, hLoadLib, hStringBuffer, 0, out _);
        if (hThread == IntPtr.Zero)
        {
            scopedLogger.LogError("Unable to create remote thread");
            return false;
        }

        var threadResult = NativeMethods.WaitForSingleObject(hThread, 30000u);
        if (threadResult is 0x102 or 0xFFFFFFFF /* WAIT_FAILED */)
        {
            scopedLogger.LogError($"Exception occurred while waiting for the remote thread. Result is {threadResult}");
            return false;
        }

        var dllResult = NativeMethods.GetExitCodeThread(hThread, out _);
        if (dllResult == 0)
        {
            scopedLogger.LogError($"Injected dll returned non-success status code {dllResult}");
            return false;
        }

        var memoryFreeResult = NativeMethods.VirtualFreeEx(process.Handle, hStringBuffer, 0, 0x8000 /* MEM_RELEASE */);
        if (!memoryFreeResult)
        {
            scopedLogger.LogError($"Failed to free dll memory");
        }

        return memoryFreeResult;
    }

    private static void WriteBytes(Process process, IntPtr address, byte[] data)
    {
        var size = data.Length;
        var buffer = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, buffer, size);

        NativeMethods.WriteProcessMemory(
            process.Handle,
            address,
            buffer,
            size,
            out _);

        Marshal.FreeHGlobal(buffer);
    }

    private static void WriteWString(Process process, IntPtr address, string data)
    {
        WriteBytes(process, address, Encoding.Unicode.GetBytes(data));
    }

    private static string ReadWString(Process process, IntPtr address, int maxsize, Encoding? encoding = null)
    {
        encoding ??= Encoding.Unicode;
        var rawbytes = ReadBytes(process, address, maxsize);
        if (rawbytes.Length == 0)
        {
            return "";
        }

        var ret = encoding.GetString(rawbytes);
        if (ret.Contains('\0'))
        {
            ret = ret[..ret.IndexOf('\0')];
        }

        return ret;
    }

    private static byte[] ReadBytes(Process process, IntPtr address, int size)
    {
        var buffer = Marshal.AllocHGlobal(size);

        NativeMethods.ReadProcessMemory(process.Handle,
            address,
            buffer,
            size,
            out _
        );

        var ret = new byte[size];
        Marshal.Copy(buffer, ret, 0, size);
        Marshal.FreeHGlobal(buffer);

        return ret;
    }
}

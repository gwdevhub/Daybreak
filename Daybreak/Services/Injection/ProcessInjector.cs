using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Text;

namespace Daybreak.Services.Injection;
internal sealed class ProcessInjector : IProcessInjector
{
    private readonly ILogger<ProcessInjector> logger;
    
    public ProcessInjector(
        ILogger<ProcessInjector> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public bool Inject(Process process, string pathToDll)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.Inject), pathToDll);
        scopedLogger.LogInformation("Getting process handle");
        var processHandle = NativeMethods.OpenProcess(
            NativeMethods.ProcessAccessFlags.CreateThread |
            NativeMethods.ProcessAccessFlags.QueryInformation |
            NativeMethods.ProcessAccessFlags.VMOperation |
            NativeMethods.ProcessAccessFlags.VMRead |
            NativeMethods.ProcessAccessFlags.VMWrite,
            false,
            (uint)process.Id);

        if (processHandle == IntPtr.Zero)
        {
            return false;
        }

        scopedLogger.LogInformation("Allocating address for dll path");
        var dwSize = pathToDll.Length + 1;
        var allocatedAddress = NativeMethods.VirtualAllocEx(
            processHandle,
            IntPtr.Zero,
            (uint)dwSize,
            NativeMethods.MemoryAllocationType.MEM_COMMIT | NativeMethods.MemoryAllocationType.MEM_RESERVE,
            NativeMethods.MemoryProtection.PAGE_READWRITE);
        if (allocatedAddress == IntPtr.Zero)
        {
            return false;
        }

        scopedLogger.LogInformation("Writing dll path to process memory");
        var stringBytes = Encoding.Default.GetBytes(pathToDll);
        if (!NativeMethods.WriteProcessMemory(processHandle, allocatedAddress, stringBytes, (uint)dwSize, out var bytesWritten))
        {
            return false;
        }

        scopedLogger.LogInformation("Getting kernel32.dll handle");
        var kernel32Handle = NativeMethods.GetModuleHandle("kernel32.dll");
        scopedLogger.LogInformation("Getting address of LoadLibraryA");
        var loadLibraryA = NativeMethods.GetProcAddress(kernel32Handle, "LoadLibraryA");
        scopedLogger.LogInformation("Creating remote thread");
        var threadHandle = NativeMethods.CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryA, allocatedAddress, 0, out _);
        scopedLogger.LogInformation("Cleaning up allocated memory");
        NativeMethods.VirtualFreeEx(processHandle, IntPtr.Zero, dwSize, NativeMethods.MemoryAllocationType.MEM_RESET);
        scopedLogger.LogInformation("Closing process handle");
        NativeMethods.CloseHandle(processHandle);

        return true;
    }
}

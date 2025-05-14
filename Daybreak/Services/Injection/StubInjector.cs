using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using Reloaded.Assembler;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions.Core;
using System.Text;

namespace Daybreak.Services.Injection;
public sealed class StubInjector(
    ILogger<StubInjector> logger) : IStubInjector
{
    const string ListenerEntryPoint = "ThreadInit";

    /// <summary>
    /// ebp+8 is the dll path pointer
    /// 0xDEADBEEF is a placeholder to be patched later for LoadLibraryA
    /// 0xFEEDF00D is a placeholder to be patched later for GetProcAddress
    /// </summary>
    const string Asm = @"
use32
push ebp
mov  ebp, esp

mov  esi, [ebp+8]              ; ESI = &INJECT_DATA  (kept unchanged)

; -------- LoadLibraryA(dllPath) ------------------------------
push dword [esi]               ; dllPath
mov  eax, 0xDEADBEEF           ; patched → LoadLibraryA
call eax                       ; EAX = hModule

; -------- GetProcAddress(hModule, funcName) ------------------
push dword [esi+4]             ; funcName
push eax                       ; hModule  (still in EAX)
mov  eax, 0xFEEDF00D           ; patched → GetProcAddress
call eax                       ; EAX = exported EntryPoint

; -------- call EntryPoint() ----------------------------------
call eax                       ; calls EntryPoint
; xor  eax, eax                  ; thread exit-code 0

; -------- return EntryPoint() response -----------------------
leave                        ; = mov esp, ebp / pop ebp
ret  4                      ; stdcall: pop lpParameter
";

    private readonly ILogger<StubInjector> logger = logger.ThrowIfNull();

    public bool Inject(Process target, string dllPath, out int exitCode)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: dllPath);
        var hProcess = NativeMethods.OpenProcess(
            NativeMethods.ProcessAccessFlags.All, false, (uint)target.Id);

        exitCode = 0;
        if (hProcess is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not open process by id {processId}", target.Id);
            return false;
        }

        using var assembler = new Assembler();
        var stubBytes = assembler.Assemble(Asm);

        // patch placeholders
        var hKernel = NativeMethods.GetModuleHandle("kernel32.dll");
        if (hKernel is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not get handle of kernel32.dll");
            return false;
        }

        var pLoadLibA = NativeMethods.GetProcAddress(hKernel, "LoadLibraryA");
        if (pLoadLibA is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not get address of LoadLibraryA");
            return false;
        }

        var pGetProc = NativeMethods.GetProcAddress(hKernel, "GetProcAddress");
        if (pGetProc is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not get address of GetProcAddress");
            return false;
        }

        Patch(stubBytes, 0xDEADBEEF, pLoadLibA);
        Patch(stubBytes, 0xFEEDF00D, pGetProc);

        var dllBytes = Encoding.ASCII.GetBytes(dllPath + '\0');
        var funcBytes = Encoding.ASCII.GetBytes(ListenerEntryPoint + '\0');
        var remoteDll = AllocWrite(hProcess, dllBytes, NativeMethods.MemoryProtection.PAGE_READWRITE);
        if (remoteDll is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not allocate memory for dll path");
            return false;
        }

        var remoteFunc = AllocWrite(hProcess, funcBytes, NativeMethods.MemoryProtection.PAGE_READWRITE);
        if (remoteFunc is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not allocate memory for function name");
            return false;
        }

        var injectData = new byte[IntPtr.Size * 2];
        BitConverter.GetBytes(remoteDll.ToInt32()).CopyTo(injectData, 0);
        BitConverter.GetBytes(remoteFunc.ToInt32()).CopyTo(injectData, 4);
        var remoteStruct = AllocWrite(hProcess, injectData, NativeMethods.MemoryProtection.PAGE_READWRITE);
        if (remoteStruct is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not allocate memory for injectData");
            return false;
        }

        var remoteStub = AllocWrite(hProcess, stubBytes, NativeMethods.MemoryProtection.PAGE_EXECUTE_READ_WRITE);
        if (remoteStub is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not allocate memory for stub");
            return false;
        }

        var hThread = NativeMethods.CreateRemoteThread(
            hProcess, IntPtr.Zero, 0,
            remoteStub,
            remoteStruct,
            0, out _);

        if (hThread is 0)
        {
            scopedLogger.LogError("Failed to inject with stub. Could not create remote thread");
            return false;
        }

        var waitResult = NativeMethods.WaitForSingleObject(hThread, 10000); // Wait up to 10 seconds
        if (waitResult is 0) // WAIT_OBJECT_0 - thread completed
        {
            // Get the thread exit code, which will be our port number
            if (NativeMethods.GetExitCodeThread(hThread, out var moduleExitCode) > 0)
            {
                scopedLogger.LogInformation("Thread completed with result: {result}", moduleExitCode);
                exitCode = (int)moduleExitCode;
            }
            else
            {
                scopedLogger.LogWarning("Failed to get thread exit code");
            }
        }
        else
        {
            scopedLogger.LogWarning("Thread did not complete within timeout period");
        }

        return true;
    }

    private static void Patch(byte[] blob, uint marker, IntPtr value)
    {
        byte m0 = (byte)(marker & 0xFF);
        byte m1 = (byte)((marker >> 8) & 0xFF);
        byte m2 = (byte)((marker >> 16) & 0xFF);
        byte m3 = (byte)((marker >> 24) & 0xFF);

        for (int i = 0; i <= blob.Length - 4; i++)
        {
            if (blob[i] == m0 && blob[i + 1] == m1 &&
                blob[i + 2] == m2 && blob[i + 3] == m3)
            {
                BitConverter.GetBytes(value.ToInt32()).CopyTo(blob, i);
                return;
            }
        }

        throw new Exception($"Marker 0x{marker:X8} not found");
    }

    private static IntPtr AllocWrite(IntPtr hProcess, byte[] data,
                             NativeMethods.MemoryProtection protect)
    {
        var addr = NativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero,
                          (nint)data.Length,
                          (uint)(NativeMethods.AllocationType.Commit |
                          NativeMethods.AllocationType.Reserve),
                          (uint)protect);
        NativeMethods.WriteProcessMemory(hProcess, addr, data,
                                  data.Length, out _);
        return addr;
    }
}

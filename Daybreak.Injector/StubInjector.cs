using Daybreak.Shared.Models;
using System.Diagnostics;
using System.Text;

namespace Daybreak.Injector;

public static class StubInjector
{
    /// <summary>
    /// Pre-assembled x86 stub bytecode.
    /// 0xDEADBEEF is a placeholder to be patched later for LoadLibraryA.
    /// 0xFEEDF00D is a placeholder to be patched later for GetProcAddress.
    /// </summary>
    private static readonly byte[] StubTemplate =
    [
        0x55,                               // push ebp
        0x89, 0xE5,                         // mov ebp, esp
        0x8B, 0x75, 0x08,                   // mov esi, [ebp+8]
        0xFF, 0x36,                         // push dword [esi]          ; dllPath
        0xB8, 0xEF, 0xBE, 0xAD, 0xDE,       // mov eax, 0xDEADBEEF       ; -> LoadLibraryA
        0xFF, 0xD0,                         // call eax
        0xFF, 0x76, 0x04,                   // push dword [esi+4]        ; funcName
        0x50,                               // push eax                  ; hModule
        0xB8, 0x0D, 0xF0, 0xED, 0xFE,       // mov eax, 0xFEEDF00D       ; -> GetProcAddress
        0xFF, 0xD0,                         // call eax
        0xFF, 0xD0,                         // call eax                  ; EntryPoint()
        0xC9,                               // leave
        0xC2, 0x04, 0x00,                   // ret 4                     ; stdcall cleanup
    ];

    public static InjectorResponses.InjectResult Inject(Process target, string dllPath, string entryPoint, out int exitCode)
    {
        var hProcess = NativeMethods.OpenProcess(
            NativeMethods.ProcessAccessFlags.All, false, (uint)target.Id);

        exitCode = 0;
        if (hProcess is 0)
        {
            Console.WriteLine($"Failed to inject with stub. Could not open process by id {target.Id}");
            return InjectorResponses.InjectResult.InvalidProcess;
        }

        var stubBytes = (byte[])StubTemplate.Clone();

        // patch placeholders
        var hKernel = NativeMethods.GetModuleHandle("kernel32.dll");
        if (hKernel is 0)
        {
            Console.WriteLine("Failed to inject with stub. Could not get handle of kernel32.dll");
            return InjectorResponses.InjectResult.InvalidKernel32;
        }

        var pLoadLibA = NativeMethods.GetProcAddress(hKernel, "LoadLibraryA");
        if (pLoadLibA is 0)
        {
            Console.WriteLine("Failed to inject with stub. Could not get address of LoadLibraryA");
            return InjectorResponses.InjectResult.InvalidLoadLibraryA;
        }

        var pGetProc = NativeMethods.GetProcAddress(hKernel, "GetProcAddress");
        if (pGetProc is 0)
        {
            Console.WriteLine("Failed to inject with stub. Could not get address of GetProcAddress");
            return InjectorResponses.InjectResult.InvalidGetProcAddress;
        }

        Patch(stubBytes, 0xDEADBEEF, pLoadLibA);
        Patch(stubBytes, 0xFEEDF00D, pGetProc);

        var dllBytes = Encoding.ASCII.GetBytes(dllPath + '\0');
        var funcBytes = Encoding.ASCII.GetBytes(entryPoint + '\0');
        var remoteDll = AllocWrite(hProcess, dllBytes, NativeMethods.MemoryProtection.PAGE_READWRITE);
        if (remoteDll is 0)
        {
            Console.WriteLine("Failed to inject with stub. Could not allocate memory for dll path");
            return InjectorResponses.InjectResult.DllPathWriteFailed;
        }

        var remoteFunc = AllocWrite(hProcess, funcBytes, NativeMethods.MemoryProtection.PAGE_READWRITE);
        if (remoteFunc is 0)
        {
            Console.WriteLine("Failed to inject with stub. Could not allocate memory for function name");
            return InjectorResponses.InjectResult.FunctionNameWriteFailed;
        }

        var injectData = new byte[IntPtr.Size * 2];
        BitConverter.GetBytes(remoteDll.ToInt32()).CopyTo(injectData, 0);
        BitConverter.GetBytes(remoteFunc.ToInt32()).CopyTo(injectData, 4);
        var remoteStruct = AllocWrite(hProcess, injectData, NativeMethods.MemoryProtection.PAGE_READWRITE);
        if (remoteStruct is 0)
        {
            Console.WriteLine("Failed to inject with stub. Could not allocate memory for injectData");
            return InjectorResponses.InjectResult.InjectDataWriteFailed;
        }

        var remoteStub = AllocWrite(hProcess, stubBytes, NativeMethods.MemoryProtection.PAGE_EXECUTE_READ_WRITE);
        if (remoteStub is 0)
        {
            Console.WriteLine("Failed to inject with stub. Could not allocate memory for stub");
            return InjectorResponses.InjectResult.StubAllocationFailed;
        }

        var hThread = NativeMethods.CreateRemoteThread(
            hProcess, IntPtr.Zero, 0,
            remoteStub,
            remoteStruct,
            0, out _);

        if (hThread is 0)
        {
            Console.WriteLine("Failed to inject with stub. Could not create remote thread");
            return InjectorResponses.InjectResult.CreateRemoteThreadFailed;
        }

        var waitResult = NativeMethods.WaitForSingleObject(hThread, 10000); // Wait up to 10 seconds
        if (waitResult is 0) // WAIT_OBJECT_0 - thread completed
        {
            // Get the thread exit code, which will be our port number
            if (NativeMethods.GetExitCodeThread(hThread, out var moduleExitCode) > 0)
            {
                Console.WriteLine($"Thread completed with result: {moduleExitCode}");
                exitCode = (int)moduleExitCode;
            }
            else
            {
                Console.WriteLine("Failed to get thread exit code");
            }
        }
        else
        {
            Console.WriteLine("Thread did not complete within timeout period");
            return InjectorResponses.InjectResult.RemoteThreadTimeout;
        }

        return InjectorResponses.InjectResult.Success;
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

using System.Runtime.InteropServices;

namespace Daybreak.Scanner;

internal static class NativeMethods
{
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        IntPtr lpBuffer,
        int nSize,
        out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        IntPtr lpBuffer,
        int nSize,
        out IntPtr lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        out IntPtr lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    internal static extern IntPtr VirtualAllocEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        IntPtr dwSize,
        uint dwAllocationType,
        uint dwProtect);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    internal static extern bool VirtualFreeEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        uint dwFreeType);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern uint GetExitCodeThread(IntPtr hHandle, out IntPtr dwMilliseconds);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    internal static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool IsWow64Process(IntPtr hHandle, out bool isWow64Process);
}

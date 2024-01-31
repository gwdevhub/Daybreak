using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Daybreak.Injector;

internal static class NativeMethods
{
    public static uint WM_KEYDOWN = 0x0100;
    public static uint SWP_SHOWWINDOW = 0x0040;
    public static IntPtr HWND_TOPMOST = new(-1);
    public static IntPtr HWND_TOP = IntPtr.Zero;
    public const int WH_KEYBOARD_LL = 13;
    public const uint LIST_MODULES_32BIT = 0x01;

    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    internal struct ProcessEntry32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;
    };
    [StructLayout(LayoutKind.Sequential)]
    internal struct PEB
    {
        private readonly byte InheritedAddressSpace;
        private readonly byte ReadImageFileExecOptions;
        private readonly byte BeingDebugged;
        private readonly byte BitField;
        private readonly IntPtr Mutant;
        internal IntPtr ImageBaseAddress;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityAttributes
    {
        public uint nLength;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }
    [Flags]
    public enum MemoryProtection : uint
    {
        PAGE_EXECUTE = 0x10,
        PAGE_EXECUTE_READ = 0x20,
        PAGE_EXECUTE_READ_WRITE = 0x40,
        PAGE_EXECUTE_WRITECOPY = 0x80,
        PAGE_NOACCESS = 0x01,
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,
        PAGE_GUARD = 0x100,
        PAGE_NOCACHE = 0x200,
        PAGE_WRITECOMBINE = 0x400
    }
    [Flags]
    public enum MemoryAllocationType : uint
    {
        MEM_COMMIT = 0x00001000,
        MEM_RESERVE = 0x00002000,
        MEM_RESET = 0x00080000,
        MEM_RESET_UNDO = 0x1000000,
        MEM_LARGE_PAGES = 0x20000000,
        MEM_PHYSICAL = 0x00400000,
        MEM_TOP_DOWN = 0x00100000,
        MEM_WRITE_WATCH = 0x00200000
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct RECT
    {
        public readonly int Left, Top, Right, Bottom;

        public int Height => this.Bottom - this.Top;

        public int Width => this.Right - this.Left;

        public RECT(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SystemHandleInformation
    {
        public uint OwnerPID;
        public byte ObjectType;
        public byte HandleFlags;
        public ushort HandleValue;
        public UIntPtr ObjectPointer;
        public IntPtr AccessMask;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectBasicInformation
    {
        public uint Attributes;
        public uint GrantedAccess;
        public uint HandleCount;
        public uint PointerCount;
        public uint PagedPoolUsage;
        public uint NonPagedPoolUsage;
        public uint Reserved1;
        public uint Reserved2;
        public uint Reserved3;
        public uint NameInformationLength;
        public uint TypeInformationLength;
        public uint SecurityDescriptorLength;
        public FILETIME CreateTime;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct IoStatusBlock
    {
        public uint Status;
        public ulong Information;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowInfo
    {
        public uint cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;

        public WindowInfo(bool? _) : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
        {
            this.cbSize = (uint)(Marshal.SizeOf(typeof(WindowInfo)));
        }

    }
    [Flags]
    public enum DuplicateOptions : uint
    {
        DUPLICATE_CLOSE_SOURCE = 0x00000001,
        DUPLICATE_SAME_ACCESS = 0x00000002
    }
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }
    [Flags]
    public enum NtStatus : uint
    {
        STATUS_SUCCESS = 0x00000000,
        STATUS_INFO_LENGTH_MISMATCH = 0xC0000004
    }
    [Flags]
    public enum ObjectInformationClass : uint
    {
        ObjectBasicInformation = 0,
        ObjectNameInformation = 1,
        ObjectTypeInformation = 2,
        ObjectAllTypesInformation = 3,
        ObjectHandleInformation = 4
    }
    [Flags]
    public enum SystemInformationClass : uint
    {
        SystemHandleInformation = 16
    }
    [Flags]
    public enum FileInformationClass
    {
        FileNameInformation = 9
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct StartupInfo
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessInformation
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }
    public enum ProcessInfoClass : uint
    {
        ProcessBasicInformation = 0x00,
        ProcessDebugPort = 0x07,
        ProcessExceptionPort = 0x08,
        ProcessAccessToken = 0x09,
        ProcessWow64Information = 0x1A,
        ProcessImageFileName = 0x1B,
        ProcessDebugObjectHandle = 0x1E,
        ProcessDebugFlags = 0x1F,
        ProcessExecuteFlags = 0x22,
        ProcessInstrumentationCallback = 0x28,
        MaxProcessInfoClass = 0x64
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct ProcessBasicInformation
    {
        private readonly IntPtr Reserved1;
        internal IntPtr PebBaseAddress;
        private readonly IntPtr Reserved2;
        private readonly IntPtr Reserved3;
        private readonly UIntPtr UniqueProcessId;
        private readonly IntPtr Reserved4;
    }
    public enum SaferLevel : uint
    {
        Disallowed = 0,
        Untrusted = 0x1000,
        Constrained = 0x10000,
        NormalUser = 0x20000,
        FullyTrusted = 0x40000
    }
    public enum SaferLevelScope : uint
    {
        Machine = 1,
        User = 2
    }
    public enum SaferOpen : uint
    {
        Open = 1
    }
    public enum SaferTokenBehaviour : uint
    {
        Default = 0x0,
        NullIfEqual = 0x1,
        CompareOnly = 0x2,
        MakeInert = 0x4,
        WantFlags = 0x8
    }
    public enum TokenInformationClass : uint
    {
        TokenUser = 1,
        TokenGroups,
        TokenPrivileges,
        TokenOwner,
        TokenPrimaryGroup,
        TokenDefaultDacl,
        TokenSource,
        TokenType,
        TokenImpersonationLevel,
        TokenStatistics,
        TokenRestrictedSids,
        TokenSessionId,
        TokenGroupsAndPrivileges,
        TokenSessionReference,
        TokenSandBoxInert,
        TokenAuditPolicy,
        TokenOrigin,
        TokenElevationType,
        TokenLinkedToken,
        TokenElevation,
        TokenHasRestrictions,
        TokenAccessInformation,
        TokenVirtualizationAllowed,
        TokenVirtualizationEnabled,
        TokenIntegrityLevel,
        TokenUiAccess,
        TokenMandatoryPolicy,
        TokenLogonSid,
        MaxTokenInfoClass
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct TokenMandatoryLabel
    {
        public SidAndAttributes Label;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct SidAndAttributes
    {
        public IntPtr Sid;
        public int Attributes;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Sid_Identifier_Authority
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Value;
    }
    [Flags]
    public enum CreationFlags : uint
    {
        CreateSuspended = 0x00000004,
        DetachedProcess = 0x00000008,
        CreateNoWindow = 0x08000000,
        ExtendedStartupInfoPresent = 0x00080000
    }

    public const int WM_SYSCOMMAND = 0x112;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);
    [DllImport("kernel32.dll")]
    public static extern bool Process32First(IntPtr hSnapshot, ref ProcessEntry32 lppe);
    [DllImport("kernel32.dll")]
    public static extern bool Process32Next(IntPtr hSnapshot, ref ProcessEntry32 lppe);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);
    [DllImport("kernel32.dll")]
    public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, DuplicateOptions dwOptions);
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, uint dwProcessID);
    [DllImport("ntdll.dll", SetLastError = true)]
    public static extern NtStatus NtQueryInformationFile(IntPtr FileHandle, ref IoStatusBlock IoStatusBlock, IntPtr FileInformation, int FileInformationLength, FileInformationClass FileInformationClass);
    [DllImport("ntdll.dll")]
    public static extern NtStatus NtQueryObject(IntPtr ObjectHandle, ObjectInformationClass ObjectInformationClass, IntPtr ObjectInformation, int ObjectInformationLength, out int ReturnLength);
    [DllImport("ntdll.dll")]
    public static extern NtStatus NtQuerySystemInformation(SystemInformationClass SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, out int ReturnLength);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern bool QueryFullProcessImageName(IntPtr hProcess, uint dwFlags, StringBuilder lpExeName, ref uint lpdwSize);
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hwnd, IntPtr insertAfter, int x, int y, int cx, int cy, uint flags);
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int cmd);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowTextLength(IntPtr hWnd);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr LoadLibrary(string lpFileName);
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool FreeLibrary(IntPtr hModule);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(IntPtr hHook);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, uint lpBaseAddress, IntPtr lpBuffer, uint nSize, out uint lpNumberOfBytesRead);
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowInfo(IntPtr hwnd, ref WindowInfo pwi);
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    public static extern bool CreateProcess(
        string lpApplicationName, string lpCommandLine, ref SecurityAttributes lpProcessAttributes,
        ref SecurityAttributes lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags,
        IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref StartupInfo lpStartupInfo,
        out ProcessInformation lpProcessInformation);
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    public static extern uint ResumeThread(IntPtr hThread);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr LocalFree(IntPtr hMem);
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);
    [DllImport("ntdll.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern int NtQueryInformationProcess(IntPtr hProcess, ProcessInfoClass pic, out ProcessBasicInformation pbi, int cb, out int pSize);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool SaferCreateLevel(SaferLevelScope scopeId, SaferLevel levelId, SaferOpen openFlags, out IntPtr levelHandle, IntPtr reserved);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool SaferComputeTokenFromLevel(IntPtr levelHandle, IntPtr inAccessToken, out IntPtr outAccessToken, SaferTokenBehaviour flags, IntPtr lpReserved);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool SaferCloseLevel(IntPtr levelHandle);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool SetTokenInformation(IntPtr tokenHandle, TokenInformationClass tokenInformationokenInformationClass, ref TokenMandatoryLabel tokenInformation, uint tokenInformationLength);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool ConvertStringSidToSid(string stringSid, out IntPtr ptrSid);
    [DllImport("advapi32.dll")]
    public static extern uint GetLengthSid(IntPtr pSid);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool CreateProcessAsUser(
        IntPtr hToken,
        string lpApplicationName,
        string lpCommandLine,
        ref SecurityAttributes lpProcessAttributes,
        ref SecurityAttributes lpThreadAttributes,
        bool bInheritHandles,
        uint dwCreationFlags,
        IntPtr lpEnvironment,
        string lpCurrentDirectory,
        ref StartupInfo lpStartupInfo,
        out ProcessInformation lpProcessInformation);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        out IntPtr lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr VirtualAllocEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        IntPtr dwSize,
        uint dwAllocationType,
        uint dwProtect);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern bool VirtualFreeEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        uint dwFreeType);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetExitCodeThread(IntPtr hHandle, out IntPtr dwMilliseconds);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        IntPtr lpBuffer,
        int nSize,
        out IntPtr lpNumberOfBytesWritten);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        IntPtr lpBuffer,
        int nSize,
        out IntPtr lpNumberOfBytesRead);
}

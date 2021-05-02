using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Pepa.Wpf.Utilities
{
    static class NativeMethods
    {
        public static uint WM_KEYDOWN = 0x0100;
        public static uint SWP_SHOWWINDOW = 0x0040;
        public static IntPtr HWND_TOPMOST = new(-1);
        public static IntPtr HWND_TOP = IntPtr.Zero;
        public const int WH_KEYBOARD_LL = 13;

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

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

        public const int WM_SYSCOMMAND = 0x112;

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
    }
}
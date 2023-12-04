// See https://aka.ms/new-console-template for more information
using Daybreak.Injector;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

static bool InjectWithWinApi(Process process, string pathToDll)
{
    string modulefullpath = Path.GetFullPath(pathToDll);

    if (!File.Exists(modulefullpath))
    {
        Console.WriteLine("Dll to inject not found");
        return false;
    }

    nint hKernel32 = NativeMethods.GetModuleHandle("kernel32.dll");
    if (hKernel32 == IntPtr.Zero)
    {
        Console.WriteLine("Unable to get a handle of kernel32.dll");
        return false;
    }

    nint hLoadLib = NativeMethods.GetProcAddress(hKernel32, "LoadLibraryW");
    if (hLoadLib == IntPtr.Zero)
    {
        Console.WriteLine("Unable to get the address of LoadLibraryW");
        return false;
    }

    nint hStringBuffer = NativeMethods.VirtualAllocEx(process.Handle, IntPtr.Zero, new IntPtr(2 * (modulefullpath.Length + 1)),
        0x3000 /* MEM_COMMIT | MEM_RESERVE */, 0x4 /* PAGE_READWRITE */);
    if (hStringBuffer == IntPtr.Zero)
    {
        Console.WriteLine("Unable to allocate memory for module path");
        return false;
    }

    WriteWString(process, hStringBuffer, modulefullpath);
    if (ReadWString(process, hStringBuffer, 260) != modulefullpath)
    {
        Console.WriteLine("Module path string is not correct");
        return false;
    }

    nint hThread = NativeMethods.CreateRemoteThread(process.Handle, IntPtr.Zero, 0, hLoadLib, hStringBuffer, 0, out _);
    if (hThread == IntPtr.Zero)
    {
        Console.WriteLine("Unable to create remote thread");
        return false;
    }

    uint threadResult = NativeMethods.WaitForSingleObject(hThread, 30000u);
    if (threadResult is 0x102 or 0xFFFFFFFF /* WAIT_FAILED */)
    {
        Console.WriteLine($"Exception occurred while waiting for the remote thread. Result is {threadResult}");
        return false;
    }

    uint dllResult = NativeMethods.GetExitCodeThread(hThread, out _);
    if (dllResult == 0)
    {
        Console.WriteLine($"Injected dll returned non-success status code {dllResult}");
        return false;
    }

    bool memoryFreeResult = NativeMethods.VirtualFreeEx(process.Handle, hStringBuffer, 0, 0x8000 /* MEM_RELEASE */);
    if (!memoryFreeResult)
    {
        Console.WriteLine($"Failed to free dll memory");
    }

    return memoryFreeResult;
}

static void WriteBytes(Process process, IntPtr address, byte[] data)
{
    int size = data.Length;
    nint buffer = Marshal.AllocHGlobal(size);
    Marshal.Copy(data, 0, buffer, size);

    _ = NativeMethods.WriteProcessMemory(
        process.Handle,
        address,
        buffer,
        size,
        out _);

    Marshal.FreeHGlobal(buffer);
}

static void WriteWString(Process process, IntPtr address, string data)
{
    WriteBytes(process, address, Encoding.Unicode.GetBytes(data));
}

static string ReadWString(Process process, IntPtr address, int maxsize, Encoding? encoding = null)
{
    encoding ??= Encoding.Unicode;
    byte[] rawbytes = ReadBytes(process, address, maxsize);
    if (rawbytes.Length == 0)
    {
        return "";
    }

    string ret = encoding.GetString(rawbytes);
    if (ret.Contains('\0'))
    {
        ret = ret[..ret.IndexOf('\0')];
    }

    return ret;
}

static byte[] ReadBytes(Process process, IntPtr address, int size)
{
    nint buffer = Marshal.AllocHGlobal(size);

    _ = NativeMethods.ReadProcessMemory(process.Handle,
        address,
        buffer,
        size,
        out _
    );

    byte[] ret = new byte[size];
    Marshal.Copy(buffer, ret, 0, size);
    Marshal.FreeHGlobal(buffer);

    return ret;
}

Console.WriteLine("Hello, World!");
int processId = -1;
string filePath = string.Empty;
for (int i = 0; i < args.Length; i++)
{
    if (args[i] is "-p" or "--processId")
    {
        i++;
        if (!int.TryParse(args[i], out int id))
        {
            Console.WriteLine($"Unable to parce process id {args[i]}");
            return 1;
        }

        processId = id;
    }
    else if (args[i] is "-f" or "--filePath")
    {
        i++;
        filePath = args[i];
    }
    else
    {
        Console.WriteLine($"Unknown argument {args[i]} {args[(i + 1) % args.Length]}");
        return 1;
    }
}

if (processId is -1)
{
    Console.WriteLine("Must specify target process with -p [id]");
    return 1;
}

if (string.IsNullOrWhiteSpace(filePath))
{
    Console.WriteLine("Must specify dll to inject with -f [filePath]");
    return 1;
}

Process process = default!;
try
{
    process = Process.GetProcessById(processId);
}
catch
{
    Console.WriteLine($"Failed to get process with id {processId}");
    return 1;
}

if (!InjectWithWinApi(process, filePath))
{
    Console.WriteLine("Injection failed");
    return 1;
}

Console.WriteLine("Injection success");
return 0;

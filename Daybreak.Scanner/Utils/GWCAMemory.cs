using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Daybreak.Scanner.Utils;

public class GWCAMemory
{
    #region Constructor

    // Constructor
    public GWCAMemory(Process proc)
    {
        this.scan_start = IntPtr.Zero;
        this.scan_size = 0;
        this.memory_dump = null;
        this.Process = proc;
    }

    #endregion

    public Tuple<IntPtr, int> GetImageBase()
    {
        try
        {
            var name = this.Process.ProcessName;
            var modules = this.Process.Modules;
            foreach (var module in modules.OfType<ProcessModule>())
            {
                if (module.ModuleName != null &&
                    module.ModuleName.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                {
                    return new Tuple<IntPtr, int>(module.BaseAddress, module.ModuleMemorySize);
                }
            }
        }
        catch (Exception)
        {
        }

        return new Tuple<IntPtr, int>(IntPtr.Zero, 0);
    }

    public bool HaveModule(string name)
    {
        var modules = this.Process.Modules;
        foreach (var module in modules.OfType<ProcessModule>())
        {
            if (module.ModuleName != null &&
                module.ModuleName.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1)
            {
                return true;
            }
        }

        return false;
    }

    #region Basic Members

    // GwProcess we will use
    public Process Process { get; }

    // Scan variables.
    private IntPtr scan_start;
    private int scan_size;
    private byte[]? memory_dump;

    #endregion

    #region Basic Memory Functions

    /// <summary>
    ///     Read T value from memory address.
    /// </summary>
    /// <typeparam name="T">Type of value to read.</typeparam>
    /// <param name="address">Address to read from.</param>
    /// <returns>Found value.</returns>
    public T Read<T>(IntPtr address)
    {
        var size = Marshal.SizeOf(typeof(T));
        var buffer = Marshal.AllocHGlobal(size);

        NativeMethods.ReadProcessMemory(this.Process.Handle,
            address,
            buffer,
            size,
            out _
        );

        var ret = (T) Marshal.PtrToStructure(buffer, typeof(T))!;
        Marshal.FreeHGlobal(buffer);

        return ret;
    }

    /// <summary>
    ///     Read array of bytes from memory. Used in scanner.
    /// </summary>
    /// <param name="address">Address of base to read from.</param>
    /// <param name="size">Amount of bytes to read from base.</param>
    /// <returns>bytes read.</returns>
    public byte[]? ReadBytes(IntPtr address, int size)
    {
        var buffer = Marshal.AllocHGlobal(size);

        NativeMethods.ReadProcessMemory(this.Process.Handle,
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

    /// <summary>
    ///     Read a unicode string from memory.
    /// </summary>
    /// <param name="address">Address of string base.</param>
    /// <param name="maxsize">Max possible known size of string.</param>
    /// <returns>String found.</returns>
    public string ReadWString(IntPtr address, int maxsize)
    {
        var rawbytes = this.ReadBytes(address, maxsize);
        if (rawbytes == null)
        {
            return "";
        }

        var ret = Encoding.Unicode.GetString(rawbytes);
        if (ret.Contains('\0'))
        {
            ret = ret[..ret.IndexOf('\0')];
        }

        return ret;
    }

    /// <summary>
    ///     Read through and traverse a heap allocated object for specific values.
    ///     Base read first before applying offset each iteration of traversal.
    /// </summary>
    /// <typeparam name="T">Type of value to retrieve at end.</typeparam>
    /// <param name="Base">Base address to start multilevel pointer traversal.</param>
    /// <param name="offsets">Array of offsets to use to traverse to desired memory location.</param>
    /// <returns></returns>
    public T ReadPtrChain<T>(IntPtr Base, params int[] offsets)
    {
        foreach (var offset in offsets)
        {
            Base = this.Read<IntPtr>(Base) + offset;
        }

        return this.Read<T>(Base);
    }

    public void Write<T>(IntPtr address, T data)
    {
        var size = Marshal.SizeOf(typeof(T));
        var buffer = new IntPtr();
        Marshal.StructureToPtr(data!, buffer, true);

        NativeMethods.WriteProcessMemory(
            this.Process.Handle,
            address,
            buffer,
            size,
            out _);
    }

    public void WriteBytes(IntPtr address, byte[] data)
    {
        var size = data.Length;
        var buffer = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, buffer, size);

        NativeMethods.WriteProcessMemory(
            this.Process.Handle,
            address,
            buffer,
            size,
            out _);

        Marshal.FreeHGlobal(buffer);
    }

    public void WriteWString(IntPtr address, string data)
    {
        this.WriteBytes(address, Encoding.Unicode.GetBytes(data));
    }

    #endregion

    #region Memory Scanner

    /// <summary>
    ///     Initialize scanner range, dump memory block for scan.
    /// </summary>
    /// <param name="startaddr"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool InitScanner(IntPtr startaddr, int size)
    {
        if (this.Process == null)
        {
            return false;
        }

        if (this.scan_start != IntPtr.Zero)
        {
            return false;
        }

        this.scan_start = startaddr;
        this.scan_size = size;

        this.memory_dump = this.ReadBytes(startaddr, size);

        return true;
    }

    /// <summary>
    ///     Scan memory block for byte signature matches.
    /// </summary>
    /// <param name="signature">Group of bytes to match</param>
    /// <param name="offset">Offset from matched sig to pointer.</param>
    /// <returns>Address found if sucessful, IntPtr.Zero if not.</returns>
    public IntPtr ScanForPtr(byte[] signature, int offset = 0, bool readptr = false)
    {
        bool match;
        var first = signature[0];
        var sig_length = signature.Length;

        // For start to end of scan range...
        for (var scan = 0; scan < this.scan_size; ++scan)
        {
            // Skip iteration if first byte does not match
            Debug.Assert(this.memory_dump != null, nameof(this.memory_dump) + " != null");
            if (this.memory_dump[scan] != first)
            {
                continue;
            }

            match = true;

            // For sig size... check for matching signature.
            for (var sig = 0; sig < sig_length; ++sig)
            {
                if (this.memory_dump[scan + sig] != signature[sig])
                {
                    match = false;
                    break;
                }
            }

            // Add scanned address to base, plus desired offset, and read the address stored.
            if (match)
            {
                if (readptr)
                {
                    return new IntPtr(BitConverter.ToUInt32(this.memory_dump, scan + offset));
                }

                return new IntPtr(this.scan_start.ToInt32() + scan + offset);
            }
        }

        return IntPtr.Zero;
    }

    /// <summary>
    ///     Deallocate memory block and scan range of scanner.
    /// </summary>
    public void TerminateScanner()
    {
        this.scan_start = IntPtr.Zero;
        this.scan_size = 0;
        this.memory_dump = null;
    }

    #endregion

    #region Module Injection

    public enum LoadModuleResult
    {
        SUCCESSFUL,
        MODULE_NONEXISTANT,
        KERNEL32_NOT_FOUND,
        LOADLIBRARY_NOT_FOUND,
        MEMORY_NOT_ALLOCATED,
        PATH_NOT_WRITTEN,
        REMOTE_THREAD_NOT_SPAWNED,
        REMOTE_THREAD_DID_NOT_FINISH,
        MEMORY_NOT_DEALLOCATED
    }

    /// <summary>
    ///     Inject module into process using LoadLibrary CRT method.
    /// </summary>
    /// <returns>bool if injection was sucessful</returns>
    public LoadModuleResult LoadModule(string modulepath)
    {
        var modulefullpath = Path.GetFullPath(modulepath);

        if (!File.Exists(modulefullpath))
        {
            return LoadModuleResult.MODULE_NONEXISTANT;
        }

        var hKernel32 = NativeMethods.GetModuleHandle("kernel32.dll");
        if (hKernel32 == IntPtr.Zero)
        {
            return LoadModuleResult.KERNEL32_NOT_FOUND;
        }

        var hLoadLib = NativeMethods.GetProcAddress(hKernel32, "LoadLibraryW");
        if (hLoadLib == IntPtr.Zero)
        {
            return LoadModuleResult.LOADLIBRARY_NOT_FOUND;
        }

        var hStringBuffer = NativeMethods.VirtualAllocEx(this.Process.Handle, IntPtr.Zero, new IntPtr(2 * (modulefullpath.Length + 1)),
            0x3000 /* MEM_COMMIT | MEM_RESERVE */, 0x4 /* PAGE_READWRITE */);
        if (hStringBuffer == IntPtr.Zero)
        {
            return LoadModuleResult.MEMORY_NOT_ALLOCATED;
        }

        this.WriteWString(hStringBuffer, modulefullpath);
        if (this.ReadWString(hStringBuffer, 260) != modulefullpath)
        {
            return LoadModuleResult.PATH_NOT_WRITTEN;
        }

        var hThread = NativeMethods.CreateRemoteThread(this.Process.Handle, IntPtr.Zero, 0, hLoadLib, hStringBuffer, 0, out _);
        if (hThread == IntPtr.Zero)
        {
            return LoadModuleResult.REMOTE_THREAD_NOT_SPAWNED;
        }

        var threadResult = NativeMethods.WaitForSingleObject(hThread, 5000u);
        if (threadResult is 0x102 or 0xFFFFFFFF /* WAIT_FAILED */)
        {
            return LoadModuleResult.REMOTE_THREAD_DID_NOT_FINISH;
        }

        if (NativeMethods.GetExitCodeThread(hThread, out _) == 0)
        {
            return LoadModuleResult.REMOTE_THREAD_DID_NOT_FINISH;
        }

        var memoryFreeResult = NativeMethods.VirtualFreeEx(this.Process.Handle, hStringBuffer, 0, 0x8000 /* MEM_RELEASE */);
        return memoryFreeResult ? LoadModuleResult.SUCCESSFUL : LoadModuleResult.MEMORY_NOT_DEALLOCATED;
    }

    #endregion
}

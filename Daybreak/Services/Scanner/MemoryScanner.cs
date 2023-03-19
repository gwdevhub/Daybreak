using Daybreak.Models.Interop;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Daybreak.Services.Scanner;

/// <summary>
/// Memory scanner to perform memory scans over the executable.
/// Big thanks to https://github.com/GregLando113/GWCA for inspiration on how should the scanner work.
/// </summary>
public sealed class MemoryScanner : IMemoryScanner
{
    private const double MaximumReadSize = 10e8;
    private const double MaximumArraySize = 10e5;
    private static readonly object LockObject = new();

    private readonly ILogger<MemoryScanner> logger;

    private bool scanning;

    public int ModuleStartAddress { get; private set; }
    public byte[]? Memory { get; private set; }
    public int Size { get; private set; }
    public bool Scanning
    {
        get
        {
            while(Monitor.TryEnter(LockObject) is false)
            {
            }

            var value = this.scanning;
            Monitor.Exit(LockObject);

            return value;
        }
        set
        {
            while (Monitor.TryEnter(LockObject) is false)
            {
            }

            this.scanning = value;
            Monitor.Exit(LockObject);
        }
    }
    public Process? Process { get; private set; }

    public MemoryScanner(
        ILogger<MemoryScanner> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public void BeginScanner(Process process)
    {
        _ = process.ThrowIfNull();


        if (this.Scanning)
        {
            throw new InvalidOperationException("Scanner is already running");
        }

        while (Monitor.TryEnter(LockObject) is false)
        {
        }

        this.Process = process;
        (var startAddress, var size) = this.GetModuleInfo(process);
        this.ModuleStartAddress = startAddress;
        this.Size = size;
        this.Memory = this.ReadBytesNonLocking(this.ModuleStartAddress, this.Size);

        Monitor.Exit(LockObject);

        this.Scanning = true;
    }

    public void EndScanner()
    {
        if (!this.Scanning)
        {
            return;
        }

        while (Monitor.TryEnter(LockObject) is false)
        {
        }

        this.Memory = default;
        this.Size = default;
        this.ModuleStartAddress = default;
        this.Scanning = false;
        Monitor.Exit(LockObject);
    }

    public T Read<T>(int address)
    {
        this.ValidateReadScanner();
        var size = Marshal.SizeOf(typeof(T));
        var buffer = Marshal.AllocHGlobal(size);

        NativeMethods.ReadProcessMemory(this.Process!.Handle,
            address,
            buffer,
            size,
            out _
        );

        var ret = (T)Marshal.PtrToStructure(buffer, typeof(T))!;
        Marshal.FreeHGlobal(buffer);

        return ret;
    }

    public T[] ReadArray<T>(int address, int size)
    {
        this.ValidateReadScanner();
        if (size > MaximumArraySize)
        {
            throw new InvalidOperationException($"Expected size to read is too large. Array size {size}");
        }

        var itemSize = Marshal.SizeOf(typeof(T));
        var readSize = size * itemSize;
        if (readSize > MaximumReadSize)
        {
            throw new InvalidOperationException($"Expected size to read is too large. Size {readSize}");
        }

        var buffer = Marshal.AllocHGlobal(readSize);

        NativeMethods.ReadProcessMemory(this.Process!.Handle,
            address,
            buffer,
            size * itemSize,
            out _
        );

        var retArray = new T[size];
        var arrayPointer = buffer;
        for (var i = 0; i < size; i++)
        {
            retArray[i] = (T)Marshal.PtrToStructure(arrayPointer, typeof(T))!;
            arrayPointer += itemSize;
        }
       
        Marshal.FreeHGlobal(buffer);
        return retArray;
    }

    public T[] ReadArray<T>(GuildwarsArray guildwarsArray)
    {
        return this.ReadArray<T>(guildwarsArray.Buffer, (int)guildwarsArray.Size);
    }

    public byte[]? ReadBytes(int address, int size)
    {
        this.ValidateReadScanner();
        return this.ReadBytesNonLocking(address, size);
    }

    public string ReadWString(int address, int maxsize)
    {
        this.ValidateReadScanner();
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

    public T ReadPtrChain<T>(int Base, int finalPointerOffset = 0, params int[] offsets)
    {
        this.ValidateReadScanner();
        foreach (var offset in offsets)
        {
            Base = this.Read<int>(Base + offset);
        }

        return this.Read<T>(Base + finalPointerOffset);
    }

    public int ScanForPtr(byte[] pattern, string? mask = default, bool readptr = false)
    {
        this.ValidateReadScanner();
        if (pattern?.Length == 0)
        {
            throw new ArgumentException("Pattern cannot be empty");
        }

        for (var scan = 0; scan < this.Size; ++scan)
        {
            if (this.Memory![scan] != pattern![0])
            {
                continue;
            }

            var matched = true;
            for (var patternIndex = 0; patternIndex < pattern.Length; ++patternIndex)
            {
                if (mask is not null &&
                    mask.Length > patternIndex &&
                    mask[patternIndex] == '?')
                {
                    continue;
                }

                var memoryValue = this.Memory[scan + patternIndex];
                var signatureValue = pattern[patternIndex];
                if (memoryValue != signatureValue)
                {
                    matched = false;
                    break;
                }
            }

            if (matched)
            {
                if (readptr)
                {
                    return (int)BitConverter.ToUInt32(this.Memory, scan);
                }

                return this.ModuleStartAddress + scan;
            }
        }

        return 0;
    }

    public int ScanForAssertion(string? assertionFile, string? assertionMessage)
    {
        this.ValidateReadScanner();
        var mask = new StringBuilder(64);
        for (var i = 0; i < 64; i++)
        {
            mask.Append('\0');
        }

        var assertionBytes = new byte[] { 0xBA, 0x0, 0x0, 0x0, 0x0, 0xB9, 0x0, 0x0, 0x0, 0x0 };
        var assertionMask = "x????x????";
        if (assertionMessage is not null)
        {
            var assertionMessageBytes = Encoding.ASCII.GetBytes(assertionMessage);
            for (var i = 0; i < assertionMessage.Length; i++)
            {
                mask[i] = 'x';
            }

            mask[assertionMessage.Length] = 'x';
            mask[assertionMessage.Length + 1] = '\0';
            var rdataPtr = this.ScanForPtr(assertionMessageBytes, mask.ToString(), false);
            if (rdataPtr == 0)
            {
                return 0;
            }

            assertionBytes[6] = (byte)rdataPtr;
            assertionBytes[7] = (byte)(rdataPtr >> 8);
            assertionBytes[8] = (byte)(rdataPtr >> 16);
            assertionBytes[9] = (byte)(rdataPtr >> 24);
        }
        
        if (assertionFile is not null)
        {
            var assertionFileBytes = Encoding.ASCII.GetBytes(assertionFile);
            for (var i = 0; i < assertionFile.Length; i++)
            {
                mask[i] = 'x';
            }

            mask[assertionFile.Length] = 'x';
            mask[assertionFile.Length + 1] = '\0';
            var rdataPtr = this.ScanForPtr(assertionFileBytes, mask.ToString(), false);

            assertionBytes[1] = (byte)rdataPtr;
            assertionBytes[2] = (byte)(rdataPtr >> 8);
            assertionBytes[3] = (byte)(rdataPtr >> 16);
            assertionBytes[4] = (byte)(rdataPtr >> 24);
        }

        return this.ScanForPtr(assertionBytes, assertionMask, false);
    }

    private byte[]? ReadBytesNonLocking(int address, int size)
    {
        if (size > MaximumReadSize)
        {
            throw new InvalidOperationException($"Expected size to read is too large. Size {size}");
        }

        var buffer = Marshal.AllocHGlobal(size);

        NativeMethods.ReadProcessMemory(this.Process!.Handle,
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

    private void ValidateReadScanner()
    {
        if (!this.Scanning)
        {
            throw new InvalidOperationException("Scanner is not running");
        }
    }

    private (int StartAddress, int Size) GetModuleInfo(Process process)
    {
        try
        {
            var name = process.ProcessName;
            var modules = process.Modules;
            foreach (var module in modules.OfType<ProcessModule>())
            {
                if (module.ModuleName != null &&
                    module.ModuleName.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                {
                    return (module.BaseAddress.ToInt32(), module.ModuleMemorySize);
                }
            }
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Failed to get module info");
        }

        return (0, 0);
    }
}

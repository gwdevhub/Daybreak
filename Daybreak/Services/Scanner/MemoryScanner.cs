using Daybreak.Models.Interop;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

    public uint ModuleStartAddress { get; private set; }
    public byte[]? Memory { get; private set; }
    public uint Size { get; private set; }
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
        if (startAddress == 0 &&
            size == 0)
        {
            Monitor.Exit(LockObject);
            return;
        }

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

    public T Read<T>(GuildwarsPointer<T> pointer, uint offset = 0)
    {
        if (pointer is GuildwarsPointer<string>)
        {
            return (T)(this.ReadWString(pointer.Address, 256) as object);
        }

        return this.Read<T>(pointer.Address + offset);
    }

    public T Read<T>(uint address)
    {
        this.ValidateReadScanner();
        var size = Marshal.SizeOf(typeof(T));
        var buffer = Marshal.AllocHGlobal(size);

        NativeMethods.ReadProcessMemory(this.Process!.Handle,
            address,
            buffer,
            (uint)size,
            out _
        );

        var ret = (T)Marshal.PtrToStructure(buffer, typeof(T))!;
        Marshal.FreeHGlobal(buffer);

        return ret;
    }

    public T[] ReadArray<T>(uint address, uint size)
    {
        this.ValidateReadScanner();
        if (size > MaximumArraySize)
        {
            throw new InvalidOperationException($"Expected size to read is too large. Array size {size}");
        }

        var itemSize = Marshal.SizeOf(typeof(T));
        var readSize = (int)size * itemSize;
        if (readSize > MaximumReadSize)
        {
            throw new InvalidOperationException($"Expected size to read is too large. Size {readSize}");
        }

        var buffer = Marshal.AllocHGlobal((int)readSize);

        NativeMethods.ReadProcessMemory(this.Process!.Handle,
            address,
            buffer,
            (uint)readSize,
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

    public T[] ReadArray<T>(GuildwarsArray<T> guildwarsArray)
    {
        return this.ReadArray<T>(guildwarsArray.Buffer.Address, guildwarsArray.Size);
    }
    
    public T ReadItemAtIndex<T>(uint address, int index)
    {
        var itemSize = Marshal.SizeOf(typeof(T));
        var itemAtIndexAddress = address + (index * itemSize);
        return this.Read<T>((uint)itemAtIndexAddress);
    }

    public T ReadItemAtIndex<T>(GuildwarsArray<T> guildwarsArray, int index)
    {
        return this.ReadItemAtIndex<T>(guildwarsArray.Buffer.Address, index);
    }

    public T[] ReadArray<T>(GuildwarsPointerArray<T> guildwarsPointerArray)
    {
        var ptrs = this.ReadArray<uint>(guildwarsPointerArray.Buffer.Address, guildwarsPointerArray.Size);
        var retList = new List<T>();
        foreach(var ptr in ptrs)
        {
            retList.Add(this.Read<T>(ptr));
        }

        return retList.ToArray();
    }

    public byte[]? ReadBytes(uint address, uint size)
    {
        this.ValidateReadScanner();
        return this.ReadBytesNonLocking(address, size);
    }

    public string ReadWString(uint address, uint maxsize)
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

    public T ReadPtrChain<T>(uint Base, uint finalPointerOffset = 0, params uint[] offsets)
    {
        this.ValidateReadScanner();
        foreach (var offset in offsets)
        {
            Base = this.Read<uint>(Base + offset);
        }

        return this.Read<T>(Base + finalPointerOffset);
    }

    public uint ScanForPtr(byte[] pattern, string? mask = default, bool readptr = false)
    {
        this.ValidateReadScanner();
        if (pattern?.Length == 0)
        {
            throw new ArgumentException("Pattern cannot be empty");
        }

        for (var scan = 0U; scan < this.Size; ++scan)
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
                    return BitConverter.ToUInt32(this.Memory, (int)scan);
                }

                return this.ModuleStartAddress + scan;
            }
        }

        return 0;
    }

    public uint ScanForAssertion(string? assertionFile, string? assertionMessage)
    {
        this.ValidateReadScanner();
        var mask = new StringBuilder(64);
        for (var i = 0; i < 64; i++)
        {
            mask.Append('\0');
        }

        var assertionBytes = new byte[] { 0xBA, 0x0, 0x0, 0x0, 0x0, 0xB9, 0x0, 0x0, 0x0, 0x0 };
        var assertionMask = new StringBuilder("x????x????");
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
            assertionMask[6] = assertionMask[7] = assertionMask[8] = assertionMask[9] = 'x';
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
            assertionMask[1] = assertionMask[2] = assertionMask[3] = assertionMask[4] = 'x';
        }

        return this.ScanForPtr(assertionBytes, assertionMask.ToString(), false);
    }

    private byte[]? ReadBytesNonLocking(uint address, uint size)
    {
        if (size > MaximumReadSize)
        {
            throw new InvalidOperationException($"Expected size to read is too large. Size {size}");
        }

        var buffer = Marshal.AllocHGlobal((int)size);

        NativeMethods.ReadProcessMemory(this.Process!.Handle,
            address,
            buffer,
            size,
            out _
        );

        var ret = new byte[size];
        Marshal.Copy(buffer, ret, 0, (int)size);
        Marshal.FreeHGlobal(buffer);

        return ret;
    }

    private void ValidateReadScanner()
    {
        if (!this.Scanning)
        {
            throw new InvalidOperationException("Scanner is not running");
        }

        if (this.Process is null ||
            this.Process?.HasExited is true)
        {
            throw new InvalidOperationException("Process has exited");
        }
    }

    private (uint StartAddress, uint Size) GetModuleInfo(Process process)
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
                    return ((uint)module.BaseAddress.ToInt32(), (uint)module.ModuleMemorySize);
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

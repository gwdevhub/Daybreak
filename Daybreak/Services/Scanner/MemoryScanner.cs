using Daybreak.Models.Interop;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
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
    private static readonly object LockObject = new();

    private readonly ILogger<MemoryScanner> logger;

    private bool scanning;

    public IntPtr ModuleStartAddress { get; private set; }
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

    public T Read<T>(IntPtr address)
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

    public T[] ReadArray<T>(IntPtr address, int size)
    {
        this.ValidateReadScanner();
        var itemSize = Marshal.SizeOf(typeof(T));
        var buffer = Marshal.AllocHGlobal(size * itemSize);

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

    public byte[]? ReadBytes(IntPtr address, int size)
    {
        this.ValidateReadScanner();
        return this.ReadBytesNonLocking(address, size);
    }

    public string ReadWString(IntPtr address, int maxsize)
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

    public T ReadPtrChain<T>(IntPtr Base, int finalPointerOffset = 0, params int[] offsets)
    {
        this.ValidateReadScanner();
        foreach (var offset in offsets)
        {
            Base = this.Read<IntPtr>(Base + offset);
        }

        return this.Read<T>(Base + finalPointerOffset);
    }

    public IntPtr ScanForPtr(byte[] pattern, string? mask = default, bool readptr = false)
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
                    return new IntPtr(BitConverter.ToUInt32(this.Memory, scan));
                }

                return new IntPtr(this.ModuleStartAddress.ToInt32() + scan);
            }
        }

        return IntPtr.Zero;
    }

    private byte[]? ReadBytesNonLocking(IntPtr address, int size)
    {
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

    private (IntPtr StartAddress, int Size) GetModuleInfo(Process process)
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
                    return (module.BaseAddress, module.ModuleMemorySize);
                }
            }
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Failed to get module info");
        }

        return (IntPtr.Zero, 0);
    }
}

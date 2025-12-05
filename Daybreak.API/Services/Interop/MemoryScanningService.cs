using PeNet;
using PeNet.Header.Pe;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions.Core;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class MemoryScanningService(
    ILogger<MemoryScanningService> logger)
{
    private readonly ILogger<MemoryScanningService> logger = logger.ThrowIfNull();
    private readonly (nuint BaseAddress, ImageSectionHeader Section) textSection = GetSectionHeader(".text");
    private readonly (nuint BaseAddress, ImageSectionHeader Section) dataSection = GetSectionHeader(".rdata");

    public uint GameTlsIndex { get; } = GetGameTlsIndex();

    public nuint FunctionFromNearCall(nuint callInstructionAddress, bool checkValidPtr = true)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (callInstructionAddress is 0)
        {
            scopedLogger.LogError("Invalid call instruction address: 0x{address:X8}", callInstructionAddress);
            return 0;
        }

        var opcode = Marshal.ReadByte((nint)callInstructionAddress);
        var functionAddress = opcode switch
        {
            0xE8 => (nuint)((nint)callInstructionAddress + 5 + Marshal.ReadInt32((nint)(callInstructionAddress + 1))),
            0xE9 => (nuint)((nint)callInstructionAddress + 5 + Marshal.ReadInt32((nint)(callInstructionAddress + 1))),
            0xEB => (nuint)((nint)callInstructionAddress + 2 + (sbyte)Marshal.ReadByte((nint)(callInstructionAddress + 1))),
            _ => (nuint)0
        };

        if (checkValidPtr && functionAddress is 0)
        {
            scopedLogger.LogError("Invalid function address: 0x{address:X8}", functionAddress);
            return 0;
        }

        // Recursively resolve nested JMPs
        var nestedCall = this.FunctionFromNearCall(functionAddress, checkValidPtr);
        if (nestedCall != 0)
        {
            scopedLogger.LogInformation("Resolved nested call to: 0x{address:X8}", nestedCall);
            return nestedCall;
        }

        scopedLogger.LogInformation("Resolved function address: 0x{address:X8}", functionAddress);
        return functionAddress;
    }

    public nuint FindAssertion(
        string assertionFile,
        string assertionMessage,
        uint lineNumber = 0,
        int offset = 0)
    {
        Span<byte> pattern =
        [
            0x68, 0,0,0,0,      // push <line> / will be overwritten later
            0xBA, 0,0,0,0,      // mov  edx,<file> / will be overwritten later
            0xB9, 0,0,0,0       // mov  ecx,<msg > / will be overwritten later
        ];

        const string maskAll = "xxxxxxxxxxxxxxx";   // 15 × 'x'

        var msgBytes = Encoding.ASCII.GetBytes(assertionMessage + '\0');
        var msgMask = new string('x', msgBytes.Length);

        var fileOrigBytes = Encoding.ASCII.GetBytes(assertionFile + '\0');
        var fileOrigMask = new string('x', fileOrigBytes.Length);

        var (rdataBase, rdataHdr) = this.dataSection;
        var rdataStart = rdataBase + rdataHdr.VirtualAddress;
        var rdataEnd = rdataStart + rdataHdr.VirtualSize;

        nuint msgSearchPos = rdataStart;
        while (true)
        {
            var msgPtr = FindInRange(msgBytes, msgMask, 0, msgSearchPos, rdataEnd);
            if (msgPtr is 0)
            {
                break;                 // no more messages → we are done
            }

            msgSearchPos = msgPtr + 1; // next search starts just after the hit

            // write ECX operand (little-endian) into the pattern
            BitConverter.TryWriteBytes(pattern.Slice(11, 4), (uint)msgPtr);

            var fileSearchPos = rdataStart;
            while (true)
            {
                var filePtr = FindInRange(fileOrigBytes, fileOrigMask, 0, fileSearchPos, rdataEnd);

                // fallbacks: lower-case and CamelCase, exactly like the C++
                if (filePtr == 0)
                {
                    var lower = assertionFile.ToLowerInvariant();
                    var lowerBytes = Encoding.ASCII.GetBytes(lower + '\0');
                    filePtr = FindInRange(lowerBytes, new string('x', lowerBytes.Length), 0, fileSearchPos, rdataEnd);
                }

                if (filePtr == 0)
                {
                    var camel = ToCamelCase(assertionFile);
                    var camelBytes = Encoding.ASCII.GetBytes(camel + '\0');
                    filePtr = FindInRange(camelBytes, new string('x', camelBytes.Length), 0, fileSearchPos, rdataEnd);
                }

                if (filePtr == 0)
                {
                    break;                          // try next message
                }

                fileSearchPos = filePtr + 1;

                // If what we found is "…/file.hpp" (no drive letter) fix it
                if (Marshal.ReadByte((nint)(filePtr + 1)) != (byte)':')
                {
                    // look ≤128 bytes backward for ':' and back-up one char
                    nuint colon = FindInRange([(byte)':'], "x", -1, filePtr, filePtr - 128);
                    if (colon == 0)
                    {
                        continue; // failed → try next file hit
                    }

                    filePtr = colon;
                }

                // write EDX operand
                BitConverter.TryWriteBytes(pattern.Slice(6, 4), (uint)filePtr);

                nuint hit = 0;
                if (lineNumber != 0 && (lineNumber & 0xff) == lineNumber)
                {
                    // PUSH imm8 variant  –  start matching at pattern[3]
                    pattern[3] = 0x6A;           // opcode
                    pattern[4] = (byte)lineNumber;
                    hit = FindAddressInternal(
                        this.textSection,
                        pattern[3..].ToArray(),
                        maskAll[3..],
                        offset);
                }

                if (hit == 0 && lineNumber != 0)
                {
                    // PUSH imm32 variant  –  whole pattern
                    pattern[0] = 0x68;
                    BitConverter.TryWriteBytes(pattern.Slice(1, 4), lineNumber);
                    hit = FindAddressInternal(
                        this.textSection,
                        pattern.ToArray(),
                        maskAll,
                        offset);
                }

                if (hit == 0 && lineNumber == 0)
                {
                    // No line number  –  match only from MOV EDX onward
                    hit = FindAddressInternal(
                        this.textSection,
                        pattern[5..].ToArray(),
                        maskAll[5..],
                        offset);
                }

                if (hit != 0)
                {
                    return hit;           // found exactly what we wanted
                }
            }
        }

        return 0;                         // nothing matched
    }

    public nuint FindAndResolveAddress(byte[] pattern, string mask, int offset = 0)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var address = FindAddressInternal(this.textSection, pattern, mask, offset);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to find address");
            return 0;
        }

        var ptr = (nuint)Marshal.ReadIntPtr((nint)address);
        return ptr;
    }

    public nuint FindAddress(byte[] pattern, string mask, int offset = 0)
    {
        var address = FindAddressInternal(this.textSection, pattern, mask, offset);
        return address;
    }

    public nuint FindAddress(byte[] pattern, string mask, int offset, nuint startAddress, nuint endAddress)
    {
        var address = FindAddressInternal(startAddress, endAddress, pattern, mask, offset);
        return address;
    }

    public nuint ToFunctionStart(nuint callInstructionAddress, uint scanRange = 0x500)
    {
        if (callInstructionAddress == 0)
        {
            return 0;
        }

        // pattern: 55 8B EC   – mask: "xxx"
        ReadOnlySpan<byte> prologue = [0x55, 0x8B, 0xEC];
        const string mask = "xxx";

        var start = callInstructionAddress;                // begin at the CALL itself
        var end = callInstructionAddress - scanRange;    // scan backwards

        return FindInRange(prologue, mask, 0, start, end);
    }

    public nuint FindNthUseOfString(string str, uint nth, int offset = 0, bool useTextSection = true)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var strBytes = Encoding.ASCII.GetBytes(str);
        var mask = new string('x', strBytes.Length);

        var foundStr = FindAddressInternal(this.dataSection, strBytes, mask, 0);
        if (foundStr is 0)
        {
            scopedLogger.LogError("Failed to find string in .rdata section");
            return 0;
        }

        var firstNullChar = FindInRange([0x0], "x", 1, foundStr, foundStr - 0x64);
        if (firstNullChar is 0)
        {
            scopedLogger.LogError("Failed to find null terminator");
            return 0;
        }

        return this.FindNthUseOfAddress(firstNullChar, nth, offset, useTextSection);
    }

    public nuint FindUseOfString(string str, int offset = 0, bool useTextSection = true)
    {
        return this.FindNthUseOfString(str, 0, offset, useTextSection);
    }

    public nuint FindNthUseOfWideString(string str, uint nth, int offset = 0, bool useTextSection = true)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var strBytes = Encoding.Unicode.GetBytes(str);
        var mask = new string('x', strBytes.Length);

        var foundStr = FindAddressInternal(this.dataSection, strBytes, mask, 0);
        if (foundStr is 0)
        {
            scopedLogger.LogError("Failed to find wide string in .rdata section");
            return 0;
        }

        var firstNullChar = FindInRange([0x0, 0x0], "xx", 2, foundStr, foundStr - 0x128);
        if (firstNullChar is 0)
        {
            scopedLogger.LogError("Failed to find null terminator for wide string");
            return 0;
        }

        return this.FindNthUseOfAddress(firstNullChar, nth, offset, useTextSection);
    }

    public nuint FindUseOfWideString(string str, int offset = 0, bool useTextSection = true)
    {
        return this.FindNthUseOfWideString(str, 0, offset, useTextSection);
    }

    private nuint FindNthUseOfAddress(nuint address, uint nth, int offset, bool useTextSection)
    {
        var (BaseAddress, Section) = useTextSection ? this.textSection : this.dataSection;
        var addressBytes = BitConverter.GetBytes((uint)address);
        var mask = "xxxx";

        var currentAddress = BaseAddress + Section.VirtualAddress;
        var endAddress = currentAddress + Section.VirtualSize;

        uint foundCount = 0;
        while (currentAddress < endAddress)
        {
            var found = FindAddressInternal(currentAddress, endAddress, addressBytes, mask, 0);
            if (found is 0)
            {
                break;
            }

            if (foundCount == nth)
            {
                return (nuint)((nint)found + offset);
            }

            foundCount++;
            currentAddress = found + 1;
        }

        return 0;
    }

    private static bool IsValidPtr(nuint address, (nuint BaseAddress, ImageSectionHeader Section) section)
    {
        return address > 0 && 
            (ulong)address > section.Section.ImageBaseAddress &&
            (ulong)address < (section.Section.ImageBaseAddress + section.Section.VirtualSize);
    }

    private static unsafe nuint FindInRange(
        ReadOnlySpan<byte> pattern,
        string mask,
        int offset,
        nuint start,
        nuint end)
    {
        bool forward = start < end;
        int patLength = pattern.Length;

        if (forward)
        {
            end -= (uint)patLength;                 // forward scan ⇒ clamp tail
        }

        var cur = start;
        while (forward ? cur <= end : cur >= end)
        {
            if (Marshal.ReadByte((nint)cur) == pattern[0])
            {
                var matched = true;
                for (int i = 1; i < patLength && matched; ++i)
                {
                    if (mask[i] == 'x' &&
                        Marshal.ReadByte((nint)(cur + (uint)i)) != pattern[i])
                    {
                        matched = false;
                    }
                }

                if (matched)
                {
                    return (nuint)((long)cur + offset);
                }
            }

            cur = forward ? cur + 1 : cur - 1;
            if (!forward && cur == 0)               // prevent unsigned wrap-around
            {
                break;
            }
        }

        return 0;
    }

    private static string ToCamelCase(string text)
        => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text.ToLowerInvariant());

    private static nuint FindAddressInternal((nuint BaseAddress, ImageSectionHeader Section) section, byte[] pattern, string mask, int offset = 0)
    {
        var textStart = section.BaseAddress + section.Section.VirtualAddress;
        var textEnd = textStart + section.Section.VirtualSize - (uint)pattern.Length;
        return FindAddressInternal(textStart, textEnd, pattern, mask, offset);
    }

    private static nuint FindAddressInternal(nuint startAddr, nuint endAddr, byte[] pattern, string mask, int offset = 0)
    {
        for (var cur = startAddr; cur <= endAddr; ++cur)
        {
            if (Marshal.ReadByte((nint)cur) != pattern[0])
            {
                continue;
            }

            var match = true;
            for (var i = 1; i < pattern.Length && match; ++i)
            {
                if (mask[i] is 'x' && Marshal.ReadByte((nint)(cur + (uint)i)) != pattern[i])
                {
                    match = false;
                }
            }

            if (match)
            {
                return (nuint)((nint)cur + offset);
            }
        }

        return 0;
    }

    private static (nuint, ImageSectionHeader) GetSectionHeader(string headerName)
    {
        var m = Process.GetCurrentProcess().MainModule ?? throw new InvalidOperationException("Failed to initialize memory scanner. Failed to find main module");
        var baseAddr = (nuint)m.BaseAddress;
        var pe = new PeFile(m.FileName);
        var textHdr = pe.ImageSectionHeaders?.FirstOrDefault(h => h.Name == headerName);
        return textHdr is null
            ? throw new InvalidOperationException($"Failed to initialize memory scanner. Failed to find {headerName} section")
            : ((nuint, ImageSectionHeader))(baseAddr, textHdr);
    }

    private static uint GetGameTlsIndex()
    {
        var m = Process.GetCurrentProcess().MainModule ?? throw new InvalidOperationException("Failed to get TLS index. Failed to find main module");
        var baseAddr = (nint)m.BaseAddress;
        var pe = new PeFile(m.FileName);

        // Get the TLS data directory entry (index 9 = IMAGE_DIRECTORY_ENTRY_TLS)
        var tlsDirectory = pe.ImageNtHeaders?.OptionalHeader.DataDirectory[9];
        if (tlsDirectory is null || tlsDirectory.VirtualAddress == 0)
        {
            throw new InvalidOperationException("Failed to get TLS index. No TLS directory found");
        }

        // Calculate the address of the TLS directory in memory
        var tlsDirectoryAddress = baseAddr + (nint)tlsDirectory.VirtualAddress;

        // Read the IMAGE_TLS_DIRECTORY structure
        // AddressOfIndex is at offset 8 (after StartAddressOfRawData and EndAddressOfRawData)
        var addressOfIndexPtr = Marshal.ReadIntPtr(tlsDirectoryAddress + 8);

        // Read the TLS index from the location pointed to by AddressOfIndex
        var tlsIndex = (uint)Marshal.ReadInt32(addressOfIndexPtr);

        return tlsIndex;
    }
}

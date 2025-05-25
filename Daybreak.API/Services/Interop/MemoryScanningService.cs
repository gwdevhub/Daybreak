using Daybreak.Shared.Models.Interop;
using PeNet;
using PeNet.Header.Pe;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Extensions.Core;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Daybreak.API.Services.Interop;

public sealed unsafe class MemoryScanningService
{
    private readonly ILogger<MemoryScanningService> logger;
    private readonly (nuint BaseAddress, ImageSectionHeader Section) textSection = GetSectionHeader(".text");
    private readonly (nuint BaseAddress, ImageSectionHeader Section) dataSection = GetSectionHeader(".rdata");

    public MemoryScanningService(
        ILogger<MemoryScanningService> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public T? ReadPointer<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(GuildwarsPointer<T> ptr)
        where T : struct
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Reading pointer: 0x{ptr:X8}", ptr.Address);
        if (ptr.Address is 0)
        {
            return default;
        }

        return Marshal.PtrToStructure<T>((nint)ptr.Address);
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

                // If what we found is “…/file.hpp” (no drive letter) fix it
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

        for (var cur = textStart; cur <= textEnd; ++cur)
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
}

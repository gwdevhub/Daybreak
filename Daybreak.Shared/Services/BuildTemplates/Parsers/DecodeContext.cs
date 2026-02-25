using Daybreak.Shared.Services.BuildTemplates.Models;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

public readonly struct DecodeContext(
    string encodedString,
    string bitString,
    DecodeCharStream decodeCharStream)
{
    public readonly string EncodedString = encodedString;
    public readonly string BitString = bitString;
    public readonly DecodeCharStream DecodeCharStream = decodeCharStream;
}
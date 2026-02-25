using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates.Models;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

public readonly struct EncodeContext(
    IBuildEntry buildEntry,
    EncodeCharStream encodeCharStream)
{
    public readonly IBuildEntry BuildEntry = buildEntry;
    public readonly EncodeCharStream EncodeCharStream = encodeCharStream;
}
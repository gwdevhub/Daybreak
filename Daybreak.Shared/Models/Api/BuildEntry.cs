using System.Collections.Generic;

namespace Daybreak.Shared.Models.Api;
public sealed record BuildEntry(List<AttributeEntry> Attributes, List<uint> Skills)
{
}

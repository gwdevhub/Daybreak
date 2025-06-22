using System.Collections.Generic;

namespace Daybreak.Shared.Models.Api;
public sealed record PartyLoadout(List<PartyLoadoutEntry> Entries)
{
}

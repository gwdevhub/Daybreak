using System.Collections.Generic;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class BuildMetadata
{
    public List<int>? Base64Decoded { get; set; }
    public List<string>? BinaryDecoded { get; set; }
    public int Header { get; set; }
    public int VersionNumber { get; set; }
    public int ProfessionIdLength { get; set; }
    public int PrimaryProfessionId { get; set; }
    public int SecondaryProfessionId { get; set; }
    public int AttributeCount { get; set; }
    public int AttributesLength { get; set; }
    public int SkillsLength { get; set; }
    public bool TailPresent { get; set; }
    public bool NewTemplate { get; set; }
    public List<int> SkillIds { get; set; } = [];
    public List<int> AttributesIds { get; set; } = [];
    public List<int> AttributePoints { get; set; } = [];
}

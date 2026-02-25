using Daybreak.Shared.Models.Builds;

namespace Daybreak.Shared.Models.Guildwars;

public readonly struct SkillTemplateMetadata(
        List<int>? base64Decoded,
        List<string>? binaryDecoded,
        TemplateHeader header,
        int version,
        int professionIdLength,
        int primaryProfessionId,
        int secondaryProfessionId,
        int attributeCount,
        int attributesLength,
        int skillLength,
        bool tailPresent,
        bool newTemplate,
        List<int> skillIds,
        List<int> attributeIds,
        List<int> attributePoints
        )
{
    public readonly List<int>? Base64Decoded = base64Decoded;
    public readonly List<string>? BinaryDecoded = binaryDecoded;
    public readonly TemplateHeader Header = header;
    public readonly int VersionNumber = version;
    public readonly int ProfessionIdLength = professionIdLength;
    public readonly int PrimaryProfessionId = primaryProfessionId;
    public readonly int SecondaryProfessionId = secondaryProfessionId;
    public readonly int AttributeCount = attributeCount;
    public readonly int AttributesLength = attributesLength;
    public readonly int SkillsLength = skillLength;
    public readonly bool TailPresent = tailPresent;
    public readonly bool NewTemplate = newTemplate;
    public readonly List<int> SkillIds = skillIds;
    public readonly List<int> AttributesIds = attributeIds;
    public readonly List<int> AttributePoints = attributePoints;
}

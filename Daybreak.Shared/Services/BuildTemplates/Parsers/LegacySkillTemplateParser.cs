using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

/// <summary>
/// Parser for legacy skill templates (header values 0, 2-13).
/// See <see cref="Templates.md"/> for details on the skill template format.
/// </summary>
public sealed class LegacySkillTemplateParser : SkillTemplateParserBase, ITemplateParser<SkillTemplateMetadata>
{
    public bool CanDecode(TemplateHeader header)
    {
        return header switch
        {
            TemplateHeader.LegacySkill0 => true,
            TemplateHeader.LegacySkill2 => true,
            TemplateHeader.LegacySkill3 => true,
            TemplateHeader.LegacySkill4 => true,
            TemplateHeader.LegacySkill5 => true,
            TemplateHeader.LegacySkill6 => true,
            TemplateHeader.LegacySkill7 => true,
            TemplateHeader.LegacySkill8 => true,
            TemplateHeader.LegacySkill9 => true,
            TemplateHeader.LegacySkill10 => true,
            TemplateHeader.LegacySkill11 => true,
            TemplateHeader.LegacySkill12 => true,
            TemplateHeader.LegacySkill13 => true,
            _ => false
        };
    }

    public SkillTemplateMetadata Decode(DecodeContext context)
    {
        // For legacy templates, header was already read and IS the version
        // The stream position is already past the header (4 bits)
        // Decode embedded build (professions, attributes, skills)
        var (primaryProfessionId, secondaryProfessionId, attributeIds, attributePoints, skillIds) = DecodeEmbeddedBuild(context);

        // Calculate lengths for metadata (reverse the formulas)
        var professionIdLength = GetBitLength(Math.Max(primaryProfessionId, secondaryProfessionId));
        professionIdLength = Math.Max(4, ((professionIdLength - 4) / 2 * 2) + 4); // Normalize to valid length
        var attributesLength = GetBitLength(attributeIds.Count > 0 ? attributeIds.Max() : 0);
        attributesLength = Math.Max(4, attributesLength);
        var skillsLength = GetBitLength(skillIds.Count > 0 ? skillIds.Max() : 0);
        skillsLength = Math.Max(8, skillsLength);

        // Check for tail
        var tailPresent = context.DecodeCharStream.Position < context.DecodeCharStream.Length - 1;
        return new SkillTemplateMetadata(
            base64Decoded: [],
            binaryDecoded: [],
            header: TemplateHeader.LegacySkill0, // Will be set by caller based on actual header
            version: 0, // Legacy templates use header as version, but we normalize to 0 for processing
            professionIdLength: professionIdLength,
            primaryProfessionId: primaryProfessionId,
            secondaryProfessionId: secondaryProfessionId,
            attributeCount: attributeIds.Count,
            attributesLength: attributesLength,
            skillLength: skillsLength,
            tailPresent: tailPresent,
            newTemplate: false,
            skillIds: skillIds,
            attributeIds: attributeIds,
            attributePoints: attributePoints
        );
    }

    public IBuildEntry CreateBuildEntry(SkillTemplateMetadata templateContext)
    {
        var singleBuildEntry = new SingleBuildEntry();
        Profession.TryParse(templateContext.PrimaryProfessionId, out var primary);
        Profession.TryParse(templateContext.SecondaryProfessionId, out var secondary);

        singleBuildEntry.Primary = primary ?? Profession.None;
        singleBuildEntry.Secondary = secondary ?? Profession.None;
        singleBuildEntry.Attributes = [.. templateContext.AttributesIds
            .Select((id, index) => new AttributeEntry
            {
                Attribute = Shared.Models.Guildwars.Attribute.TryParse(id, out var attr) ? attr : Shared.Models.Guildwars.Attribute.None,
                Points = templateContext.AttributePoints[index]
            })];
        singleBuildEntry.Skills = [.. templateContext.SkillIds.Select(id => Skill.TryParse(id, out var skill) ? skill : Skill.None)];
        return singleBuildEntry;
    }

    public bool CanEncode(IBuildEntry buildEntry)
    {
        return false;
    }

    public string Encode(EncodeContext context)
    {
        throw new NotSupportedException("Legacy skill templates cannot be encoded.");
    }
}

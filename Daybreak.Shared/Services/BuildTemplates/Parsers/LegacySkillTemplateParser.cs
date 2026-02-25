using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

/// <summary>
/// Parser for legacy skill templates (header values 0, 2-13).
/// These templates were generated before the April 5, 2007 update.
/// In these templates, the header value IS the version number.
/// </summary>
public sealed class LegacySkillTemplateParser : ITemplateParser<SkillTemplateMetadata>
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
        // We need to reconstruct the version from the header that was read
        
        // Read profession length and professions
        var professionIdLength = (context.DecodeCharStream.Read(2) * 2) + 4;
        var primaryProfessionId = context.DecodeCharStream.Read(professionIdLength);
        var secondaryProfessionId = context.DecodeCharStream.Read(professionIdLength);

        // Read attributes
        var attributeCount = context.DecodeCharStream.Read(4);
        var attributesLength = context.DecodeCharStream.Read(4) + 4;
        var attributeIds = new List<int>();
        var attributePoints = new List<int>();
        for (int i = 0; i < attributeCount; i++)
        {
            attributeIds.Add(context.DecodeCharStream.Read(attributesLength));
            attributePoints.Add(context.DecodeCharStream.Read(4));
        }

        // Read skills
        var skillsLength = context.DecodeCharStream.Read(4) + 8;
        var skillIds = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            skillIds.Add(context.DecodeCharStream.Read(skillsLength));
        }

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
            attributeCount: attributeCount,
            attributesLength: attributesLength,
            skillLength: skillsLength,
            tailPresent: tailPresent,
            newTemplate: false,
            skillIds: skillIds,
            attributeIds: attributeIds,
            attributePoints: attributePoints
        );
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

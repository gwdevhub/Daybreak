using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

/// <summary>
/// See <see cref="Templates.md"/> for details on the skill template format.
/// </summary>
public sealed class SkillTemplateParser : ITemplateParser<SkillTemplateMetadata>
{
    private const int SkillTemplateHeader = 14;
    private const int CurrentVersion = 0;

    public bool CanDecode(TemplateHeader header)
    {
        return header == TemplateHeader.Skill;
    }

    public SkillTemplateMetadata Decode(DecodeContext context)
    {
        // Header 14 was already read, now read version
        var version = context.DecodeCharStream.Read(4);

        // Read profession length and professions
        // p = value * 2 + 4
        var professionLengthCode = context.DecodeCharStream.Read(2);
        var professionIdLength = (professionLengthCode * 2) + 4;
        var primaryProfessionId = context.DecodeCharStream.Read(professionIdLength);
        var secondaryProfessionId = context.DecodeCharStream.Read(professionIdLength);

        // Read attributes
        // a = value + 4
        var attributeCount = context.DecodeCharStream.Read(4);
        var attributesLengthCode = context.DecodeCharStream.Read(4);
        var attributesLength = attributesLengthCode + 4;
        var attributeIds = new List<int>();
        var attributePoints = new List<int>();
        for (int i = 0; i < attributeCount; i++)
        {
            attributeIds.Add(context.DecodeCharStream.Read(attributesLength));
            attributePoints.Add(context.DecodeCharStream.Read(4));
        }

        // Read skills
        // s = value + 8
        var skillsLengthCode = context.DecodeCharStream.Read(4);
        var skillsLength = skillsLengthCode + 8;
        var skillIds = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            skillIds.Add(context.DecodeCharStream.Read(skillsLength));
        }

        // Check for optional trailing bit
        var tailPresent = context.DecodeCharStream.Position < context.DecodeCharStream.Length - 1;

        return new SkillTemplateMetadata(
            base64Decoded: [],
            binaryDecoded: [],
            header: TemplateHeader.Skill,
            version: version,
            professionIdLength: professionIdLength,
            primaryProfessionId: primaryProfessionId,
            secondaryProfessionId: secondaryProfessionId,
            attributeCount: attributeCount,
            attributesLength: attributesLength,
            skillLength: skillsLength,
            tailPresent: tailPresent,
            newTemplate: true,
            skillIds: skillIds,
            attributeIds: attributeIds,
            attributePoints: attributePoints
        );
    }

    public bool CanEncode(IBuildEntry buildEntry)
    {
        return buildEntry is SingleBuildEntry;
    }

    public string Encode(EncodeContext context)
    {
        if (context.BuildEntry is not SingleBuildEntry build)
        {
            throw new InvalidOperationException("SkillTemplateParser can only encode Build entries.");
        }

        var stream = context.EncodeCharStream;

        // Write header (14)
        stream.Write(SkillTemplateHeader, 4);

        // Write version (0)
        stream.Write(CurrentVersion, 4);

        // Calculate profession length code
        // p = code * 2 + 4, so code = (p - 4) / 2
        var primaryId = build.Primary?.Id ?? 0;
        var secondaryId = build.Secondary?.Id ?? 0;
        var maxProfessionId = Math.Max(primaryId, secondaryId);
        var desiredProfessionBits = GetBitLength(maxProfessionId);
        var professionLengthCode = Math.Max((desiredProfessionBits - 4) / 2, 0);
        var professionIdLength = (professionLengthCode * 2) + 4;

        stream.Write(professionLengthCode, 2);
        stream.Write(primaryId, professionIdLength);
        stream.Write(secondaryId, professionIdLength);

        // Write attributes
        var attributeCount = build.Attributes.Count;
        stream.Write(attributeCount, 4);

        // Calculate attribute length code
        // a = code + 4, so code = a - 4
        var maxAttributeId = attributeCount > 0
            ? build.Attributes.Max(a => a.Attribute?.Id ?? 0)
            : 0;
        var desiredAttributeBits = GetBitLength(maxAttributeId);
        var attributesLengthCode = Math.Max(desiredAttributeBits - 4, 0);
        var attributesLength = attributesLengthCode + 4;

        stream.Write(attributesLengthCode, 4);
        for (int i = 0; i < attributeCount; i++)
        {
            var attrEntry = build.Attributes[i];
            stream.Write(attrEntry.Attribute?.Id ?? 0, attributesLength);
            stream.Write(attrEntry.Points, 4);
        }

        // Write skills
        // s = code + 8, so code = s - 8
        var skillIds = build.Skills.Select(s => s?.Id ?? 0).ToList();
        var maxSkillId = skillIds.Count > 0 ? skillIds.Max() : 0;
        var desiredSkillBits = GetBitLength(maxSkillId);
        var skillsLengthCode = Math.Max(desiredSkillBits - 8, 0);
        var skillsLength = skillsLengthCode + 8;

        stream.Write(skillsLengthCode, 4);
        for (int i = 0; i < 8; i++)
        {
            var skillId = i < skillIds.Count ? skillIds[i] : 0;
            stream.Write(skillId, skillsLength);
        }

        // Write trailing bit
        stream.Write(0, 1);

        return stream.GetEncodedString();
    }

    private static int GetBitLength(int value)
    {
        if (value <= 0)
        {
            return 1;
        }

        var bits = 1;
        value /= 2;
        while (value > 0)
        {
            bits++;
            value /= 2;
        }

        return bits;
    }
}

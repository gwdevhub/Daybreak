using Daybreak.Shared.Models.Builds;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

/// <summary>
/// Base class providing shared encoding/decoding logic for skill templates.
/// See <see cref="Templates.md"/> for format details.
/// </summary>
public abstract class SkillTemplateParserBase
{
    /// <summary>
    /// Decodes the embedded build portion of a skill template (professions, attributes, skills).
    /// Does not read header or version - those are template-specific.
    /// </summary>
    protected static (int primaryId, int secondaryId, List<int> attributeIds, List<int> attributePoints, List<int> skillIds)
        DecodeEmbeddedBuild(DecodeContext context)
    {
        // Read profession length code (2 bits)
        // p = code * 2 + 4
        var professionLengthCode = context.DecodeCharStream.Read(2);
        var professionIdLength = (professionLengthCode * 2) + 4;

        var primaryId = context.DecodeCharStream.Read(professionIdLength);
        var secondaryId = context.DecodeCharStream.Read(professionIdLength);

        // Read attribute count (4 bits)
        var attributeCount = context.DecodeCharStream.Read(4);

        // Read attribute length code (4 bits)
        // a = code + 4
        var attributesLengthCode = context.DecodeCharStream.Read(4);
        var attributesLength = attributesLengthCode + 4;

        var attributeIds = new List<int>();
        var attributePoints = new List<int>();
        for (int i = 0; i < attributeCount; i++)
        {
            attributeIds.Add(context.DecodeCharStream.Read(attributesLength));
            attributePoints.Add(context.DecodeCharStream.Read(4));
        }

        // Read skill length code (4 bits)
        // s = code + 8
        var skillsLengthCode = context.DecodeCharStream.Read(4);
        var skillsLength = skillsLengthCode + 8;

        var skillIds = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            skillIds.Add(context.DecodeCharStream.Read(skillsLength));
        }

        return (primaryId, secondaryId, attributeIds, attributePoints, skillIds);
    }

    /// <summary>
    /// Encodes the embedded build portion of a skill template (professions, attributes, skills).
    /// Does not write header or version - those are template-specific.
    /// </summary>
    protected static void EncodeEmbeddedBuild(Models.EncodeCharStream stream, SingleBuildEntry build)
    {
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
    }

    /// <summary>
    /// Gets the number of bits required to represent a value.
    /// </summary>
    protected static int GetBitLength(int value)
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

using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

/// <summary>
/// See <see cref="Templates.md"/> for details on the skill template format.
/// </summary>
public sealed class SkillTemplateParser : SkillTemplateParserBase, ITemplateParser<SkillTemplateMetadata>
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

        // Decode embedded build (professions, attributes, skills)
        var (primaryProfessionId, secondaryProfessionId, attributeIds, attributePoints, skillIds) = DecodeEmbeddedBuild(context);

        // Calculate lengths for metadata (reverse the formulas)
        var professionIdLength = GetBitLength(Math.Max(primaryProfessionId, secondaryProfessionId));
        professionIdLength = Math.Max(4, ((professionIdLength - 4) / 2 * 2) + 4); // Normalize to valid length
        var attributesLength = GetBitLength(attributeIds.Count > 0 ? attributeIds.Max() : 0);
        attributesLength = Math.Max(4, attributesLength);
        var skillsLength = GetBitLength(skillIds.Count > 0 ? skillIds.Max() : 0);
        skillsLength = Math.Max(8, skillsLength);

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
            attributeCount: attributeIds.Count,
            attributesLength: attributesLength,
            skillLength: skillsLength,
            tailPresent: tailPresent,
            newTemplate: true,
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

        // Encode embedded build (professions, attributes, skills)
        EncodeEmbeddedBuild(stream, build);

        // Write trailing bit
        stream.Write(0, 1);

        return stream.GetEncodedString();
    }
}

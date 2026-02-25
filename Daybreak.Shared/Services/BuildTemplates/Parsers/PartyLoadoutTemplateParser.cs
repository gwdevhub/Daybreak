using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

/// <summary>
/// Parser for party loadout templates (extended template header 15, type 1).
/// See <see cref="Templates.md"/> for format details.
/// </summary>
public sealed class PartyLoadoutTemplateParser : ITemplateParser<PartyLoadoutTemplateMetadata>
{
    private const int ExtendedTemplateHeader = 15;
    private const int PartyLoadoutType = 1;
    private const int CurrentVersion = 1;

    public bool CanDecode(TemplateHeader header)
    {
        return header == TemplateHeader.Extension;
    }

    public PartyLoadoutTemplateMetadata Decode(DecodeContext context)
    {
        // Header 15 was already read, now read type (8 bits)
        var type = context.DecodeCharStream.Read(8);
        if (type != PartyLoadoutType)
        {
            throw new InvalidOperationException($"Unknown extended template type: {type}");
        }

        // Read version (4 bits)
        var version = context.DecodeCharStream.Read(4);

        // Read party size (4 bits)
        var partySize = context.DecodeCharStream.Read(4);

        var members = new List<PartyLoadoutMemberMetadata>();
        for (int i = 0; i < partySize; i++)
        {
            var member = DecodePartyMember(context);
            members.Add(member);
        }

        return new PartyLoadoutTemplateMetadata(version, partySize, members);
    }

    private static PartyLoadoutMemberMetadata DecodePartyMember(DecodeContext context)
    {
        // Read member type (2 bits)
        var memberTypeCode = context.DecodeCharStream.Read(2);
        var memberType = memberTypeCode switch
        {
            0 => PartyCompositionMemberType.Player,
            1 => PartyCompositionMemberType.Henchman,
            2 => PartyCompositionMemberType.Hero,
            _ => PartyCompositionMemberType.Unknown
        };

        // Read hero ID (6 bits) - only meaningful for heroes
        var heroId = context.DecodeCharStream.Read(6);

        // Read behavior (2 bits)
        var behavior = context.DecodeCharStream.Read(2);

        // Read embedded build (no header/version)
        var (primaryId, secondaryId, attributeIds, attributePoints, skillIds) = DecodeEmbeddedBuild(context);

        return new PartyLoadoutMemberMetadata(
            memberType,
            heroId,
            behavior,
            primaryId,
            secondaryId,
            attributeIds,
            attributePoints,
            skillIds);
    }

    private static (int primaryId, int secondaryId, List<int> attributeIds, List<int> attributePoints, List<int> skillIds)
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

    public IBuildEntry CreateBuildEntry(PartyLoadoutTemplateMetadata templateContext)
    {
        var teamBuild = new TeamBuildEntry();
        var partyComposition = new List<PartyCompositionMetadataEntry>();

        for (int i = 0; i < templateContext.Members.Count; i++)
        {
            var member = templateContext.Members[i];

            var singleBuild = new SingleBuildEntry();
            Profession.TryParse(member.PrimaryProfessionId, out var primary);
            Profession.TryParse(member.SecondaryProfessionId, out var secondary);

            singleBuild.Primary = primary ?? Profession.None;
            singleBuild.Secondary = secondary ?? Profession.None;
            singleBuild.Attributes = [.. member.AttributeIds
                .Select((id, idx) => new Shared.Models.Builds.AttributeEntry
                {
                    Attribute = Shared.Models.Guildwars.Attribute.TryParse(id, out var attr) ? attr : Shared.Models.Guildwars.Attribute.None,
                    Points = member.AttributePoints[idx]
                })];
            singleBuild.Skills = [.. member.SkillIds.Select(id => Skill.TryParse(id, out var skill) ? skill : Skill.None)];

            teamBuild.Builds.Add(singleBuild);

            // Build party composition metadata
            var behavior = member.Behavior switch
            {
                0 => HeroBehavior.Fight,
                1 => HeroBehavior.Guard,
                2 => HeroBehavior.Avoid,
                _ => HeroBehavior.Undefined
            };

            partyComposition.Add(new PartyCompositionMetadataEntry
            {
                Type = member.MemberType,
                Index = i,
                HeroId = member.MemberType == PartyCompositionMemberType.Hero ? member.HeroId : null,
                Behavior = behavior
            });
        }

        // Set party composition all at once (the property serializes to Metadata)
        teamBuild.PartyComposition = partyComposition;

        return teamBuild;
    }

    public bool CanEncode(IBuildEntry buildEntry)
    {
        return buildEntry is TeamBuildEntry;
    }

    public string Encode(EncodeContext context)
    {
        if (context.BuildEntry is not TeamBuildEntry teamBuild)
        {
            throw new InvalidOperationException("PartyLoadoutTemplateParser can only encode TeamBuildEntry.");
        }

        var stream = context.EncodeCharStream;

        // Write header (15)
        stream.Write(ExtendedTemplateHeader, 4);

        // Write type (1 = party loadout)
        stream.Write(PartyLoadoutType, 8);

        // Write version (1)
        stream.Write(CurrentVersion, 4);

        // Write party size
        var partySize = teamBuild.Builds.Count;
        stream.Write(partySize, 4);

        var partyComposition = teamBuild.PartyComposition ?? [];

        // Write each party member
        for (int i = 0; i < partySize; i++)
        {
            var build = teamBuild.Builds[i];
            var composition = i < partyComposition.Count
                ? partyComposition[i]
                : new PartyCompositionMetadataEntry
                {
                    Type = PartyCompositionMemberType.Player,
                    Index = i,
                    HeroId = null,
                    Behavior = HeroBehavior.Guard
                };

            EncodePartyMember(stream, build, composition);
        }

        return stream.GetEncodedString();
    }

    private static void EncodePartyMember(
        Models.EncodeCharStream stream,
        SingleBuildEntry build,
        PartyCompositionMetadataEntry composition)
    {
        // Write member type (2 bits)
        var memberTypeCode = composition.Type switch
        {
            PartyCompositionMemberType.Player => 0,
            PartyCompositionMemberType.MainPlayer => 0,
            PartyCompositionMemberType.Henchman => 1,
            PartyCompositionMemberType.Hero => 2,
            _ => 0
        };
        stream.Write(memberTypeCode, 2);

        // Write hero ID (6 bits)
        var heroId = composition.HeroId ?? 0;
        stream.Write(heroId, 6);

        // Write behavior (2 bits)
        var behaviorCode = composition.Behavior switch
        {
            HeroBehavior.Fight => 0,
            HeroBehavior.Guard => 1,
            HeroBehavior.Avoid => 2,
            _ => 1 // Default to Guard
        };
        stream.Write(behaviorCode, 2);

        // Write embedded build
        EncodeEmbeddedBuild(stream, build);
    }

    private static void EncodeEmbeddedBuild(Models.EncodeCharStream stream, SingleBuildEntry build)
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

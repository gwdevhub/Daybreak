using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Services.BuildTemplates.Parsers;

/// <summary>
/// Parser for party loadout templates (extended template header 15, type 1).
/// See <see cref="Templates.md"/> for format details.
/// </summary>
public sealed class PartyLoadoutTemplateParser : SkillTemplateParserBase, ITemplateParser<PartyLoadoutTemplateMetadata>
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

    // DecodeEmbeddedBuild is inherited from SkillTemplateParserBase

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
}

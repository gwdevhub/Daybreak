using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Extensions;
using System.Extensions.Core;
using System.Text;
using System.Text.Json;
using AttributeEntry = Daybreak.Shared.Models.Builds.AttributeEntry;
using Convert = System.Convert;

namespace Daybreak.Shared.Services.BuildTemplates;

public sealed class BuildTemplateManager(
    ILogger<BuildTemplateManager> logger) : IBuildTemplateManager
{
    private const string DecodingLookupTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    private readonly static string BuildsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Guild Wars\\Templates\\Skills";

    private List<IBuildEntry> BuildMemoryCache { get; } = [];

    private readonly ILogger<BuildTemplateManager> logger = logger.ThrowIfNull(nameof(logger));

    public bool IsTemplate(string template)
    {
        if (template.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (template.Where(c => DecodingLookupTable.Contains(c) is false).Any())
        {
            return false;
        }

        return true;
    }

    public SingleBuildEntry CreateSingleBuild()
    {
        var emptyBuild = new Build();
        var name = Guid.NewGuid().ToString();
        var entry = new SingleBuildEntry
        {
            Name = name,
            PreviousName = string.Empty,
            Attributes = emptyBuild.Attributes,
            Skills = emptyBuild.Skills,
            CreationTime = DateTime.UtcNow
        };

        this.BuildMemoryCache.Add(entry);
        return entry;
    }

    public SingleBuildEntry CreateSingleBuild(string name)
    {
        var emptyBuild = new Build();
        var entry = new SingleBuildEntry
        {
            Name = name,
            PreviousName = string.Empty,
            Attributes = emptyBuild.Attributes,
            Skills = emptyBuild.Skills,
            CreationTime = DateTime.UtcNow
        };

        this.BuildMemoryCache.Add(entry);
        return entry;
    }

    public SingleBuildEntry CreateSingleBuild(BuildEntry buildEntry)
    {
        var singleBuildEntry = this.CreateSingleBuild();
        return this.PopulateSingleBuild(singleBuildEntry, buildEntry);
    }

    public SingleBuildEntry CreateSingleBuild(string name, BuildEntry buildEntry)
    {
        var singleBuildEntry = this.CreateSingleBuild(name);
        return this.PopulateSingleBuild(singleBuildEntry, buildEntry);
    }

    public TeamBuildEntry CreateTeamBuild()
    {
        var name = Guid.NewGuid().ToString();
        var entry = new TeamBuildEntry
        {
            Name = name,
            PreviousName = string.Empty,
            Builds = [this.CreateSingleBuild()],
            CreationTime = DateTime.UtcNow
        };

        this.BuildMemoryCache.Add(entry);
        return entry;
    }

    public TeamBuildEntry CreateTeamBuild(string name)
    {
        var entry = new TeamBuildEntry
        {
            Name = name,
            PreviousName = string.Empty,
            Builds = [this.CreateSingleBuild(name)],
            CreationTime = DateTime.UtcNow
        };

        this.BuildMemoryCache.Add(entry);
        return entry;
    }

    public TeamBuildEntry CreateTeamBuild(PartyLoadout partyLoadout)
    {
        var entry = this.CreateTeamBuild();
        return this.PopulateTeamBuild(entry, partyLoadout);
    }

    public TeamBuildEntry CreateTeamBuild(PartyLoadout partyLoadout, string name)
    {
        var entry = this.CreateTeamBuild(name);
        return this.PopulateTeamBuild(entry, partyLoadout);
    }

    public SingleBuildEntry ConvertToSingleBuildEntry(TeamBuildEntry teamBuildEntry)
    {
        if (teamBuildEntry.Builds.Count > 1 ||
            teamBuildEntry.Builds.Count < 1)
        {
            throw new InvalidOperationException($"Cannot convert {nameof(TeamBuildEntry)} with {teamBuildEntry.Builds.Count} builds into a {nameof(SingleBuildEntry)}");
        }

        var singleBuildEntry = teamBuildEntry.Builds.First();
        singleBuildEntry.Name = teamBuildEntry.Name;
        singleBuildEntry.PreviousName = teamBuildEntry.PreviousName;
        singleBuildEntry.SourceUrl = teamBuildEntry.SourceUrl;
        singleBuildEntry.Metadata = teamBuildEntry.Metadata;

        return singleBuildEntry;
    }

    public TeamBuildEntry ConvertToTeamBuildEntry(SingleBuildEntry singleBuildEntry)
    {
        var teamBuildEntry = new TeamBuildEntry
        {
            Name = singleBuildEntry.Name,
            PreviousName = singleBuildEntry.PreviousName,
            SourceUrl = singleBuildEntry.SourceUrl,
            Builds = [ singleBuildEntry ],
            Metadata = singleBuildEntry.Metadata
        };

        return teamBuildEntry;
    }

    public PartyLoadout ConvertToPartyLoadout(TeamBuildEntry teamBuildEntry)
    {
        if (teamBuildEntry.PartyComposition is not List<PartyCompositionMetadataEntry> partyComposition ||
            partyComposition.Count != teamBuildEntry.Builds.Count)
        {
            throw new InvalidOperationException("Invalid team build entry. Team build entry contains invalid party composition metadata");
        }

        if (partyComposition.Any(c => c.Type is PartyCompositionMemberType.Hero && (c.HeroId is null || c.Behavior is null)))
        {
            throw new InvalidOperationException("Invalid team build entry. Hero entries without specified heroes and behaviors are not currently supported");
        }

        var loadoutEntryComposition = teamBuildEntry.Builds.Select((build, index) =>
        {
            var compositionEntry = partyComposition.First(c => c.Index == index);
            return (build, compositionEntry, index);
        });

        return new PartyLoadout(
            Entries: [.. loadoutEntryComposition.Select(entry => new PartyLoadoutEntry(
                HeroId: entry.compositionEntry.Type is PartyCompositionMemberType.MainPlayer ? 0 : entry.compositionEntry.HeroId ?? throw new InvalidOperationException(),
                HeroBehavior: entry.compositionEntry.Behavior ?? HeroBehavior.Guard,
                Build: this.ConvertToBuildEntry(entry.build)))]);
    }

    public BuildEntry ConvertToBuildEntry(SingleBuildEntry singleBuildEntry)
    {
        return new BuildEntry(
            Primary: singleBuildEntry.Primary.Id,
            Secondary: singleBuildEntry.Secondary.Id,
            Attributes: [.. singleBuildEntry.Attributes.Select(a => new Shared.Models.Api.AttributeEntry((uint)(a.Attribute?.Id ?? -1), (uint)a.Points, (uint)a.Points))],
            Skills: [.. singleBuildEntry.Skills.Select(s => (uint)s.Id)]);
    }

    public bool CanApply(MainPlayerBuildContext mainPlayerBuildContext, TeamBuildEntry teamBuildEntry)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (teamBuildEntry.PartyComposition is not List<PartyCompositionMetadataEntry> partyComposition ||
            partyComposition.Count != teamBuildEntry.Builds.Count)
        {
            scopedLogger.LogDebug("Invalid team build entry {buildName}. Team build entry contains invalid party composition metadata", teamBuildEntry.Name ?? string.Empty);
            return false;
        }

        if (partyComposition.Any(c => c.Type is PartyCompositionMemberType.Hero && (c.HeroId is null || c.Behavior is null)))
        {
            scopedLogger.LogDebug("Invalid team build entry {buildName}. Hero entries without specified heroes and behaviors are not currently supported", teamBuildEntry.Name ?? string.Empty);
            return false;
        }

        var loadoutEntryComposition = teamBuildEntry.Builds.Select((build, index) =>
        {
            var compositionEntry = partyComposition.First(c => c.Index == index);
            return (build, compositionEntry, index);
        });

        return loadoutEntryComposition.Any(entry => entry.compositionEntry.Type is PartyCompositionMemberType.MainPlayer && this.CanApply(mainPlayerBuildContext, entry.build));
    }

    public bool CanApply(MainPlayerBuildContext mainPlayerBuildContext, SingleBuildEntry singleBuildEntry)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (singleBuildEntry.Primary.Id != mainPlayerBuildContext.PrimaryProfessionId)
        {
            scopedLogger.LogDebug("Invalid build entry {buildName}. Build primary {buildPrimaryId} does not match player primary {playerPrimaryId}", singleBuildEntry.Name ?? string.Empty, singleBuildEntry.Primary.Id, mainPlayerBuildContext.PrimaryProfessionId);
            return false;
        }

        if (singleBuildEntry.Secondary != Profession.None &&
            !IsProfessionUnlocked(singleBuildEntry.Secondary.Id, mainPlayerBuildContext.UnlockedProfessions))
        {
            scopedLogger.LogDebug("Invalid build entry {buildName}. Build secondary {buildSecondaryId} is not unlocked by the player", singleBuildEntry.Name ?? string.Empty, singleBuildEntry.Secondary.Id);
            return false;
        }

        return true;
    }

    public bool CanTemplateApply(BuildTemplateValidationRequest request)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (request.BuildPrimary != request.CurrentPrimary)
        {
            scopedLogger.LogError("Primary profession does not match current primary profession");
            return false;
        }

        if (request.BuildSecondary is not 0 &&
            !IsProfessionUnlocked((int)request.BuildSecondary, request.UnlockedCharacterProfessions))
        {
            scopedLogger.LogError("Secondary profession is not unlocked");
            return false;
        }

        return true;
    }

    public void SaveBuild(IBuildEntry buildEntry)
    {
        var encodedBuild = new StringBuilder();
        if (buildEntry is SingleBuildEntry singleBuild)
        {
            var build = new Build
            {
                Attributes = singleBuild.Attributes,
                Primary = singleBuild.Primary,
                Secondary = singleBuild.Secondary,
                Skills = singleBuild.Skills
            };

            encodedBuild.Append(this.EncodeTemplateInner(build));
        }
        else if (buildEntry is TeamBuildEntry teamBuild)
        {
            foreach(var singleBuildEntry in teamBuild.Builds)
            {
                var build = new Build
                {
                    Attributes = singleBuildEntry.Attributes,
                    Primary = singleBuildEntry.Primary,
                    Secondary = singleBuildEntry.Secondary,
                    Skills = singleBuildEntry.Skills
                };

                encodedBuild.Append(this.EncodeTemplateInner(build)).Append(' ');
            }
        }

        var newPath = Path.Combine(BuildsPath, $"{buildEntry.Name}.txt");
        if (string.IsNullOrWhiteSpace(buildEntry.PreviousName) is false)
        {
            var oldPath = Path.GetFullPath(Path.Combine(BuildsPath, $"{buildEntry.PreviousName}.txt"));
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }
        }

        Directory.CreateDirectory(Path.GetDirectoryName(newPath)!);
        var metadata = buildEntry.Metadata is not null
            ? Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(buildEntry.Metadata)))
            : string.Empty;
        File.WriteAllText(newPath, $"{encodedBuild.ToString().Trim()}\n{metadata}");

        // Remove the build from the memory cache once it's created on disk
        this.BuildMemoryCache.Remove(buildEntry);
    }

    public void RemoveBuild(IBuildEntry buildEntry)
    {
        if (File.Exists($"{BuildsPath}\\{buildEntry.Name}.txt"))
        {
            File.Delete($"{BuildsPath}\\{buildEntry.Name}.txt");
        }

        if (File.Exists($"{BuildsPath}\\{buildEntry.PreviousName}.txt"))
        {
            File.Delete($"{BuildsPath}\\{buildEntry.PreviousName}.txt");
        }
    }

    public async Task<IBuildEntry?> GetBuild(string name)
    {
        // Use the cache as fallback for newly created builds that are not yet saved to the disk
        var path = Path.Combine(BuildsPath, $"{name}.txt");
        var result = await this.LoadBuildFromFile(path, name);
        if (result is null)
        {
            var maybeCachedBuild = this.BuildMemoryCache.FirstOrDefault(b => b.Name == name);
            if (maybeCachedBuild is not null)
            {
                return maybeCachedBuild;
            }

            return default;
        }

        return result;
    }

    public void ClearBuilds()
    {
        foreach(var file in Directory.GetFiles(BuildsPath, "*.txt", SearchOption.AllDirectories))
        {
            File.Delete(file);
        }
    }

    public async IAsyncEnumerable<IBuildEntry> GetBuilds()
    {
        if (Directory.Exists(BuildsPath) is false)
        {
            yield break;
        }

        foreach (var file in Directory.GetFiles(BuildsPath, "*.txt", SearchOption.AllDirectories))
        {
            var name = Path.GetRelativePath(BuildsPath, file).Replace(".txt", string.Empty);
            var result = await this.LoadBuildFromFile(file, name);
            if (result is not null)
            {
                yield return result;
            }
        }
    }

    public IBuildEntry DecodeTemplate(string template)
    {
        var randomName = Guid.NewGuid().ToString();
        var builds = this.DecodeTemplatesInner(template);
        return builds is null
            ? throw new InvalidOperationException("Failed to decode build template")
            : builds.Length == 1 ?
                new SingleBuildEntry
                {
                    Name = randomName,
                    PreviousName = randomName,
                    Primary = builds[0].Primary,
                    Secondary = builds[0].Secondary,
                    Attributes = builds[0].Attributes,
                    Skills = builds[0].Skills
                } :
                new TeamBuildEntry
                {
                    Name = randomName,
                    PreviousName = randomName,
                    Builds = [.. builds.Select(b => new SingleBuildEntry
                    {
                        Name = randomName,
                        PreviousName = randomName,
                        Primary = b.Primary,
                        Secondary = b.Secondary,
                        Attributes = b.Attributes,
                        Skills = b.Skills
                    })]
                };
    }

    public bool TryDecodeTemplate(string template, [NotNullWhen(true)] out IBuildEntry? build)
    {
        try
        {
            var maybeBuilds = this.DecodeTemplatesInner(template);
            if (maybeBuilds is not null)
            {
                var randomName = Guid.NewGuid().ToString();
                build = maybeBuilds.Length == 1 ?
                    new SingleBuildEntry
                    {
                        Name = randomName,
                        PreviousName = randomName,
                        Primary = maybeBuilds[0].Primary,
                        Secondary = maybeBuilds[0].Secondary,
                        Attributes = maybeBuilds[0].Attributes,
                        Skills = maybeBuilds[0].Skills
                    } :
                    new TeamBuildEntry
                    {
                        Name = randomName,
                        PreviousName = randomName,
                        Builds = maybeBuilds.Select(b => new SingleBuildEntry
                        {
                            Name = randomName,
                            PreviousName = randomName,
                            Primary = b.Primary,
                            Secondary = b.Secondary,
                            Attributes = b.Attributes,
                            Skills = b.Skills
                        }).ToList()
                    };

                return true;
            }
        }
        catch (Exception e)
        {
            var scopedLogger = this.logger.CreateScopedLogger();
            scopedLogger.LogError(e, "Failed to decode build template");
        }

        build = default;
        return false;
    }

    public string EncodeTemplate(IBuildEntry build)
    {
        if (build is SingleBuildEntry singleBuildEntry)
        {
            var preparedBuild = new Build
            {
                Primary = singleBuildEntry.Primary,
                Secondary = singleBuildEntry.Secondary,
                Attributes = singleBuildEntry.Attributes,
                Skills = singleBuildEntry.Skills
            };

            return this.EncodeTemplateInner(preparedBuild);
        }
        else if (build is TeamBuildEntry teamBuildEntry)
        {
            var encodedString = new StringBuilder();
            foreach(var teamBuild in teamBuildEntry.Builds)
            {
                var preparedBuild = new Build
                {
                    Primary = teamBuild.Primary,
                    Secondary = teamBuild.Secondary,
                    Attributes = teamBuild.Attributes,
                    Skills = teamBuild.Skills
                };

                encodedString.Append(this.EncodeTemplateInner(preparedBuild)).Append(' ');
            }

            return encodedString.ToString().Trim();
        }

        throw new InvalidOperationException($"Unknown build entry of type {build.GetType().Name}");
    }

    private TeamBuildEntry PopulateTeamBuild(TeamBuildEntry teamBuildEntry, PartyLoadout partyLoadout)
    {
        teamBuildEntry.Builds.Clear();
        var partyCompositionMetadata = new List<PartyCompositionMetadataEntry>();
        foreach (var entry in partyLoadout.Entries.OrderBy(e => e.HeroId))
        {
            var build = entry.Build;
            var buildEntry = this.CreateSingleBuild();
            this.PopulateSingleBuild(buildEntry, build);

            var partyCompositionEntry = new PartyCompositionMetadataEntry
            {
                Type = entry.HeroId is 0 ? PartyCompositionMemberType.MainPlayer : PartyCompositionMemberType.Hero,
                Index = entry.HeroId is 0 ? 0 : teamBuildEntry.Builds.Count,
                Behavior = entry.HeroId is 0 ? default : (HeroBehavior)entry.HeroBehavior,
                HeroId = entry.HeroId is 0 ? default : entry.HeroId
            };

            if (partyCompositionEntry.Type is PartyCompositionMemberType.MainPlayer)
            {
                teamBuildEntry.Builds.Insert(0, buildEntry);
                partyCompositionMetadata.Insert(0, partyCompositionEntry);
            }
            else
            {
                teamBuildEntry.Builds.Add(buildEntry);
                partyCompositionMetadata.Add(partyCompositionEntry);
            }
        }

        teamBuildEntry.PartyComposition = partyCompositionMetadata;
        return teamBuildEntry;
    }

    private SingleBuildEntry PopulateSingleBuild(SingleBuildEntry singleBuildEntry, BuildEntry buildEntry)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!Profession.TryParse(buildEntry.Primary, out var primary))
        {
            scopedLogger.LogError("Failed to parse primary profession with id {professionId}", buildEntry.Primary);
            throw new InvalidOperationException($"Failed to parse primary profession with id {buildEntry.Primary}");
        }

        if (!Profession.TryParse(buildEntry.Secondary, out var secondary))
        {
            scopedLogger.LogError("Failed to parse secondary profession with id {professionId}", buildEntry.Secondary);
            throw new InvalidOperationException($"Failed to parse secondary profession with id {buildEntry.Secondary}");
        }

        singleBuildEntry.Primary = primary;
        singleBuildEntry.Secondary = secondary;
        singleBuildEntry.Attributes = [.. buildEntry.Attributes.Select(a =>
        {
            if (!Daybreak.Shared.Models.Guildwars.Attribute.TryParse((int)a.Id, out var attr))
            {
                scopedLogger.LogError("Failed to parse attribute with id {attributeId}", a.Id);
                throw new InvalidOperationException($"Failed to parse attribute with id {a.Id}");
            }

            return new Daybreak.Shared.Models.Builds.AttributeEntry
            {
                Attribute = attr,
                Points = (int)a.BasePoints
            };
        })];
        singleBuildEntry.Skills = [.. buildEntry.Skills.Select(s =>
        {
            if (!Skill.TryParse((int)s, out var skill))
            {
                scopedLogger.LogError("Failed to parse skill with id {skillId}", s);
                throw new InvalidOperationException($"Failed to parse skill with id {s}");
            }

            return skill;
        })];

        return singleBuildEntry;
    }

    private async Task<IBuildEntry?> LoadBuildFromFile(string path, string buildName)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (File.Exists(path) is false)
        {
            scopedLogger.LogError("Unable to find build file by name {buildName}", buildName);
            return default;
        }

        var content = await File.ReadAllLinesAsync(path);
        if (content.Length == 0)
        {
            scopedLogger.LogError("File {buildName} does not contain a valid template code", buildName);
            return default;
        }

        if (this.TryDecodeTemplate(content.First(), out var build) is false)
        {
            scopedLogger.LogError("Unable to parse build file {buildName}", buildName);
            return default;
        }

        build.Name = buildName;
        build.PreviousName = buildName;
        if (content.Length > 1)
        {
            // This check has to stay in place for backwards compatibility. If the second line in a build is url, it is the source Url of the build. Otherwise, it is a base64 encoded metadata of the build
            if (Uri.TryCreate(content[1], UriKind.Absolute, out var sourceUrl))
            {
                build.SourceUrl = sourceUrl.ToString();
            }
            else
            {
                try
                {
                    build.Metadata =
                        JsonSerializer.Deserialize<Dictionary<string, string>>(
                            Encoding.UTF8.GetString(
                                Convert.FromBase64String(content[1])));
                }
                catch(Exception ex)
                {
                    scopedLogger.LogError(ex, "Failed to parse build metadata {buildName}", buildName);
                    return default;
                }
            }
        }

        if (build.CreationTime == DateTime.MinValue)
        {
            build.CreationTime = new FileInfo(path).CreationTimeUtc;
        }

        return build;
    }

    private Build[]? DecodeTemplatesInner(string template)
    {
        var buildTemplates = template.Split(' ').Where(s => !s.IsNullOrWhiteSpace()).ToArray();
        if (buildTemplates.Length == 0)
        {
            return [];
        }

        var builds = new Build[buildTemplates.Length];
        for(var i = 0; i < builds.Length; i++)
        {
            var result = this.DecodeTemplateInner(buildTemplates[i]);
            if (result is null)
            {
                return default;
            }

            builds[i] = result;
        }

        return builds;
    }

    private Build? DecodeTemplateInner(string template)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Attempting to decode template");
        var buildMetadata = ParseEncodedTemplate(template);
        scopedLogger.LogDebug("Decoded template. Beginning parsing");
        if (buildMetadata.VersionNumber != 0)
        {
            scopedLogger.LogError("Expected version number to be 0 but found {buildMetadata.VersionNumber}", buildMetadata.VersionNumber);
            return default;
        }

        var build = new Build()
        {
            BuildMetadata = buildMetadata,
            Skills = []
        };

        if (Profession.TryParse(buildMetadata.PrimaryProfessionId, out var primaryProfession) is false)
        {
            scopedLogger.LogError("Failed to parse profession with id {buildMetadata.PrimaryProfessionId}", buildMetadata.PrimaryProfessionId);
            return default;
        }

        build.Primary = primaryProfession;
        if (Profession.TryParse(buildMetadata.SecondaryProfessionId, out var secondaryProfession) is false)
        {
            scopedLogger.LogError("Failed to parse profession with id {buildMetadata.SecondaryProfessionId}", buildMetadata.SecondaryProfessionId);
            return default;
        }

        build.Secondary = secondaryProfession;
        /*
         * Prepopulate the attributes first and then populate the attribute points based on ids.
         */

        if (primaryProfession != Profession.None)
        {
            build.Attributes.Add(new AttributeEntry { Attribute = primaryProfession.PrimaryAttribute });
            build.Attributes.AddRange(primaryProfession.Attributes!.Select(a => new AttributeEntry { Attribute = a }));
        }
        
        if (secondaryProfession != Profession.None)
        {
            build.Attributes.AddRange(secondaryProfession.Attributes!.Select(a => new AttributeEntry { Attribute = a }));
        }

        for(int i = 0; i < buildMetadata.AttributeCount; i++)
        {
            var attributeId = buildMetadata.AttributesIds[i];
            var maybeAttribute = build.Attributes.FirstOrDefault(a => a.Attribute!.Id == attributeId);
            if (maybeAttribute is null)
            {
                scopedLogger.LogError("Failed to parse attribute with id {attributeId} for professions {primaryProfession.Name}/{secondaryProfession.Name}", attributeId, primaryProfession.Name ?? string.Empty, secondaryProfession.Name ?? string.Empty);
                return default;
            }

            maybeAttribute!.Points = buildMetadata.AttributePoints[i];
        }

        for(int i = 0; i < 8; i++)
        {
            if (Skill.TryParse(buildMetadata.SkillIds[i], out var skill) is false)
            {
                scopedLogger.LogError("Failed to parse skill with id {skillId}", buildMetadata.SkillIds[i]);
                return default;
            }

            build.Skills.Add(skill);
        }

        scopedLogger.LogDebug("Successfully parsed build template");
        return build;
    }

    private string EncodeTemplateInner(Build build)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Building build metadata");
        var buildMetadata = new BuildMetadata
        {
            VersionNumber = 0,
            NewTemplate = true,
            Header = 14,
            PrimaryProfessionId = build.Primary.Id,
            SecondaryProfessionId = build.Secondary.Id,
            AttributeCount = build.Attributes.Where(attrEntry => attrEntry.Points > 0).Count(),
            AttributesIds = [.. build.Attributes.Where(attrEntry => attrEntry.Points > 0).OrderBy(attrEntry => attrEntry.Attribute!.Id).Select(attrEntry => attrEntry.Attribute!.Id)],
            AttributePoints = [.. build.Attributes.Where(attrEntry => attrEntry.Points > 0).OrderBy(attrEntry => attrEntry.Attribute!.Id).Select(attrEntry => attrEntry.Points)],
            SkillIds = [.. build.Skills.Select(skill => skill.Id)],
            TailPresent = true
        };

        scopedLogger.LogDebug("Encoding metadata into binary");
        var encodedBinary = BuildEncodedString(buildMetadata);
        int index = 0;
        var encodedBase64 = new List<int>();
        while (index < encodedBinary.Length)
        {
            var subset = new string([.. encodedBinary.Skip(index).Take(6)]);
            encodedBase64.Add(FromBitString(subset));
            index += 6;
        }

        var template = new string([.. encodedBase64.Select(b => DecodingLookupTable[b])]);
        return template;
    }

    private static string ToBitString(int value)
    {
        var sb = new StringBuilder();
        while(value > 0)
        {
            sb.Append(value % 2 == 1 ? 1 : 0);
            value /= 2;
        }

        while(sb.Length < 6)
        {
            sb.Append(0);
        }

        return sb.ToString();
    }

    private static BuildMetadata ParseEncodedTemplate(string template)
    {
        var curedTemplate = template.Trim();

        var buildMetadata = new BuildMetadata
        {
            Base64Decoded = [.. template.Select(c => DecodingLookupTable.IndexOf(c))],
        };
        buildMetadata.BinaryDecoded = [.. buildMetadata.Base64Decoded.Select(ToBitString)];

        var stream = new DecodeCharStream([.. buildMetadata.BinaryDecoded]);
        buildMetadata.Header = stream.Read(4);
        if (buildMetadata.Header == 14)
        {
            buildMetadata.VersionNumber = stream.Read(4);
            buildMetadata.NewTemplate = true;
        }
        else
        {
            buildMetadata.VersionNumber = buildMetadata.Header;
            buildMetadata.NewTemplate = false;
        }

        buildMetadata.ProfessionIdLength = (stream.Read(2) * 2) + 4;
        buildMetadata.PrimaryProfessionId = stream.Read(buildMetadata.ProfessionIdLength);
        buildMetadata.SecondaryProfessionId = stream.Read(buildMetadata.ProfessionIdLength);
        buildMetadata.AttributeCount = stream.Read(4);
        buildMetadata.AttributesLength = stream.Read(4) + 4;
        for (int i = 0; i < buildMetadata.AttributeCount; i++)
        {
            buildMetadata.AttributesIds.Add(stream.Read(buildMetadata.AttributesLength));
            buildMetadata.AttributePoints.Add(stream.Read(4));
        }

        buildMetadata.SkillsLength = stream.Read(4) + 8;
        for (int i = 0; i < 8; i++)
        {
            buildMetadata.SkillIds.Add(stream.Read(buildMetadata.SkillsLength));
        }

        if (stream.Position < stream.Length - 1)
        {
            buildMetadata.TailPresent = true;
        }

        return buildMetadata;
    } 

    private static string BuildEncodedString(BuildMetadata buildMetadata)
    {
        var stream = new EncodeCharStream();
        if(buildMetadata.NewTemplate || buildMetadata.Header == 14)
        {
            stream.Write(14, 4);
        }

        stream.Write(0, 4);
        
        var desiredProfessionIdLength = GetBitLength(new List<int> { buildMetadata.PrimaryProfessionId, buildMetadata.SecondaryProfessionId }.Max());
        var professionIdLength = Math.Max((desiredProfessionIdLength - 4) / 2, 0);
        var finalProfessionIdLength = (professionIdLength * 2) + 4;
        stream.Write(professionIdLength, 2);
        stream.Write(buildMetadata.PrimaryProfessionId, finalProfessionIdLength);
        stream.Write(buildMetadata.SecondaryProfessionId, finalProfessionIdLength);
        
        stream.Write(buildMetadata.AttributeCount, 4);
        var desiredAttributesLength = GetBitLength(buildMetadata.AttributesIds.Count != 0 ? buildMetadata.AttributesIds.Max() : 0);
        var attributesLength = Math.Max(desiredAttributesLength - 4, 0);
        var finalAttributesLength = attributesLength + 4;
        stream.Write(attributesLength, 4);
        for(int i = 0; i < buildMetadata.AttributeCount; i++)
        {
            stream.Write(buildMetadata.AttributesIds[i], finalAttributesLength);
            stream.Write(buildMetadata.AttributePoints[i], 4);
        }

        var desiredSkillsLength = GetBitLength(buildMetadata.SkillIds.Max());
        var skillsLength = Math.Max(desiredSkillsLength - 8, 0);
        var finalSkillsLength = skillsLength + 8;
        stream.Write(skillsLength, 4);
        for(int i = 0; i < 8; i++)
        {
            stream.Write(buildMetadata.SkillIds[i], finalSkillsLength);
        }

        if (buildMetadata.TailPresent)
        {
            stream.Write(0, 1);
        }

        return stream.GetEncodedString();
    }

    private static int GetBitLength(int value)
    {
        int decimals = 1;
        value /= 2;
        while (value > 0)
        {
            decimals++;
            value /= 2;
        }

        return decimals;
    }

    private static int FromBitString(string bitString)
    {
        var sb = new StringBuilder(bitString);
        while (sb.Length < 6)
        {
            sb.Append('0');
        }

        bitString = sb.ToString();
        var value = 0d;
        for (int i = 0; i < bitString.Length; i++)
        {
            value += bitString[i] == '1' ? Math.Pow(2, i) : 0;
        }

        return (int)value;
    }

    private static bool IsProfessionUnlocked(int professionId, uint unlockedProfessions) => (unlockedProfessions & (1 << professionId)) != 0;

    // Not using anymore. Skills are marked as locked if they are not part of the primary/secondary of the current character. Cannot rely on this for build viability checks.
    //private static bool IsSkillUnlocked(int skillId, uint[] unlockedSkills)
    //{
    //    var realIndex = skillId / 32;
    //    if (realIndex >= unlockedSkills.Length)
    //    {
    //        return false;
    //    }

    //    var shift = skillId % 32;
    //    var flag = 1U << shift;
    //    return (unlockedSkills[realIndex] & flag) != 0;
    //}
}

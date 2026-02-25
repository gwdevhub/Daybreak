using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Api;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates.Models;
using Daybreak.Shared.Services.BuildTemplates.Parsers;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Extensions;
using System.Extensions.Core;
using System.Text;
using System.Text.Json;
using Convert = System.Convert;

namespace Daybreak.Shared.Services.BuildTemplates;

public sealed class BuildTemplateManager(
    IEnumerable<ITemplateParser> templateParsers,
    ILogger<BuildTemplateManager> logger) : IBuildTemplateManager
{
    private const string DecodingLookupTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    private readonly static string BuildsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Guild Wars", "Templates", "Skills");

    private List<IBuildEntry> BuildMemoryCache { get; } = [];

    private readonly IEnumerable<ITemplateParser> templateParsers = templateParsers.ThrowIfNull(nameof(templateParsers));
    private readonly ILogger<BuildTemplateManager> logger = logger.ThrowIfNull(nameof(logger));

    public bool IsTemplate(string template)
    {
        if (template.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (template.Any(c => DecodingLookupTable.Contains(c) is false))
        {
            return false;
        }

        return true;
    }

    public SingleBuildEntry CreateSingleBuild()
    {
        var name = Guid.NewGuid().ToString();
        var entry = new SingleBuildEntry
        {
            Name = name,
            PreviousName = string.Empty,
            CreationTime = DateTime.UtcNow
        };

        this.BuildMemoryCache.Add(entry);
        return entry;
    }

    public SingleBuildEntry CreateSingleBuild(string name)
    {
        var entry = new SingleBuildEntry
        {
            Name = name,
            PreviousName = string.Empty,
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
        singleBuildEntry.ToolboxBuildId = teamBuildEntry.ToolboxBuildId;
        singleBuildEntry.IsToolboxBuild = teamBuildEntry.IsToolboxBuild;
        singleBuildEntry.CreationTime = teamBuildEntry.CreationTime;

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
            ToolboxBuildId = singleBuildEntry.ToolboxBuildId,
            IsToolboxBuild = singleBuildEntry.IsToolboxBuild,
            CreationTime = singleBuildEntry.CreationTime
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
        var encodedBuild = this.EncodeTemplate(buildEntry);

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
        File.WriteAllText(newPath, encodedBuild);

        // Remove the build from the memory cache once it's created on disk
        this.BuildMemoryCache.Remove(buildEntry);
    }

    public void RemoveBuild(IBuildEntry buildEntry)
    {
        var namePath = Path.Combine(BuildsPath, $"{buildEntry.Name}.txt");
        if (File.Exists(namePath))
        {
            File.Delete(namePath);
        }

        var previousNamePath = Path.Combine(BuildsPath, $"{buildEntry.PreviousName}.txt");
        if (File.Exists(previousNamePath))
        {
            File.Delete(previousNamePath);
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
        if (builds is null)
        {
            throw new InvalidOperationException("Failed to decode build template");
        }

        // If we got a single TeamBuildEntry (e.g., from party loadout), return it directly
        if (builds.Length == 1 && builds[0] is TeamBuildEntry teamBuild)
        {
            teamBuild.Name = randomName;
            teamBuild.PreviousName = randomName;
            return teamBuild;
        }

        // Otherwise, handle as before: single or multiple skill templates
        if (builds.Length == 1 && builds[0] is SingleBuildEntry single)
        {
            return new SingleBuildEntry
            {
                Name = randomName,
                PreviousName = randomName,
                Primary = single.Primary,
                Secondary = single.Secondary,
                Attributes = single.Attributes,
                Skills = single.Skills
            };
        }

        // Multiple templates -> TeamBuildEntry
        return new TeamBuildEntry
        {
            Name = randomName,
            PreviousName = randomName,
            Builds = [.. builds.OfType<SingleBuildEntry>().Select(b => new SingleBuildEntry
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

                // If we got a single TeamBuildEntry (e.g., from party loadout), return it directly
                if (maybeBuilds.Length == 1 && maybeBuilds[0] is TeamBuildEntry teamBuild)
                {
                    teamBuild.Name = randomName;
                    teamBuild.PreviousName = randomName;
                    build = teamBuild;
                    return true;
                }

                // Otherwise, handle as before: single or multiple skill templates
                if (maybeBuilds.Length == 1 && maybeBuilds[0] is SingleBuildEntry single)
                {
                    build = new SingleBuildEntry
                    {
                        Name = randomName,
                        PreviousName = randomName,
                        Primary = single.Primary,
                        Secondary = single.Secondary,
                        Attributes = single.Attributes,
                        Skills = single.Skills
                    };
                    return true;
                }

                // Multiple templates -> TeamBuildEntry
                build = new TeamBuildEntry
                {
                    Name = randomName,
                    PreviousName = randomName,
                    Builds = maybeBuilds.OfType<SingleBuildEntry>().Select(b => new SingleBuildEntry
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
            return this.EncodeTemplateInner(singleBuildEntry);
        }
        else if (build is TeamBuildEntry teamBuildEntry)
        {
            // If the team build has party composition, encode as party loadout
            if (teamBuildEntry.PartyComposition is { Count: > 0 })
            {
                return this.EncodeTemplateInner(teamBuildEntry);
            }

            // Otherwise, encode as space-separated skill templates
            var encodedString = new StringBuilder();
            foreach(var teamBuild in teamBuildEntry.Builds)
            {
                encodedString.Append(this.EncodeTemplateInner(teamBuild)).Append(' ');
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
        if (content.Length > 1 && !string.IsNullOrWhiteSpace(content[1]))
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
                    var legacyMetadata = JsonSerializer.Deserialize<Dictionary<string, string>>(
                            Encoding.UTF8.GetString(
                                Convert.FromBase64String(content[1])));

                    // Extract legacy metadata into proper properties
                    if (legacyMetadata is not null)
                    {
                        if (legacyMetadata.TryGetValue("SourceUrl", out var sourceUrlValue))
                        {
                            build.SourceUrl = sourceUrlValue;
                        }

                        if (legacyMetadata.TryGetValue("CreationTime", out var creationTimeString) &&
                            int.TryParse(creationTimeString, out var creationTimeUnix))
                        {
                            build.CreationTime = DateTimeOffset.FromUnixTimeSeconds(creationTimeUnix).DateTime;
                        }

                        if (legacyMetadata.TryGetValue("ToolboxBuildId", out var toolboxIdString) &&
                            int.TryParse(toolboxIdString, out var toolboxId))
                        {
                            build.ToolboxBuildId = toolboxId;
                        }

                        if (legacyMetadata.TryGetValue("IsToolboxBuild", out var isToolboxBuildString) &&
                            bool.TryParse(isToolboxBuildString, out var isToolboxBuild))
                        {
                            build.IsToolboxBuild = isToolboxBuild;
                        }

                        // Extract party composition for team builds
                        if (build is TeamBuildEntry teamBuild &&
                            legacyMetadata.TryGetValue("PartyComposition", out var partyCompositionJson) &&
                            !string.IsNullOrWhiteSpace(partyCompositionJson))
                        {
                            teamBuild.PartyComposition = JsonSerializer.Deserialize<List<PartyCompositionMetadataEntry>>(partyCompositionJson);
                        }
                    }
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

    private IBuildEntry[]? DecodeTemplatesInner(string template)
    {
        var buildTemplates = template.Split(' ').Where(s => !s.IsNullOrWhiteSpace()).ToArray();
        if (buildTemplates.Length == 0)
        {
            return [];
        }

        var builds = new List<IBuildEntry>();
        for(var i = 0; i < buildTemplates.Length; i++)
        {
            var result = this.DecodeTemplateInner(buildTemplates[i]);
            if (result is null)
            {
                return default;
            }

            builds.Add(result);
        }

        return [.. builds];
    }

    private IBuildEntry? DecodeTemplateInner(string template)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Attempting to decode template");
        
        // Decode base64 to binary
        var base64Decoded = template.Select(c => DecodingLookupTable.IndexOf(c)).ToList();
        var binaryDecoded = base64Decoded.Select(ToBitString).ToList();
        var bitString = string.Join("", binaryDecoded);
        var stream = new DecodeCharStream([.. binaryDecoded]);
        
        // Read header to determine template type
        var headerValue = stream.Read(4);
        var header = (TemplateHeader)headerValue;
        
        // Create decode context
        var context = new DecodeContext(template, bitString, stream);

        // Try skill template parsers first
        var skillParser = this.templateParsers.OfType<ITemplateParser<SkillTemplateMetadata>>().FirstOrDefault(p => p.CanDecode(header));
        if (skillParser is not null)
        {
            var buildMetadata = skillParser.Decode(context);
            scopedLogger.LogDebug("Decoded skill template. Creating build entry");
            return skillParser.CreateBuildEntry(buildMetadata);
        }

        // Try party loadout parser
        var partyLoadoutParser = this.templateParsers.OfType<ITemplateParser<PartyLoadoutTemplateMetadata>>().FirstOrDefault(p => p.CanDecode(header));
        if (partyLoadoutParser is not null)
        {
            var partyMetadata = partyLoadoutParser.Decode(context);
            scopedLogger.LogDebug("Decoded party loadout template. Creating build entry");
            return partyLoadoutParser.CreateBuildEntry(partyMetadata);
        }

        scopedLogger.LogError("No parser found for template header {header}", header);
        return default;
    }

    private string EncodeTemplateInner(SingleBuildEntry buildEntry)
    {
        return this.EncodeTemplateInner((IBuildEntry)buildEntry);
    }

    private string EncodeTemplateInner(TeamBuildEntry buildEntry)
    {
        return this.EncodeTemplateInner((IBuildEntry)buildEntry);
    }

    private string EncodeTemplateInner(IBuildEntry buildEntry)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Encoding build to template");
        
        // Find the appropriate parser for encoding
        var parser = this.templateParsers.FirstOrDefault(p => p.CanEncode(buildEntry));
        if (parser is null)
        {
            scopedLogger.LogError("No parser found that can encode this build");
            throw new InvalidOperationException("No parser found that can encode this build");
        }
        
        // Create encode context and encode
        var stream = new EncodeCharStream();
        var context = new EncodeContext(buildEntry, stream);
        var encodedBinary = parser.Encode(context);
        
        // Convert binary string to base64
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

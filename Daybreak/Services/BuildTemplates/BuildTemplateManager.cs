﻿using Daybreak.Services.BuildTemplates.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using NAudio.MediaFoundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Convert = System.Convert;

namespace Daybreak.Services.BuildTemplates;

internal sealed class BuildTemplateManager(
    ILogger<BuildTemplateManager> logger) : IBuildTemplateManager
{
    private const string DecodingLookupTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    private readonly static string BuildsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Guild Wars\\Templates\\Skills";

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
        };

        entry.CreationTime = DateTimeOffset.UtcNow;
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
            Skills = emptyBuild.Skills
        };

        entry.CreationTime = DateTimeOffset.UtcNow;
        return entry;
    }

    public TeamBuildEntry CreateTeamBuild()
    {
        var name = Guid.NewGuid().ToString();
        var entry = new TeamBuildEntry
        {
            Name = name,
            PreviousName = string.Empty,
            Builds = [this.CreateSingleBuild()]
        };

        entry.CreationTime = DateTimeOffset.UtcNow;
        return entry;
    }

    public TeamBuildEntry CreateTeamBuild(string name)
    {
        var entry = new TeamBuildEntry
        {
            Name = name,
            PreviousName = string.Empty,
            Builds = [this.CreateSingleBuild(name)]
        };

        entry.CreationTime = DateTimeOffset.UtcNow;
        return entry;
    }

    public TeamBuildEntry CreateTeamBuild(TeamBuildData teamBuildData)
    {
        var entry = this.CreateTeamBuild();
        return this.PopulateTeamBuild(entry, teamBuildData);
    }

    public TeamBuildEntry CreateTeamBuild(TeamBuildData teamBuildData, string name)
    {
        var entry = this.CreateTeamBuild();
        return this.PopulateTeamBuild(entry, teamBuildData);
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
                    JsonConvert.SerializeObject(buildEntry.Metadata, Formatting.None)))
            : string.Empty;
        File.WriteAllText(newPath, $"{encodedBuild.ToString().Trim()}\n{metadata}");
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

    public Task<Result<IBuildEntry, Exception>> GetBuild(string name)
    {
        var path = Path.Combine(BuildsPath, $"{name}.txt");
        return this.LoadBuildFromFile(path, name);
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
            if (result.TryExtractSuccess(out var buildEntry) && buildEntry is not null)
            {
                yield return buildEntry;
            }
        }
    }

    public IBuildEntry DecodeTemplate(string template)
    {
        var randomName = Guid.NewGuid().ToString();
        var maybeTemplate = this.DecodeTemplatesInner(template);
        return maybeTemplate.Switch(
            onSuccess: builds => (IBuildEntry)(builds.Length == 1 ?
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
                    Builds = builds.Select(b => new SingleBuildEntry
                    {
                        Name = randomName,
                        PreviousName = randomName,
                        Primary = b.Primary,
                        Secondary = b.Secondary,
                        Attributes = b.Attributes,
                        Skills = b.Skills
                    }).ToList()
                }),
            onFailure: exception => throw exception);
    }

    public bool TryDecodeTemplate(string template, out IBuildEntry build)
    {
        var maybeBuilds = this.DecodeTemplatesInner(template);
        (var result, var parsedBuilds) = maybeBuilds.Switch(
            onSuccess: parsedBuild => (true, parsedBuild),
            onFailure: _ => (false, default!));

        if (result)
        {
            var randomName = Guid.NewGuid().ToString();
            build = parsedBuilds.Length == 1 ?
                new SingleBuildEntry
                {
                    Name = randomName,
                    PreviousName = randomName,
                    Primary = parsedBuilds[0].Primary,
                    Secondary = parsedBuilds[0].Secondary,
                    Attributes = parsedBuilds[0].Attributes,
                    Skills = parsedBuilds[0].Skills
                } :
                new TeamBuildEntry
                {
                    Name = randomName,
                    PreviousName = randomName,
                    Builds = parsedBuilds.Select(b => new SingleBuildEntry
                    {
                        Name = randomName,
                        PreviousName = randomName,
                        Primary = b.Primary,
                        Secondary = b.Secondary,
                        Attributes = b.Attributes,
                        Skills = b.Skills
                    }).ToList()
                };

            return result;
        }

        build = default!;
        return result;
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

    private TeamBuildEntry PopulateTeamBuild(TeamBuildEntry teamBuildEntry, TeamBuildData teamBuildData)
    {
        teamBuildEntry.Builds.Clear();
        var partyCompositionMetadata = new List<PartyCompositionMetadataEntry>();
        var index = 0;
        foreach (var teamBuildPlayer in teamBuildData?.TeamBuildPlayers ?? [])
        {
            if (teamBuildPlayer is null)
            {
                continue;
            }

            var build = teamBuildPlayer.Build;
            var buildEntry = this.CreateSingleBuild();
            buildEntry.Primary = build.Primary;
            buildEntry.Secondary = build.Secondary;
            buildEntry.Attributes = build.Attributes;
            buildEntry.Skills = build.Skills;
            teamBuildEntry.Builds.Add(buildEntry);
            if (teamBuildPlayer is TeamBuildHeroData heroBuildData)
            {
                partyCompositionMetadata.Add(new PartyCompositionMetadataEntry { Type = PartyCompositionMemberType.Hero, HeroId = heroBuildData.Hero.Id, Index = index });
            }
            else if (teamBuildPlayer is TeamBuildMainPlayerData)
            {
                partyCompositionMetadata.Add(new PartyCompositionMetadataEntry { Type = PartyCompositionMemberType.MainPlayer, Index = index });
            }
            else if (teamBuildPlayer is TeamBuildPartyMemberData)
            {
                partyCompositionMetadata.Add(new PartyCompositionMetadataEntry { Type = PartyCompositionMemberType.Player, Index = index });
            }
            else if (teamBuildPlayer is TeamBuildHenchmanData)
            {
                partyCompositionMetadata.Add(new PartyCompositionMetadataEntry { Type = PartyCompositionMemberType.Henchman, Index = index });
            }

            index++;
        }

        teamBuildEntry.PartyComposition = partyCompositionMetadata;
        return teamBuildEntry;
    }

    private async Task<Result<IBuildEntry, Exception>> LoadBuildFromFile(string path, string buildName)
    {
        if (File.Exists(path) is false)
        {
            return new InvalidOperationException("Unable to find build file");
        }

        var content = await File.ReadAllLinesAsync(path);
        if (content.Length == 0)
        {
            return new InvalidOperationException("File does not contain a valid template code");
        }

        if (this.TryDecodeTemplate(content.First(), out var build) is false)
        {
            return new InvalidOperationException("Unable to parse build file");
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
                build.Metadata =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(
                        Encoding.UTF8.GetString(
                            Convert.FromBase64String(content[1])));   
            }
        }

        if (build.CreationTime == DateTimeOffset.MinValue)
        {
            build.CreationTime = new FileInfo(path).CreationTimeUtc.ToSafeDateTimeOffset();
        }

        return Result<IBuildEntry, Exception>.Success(build);
    }

    private Result<Build[], Exception> DecodeTemplatesInner(string template)
    {
        var buildTemplates = template.Split(' ').Where(s => !s.IsNullOrWhiteSpace()).ToArray();
        if (buildTemplates.Length == 0)
        {
            return new InvalidOperationException("Invalid build template");
        }

        var builds = new Build[buildTemplates.Length];
        for(var i = 0; i < builds.Length; i++)
        {
            var result = this.DecodeTemplateInner(buildTemplates[i]);
            if (result.TryExtractFailure(out var exception))
            {
                return exception!;
            }

            result.TryExtractSuccess(out var maybeBuild);
            builds[i] = maybeBuild!;
        }

        return builds;
    }

    private Result<Build, Exception> DecodeTemplateInner(string template)
    {
        this.logger.LogInformation("Attempting to decode template");
        var buildMetadata = ParseEncodedTemplate(template);
        this.logger.LogInformation("Decoded template. Beginning parsing");
        if (buildMetadata.VersionNumber != 0)
        {
            this.logger.LogError($"Expected version number to be 0 but found {buildMetadata.VersionNumber}");
            return new InvalidOperationException($"Failed to parse template");
        }

        var build = new Build()
        {
            BuildMetadata = buildMetadata,
            Skills = []
        };

        if (Profession.TryParse(buildMetadata.PrimaryProfessionId, out var primaryProfession) is false)
        {
            this.logger.LogError($"Failed to parse profession with id {buildMetadata.PrimaryProfessionId}");
            return new InvalidOperationException($"Failed to parse template");
        }

        build.Primary = primaryProfession;
        if (Profession.TryParse(buildMetadata.SecondaryProfessionId, out var secondaryProfession) is false)
        {
            this.logger.LogError($"Failed to parse profession with id {buildMetadata.SecondaryProfessionId}");
            return new InvalidOperationException($"Failed to parse template");
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
                var msg = $"Failed to parse attribute with id {attributeId} for professions {primaryProfession.Name}/{secondaryProfession.Name}";
                this.logger.LogError(msg);
                return new InvalidOperationException(msg);
            }

            maybeAttribute!.Points = buildMetadata.AttributePoints[i];
        }

        for(int i = 0; i < 8; i++)
        {
            if (Skill.TryParse(buildMetadata.SkillIds[i], out var skill) is false)
            {
                this.logger.LogError($"Failed to parse skill with id {buildMetadata.SkillIds[i]}");
                return new InvalidOperationException($"Failed to parse template");
            }

            build.Skills.Add(skill);
        }

        return build;
    }

    private string EncodeTemplateInner(Build build)
    {
        this.logger.LogInformation("Building build metadata");
        var buildMetadata = new BuildMetadata
        {
            VersionNumber = 0,
            NewTemplate = true,
            Header = 14,
            PrimaryProfessionId = build.Primary.Id,
            SecondaryProfessionId = build.Secondary.Id,
            AttributeCount = build.Attributes.Where(attrEntry => attrEntry.Points > 0).Count(),
            AttributesIds = build.Attributes.Where(attrEntry => attrEntry.Points > 0).OrderBy(attrEntry => attrEntry.Attribute!.Id).Select(attrEntry => attrEntry.Attribute!.Id).ToList(),
            AttributePoints = build.Attributes.Where(attrEntry => attrEntry.Points > 0).OrderBy(attrEntry => attrEntry.Attribute!.Id).Select(attrEntry => attrEntry.Points).ToList(),
            SkillIds = build.Skills.Select(skill => skill.Id!.Value).ToList(),
            TailPresent = true
        };

        this.logger.LogInformation("Encoding metadata into binary");
        var encodedBinary = BuildEncodedString(buildMetadata);
        int index = 0;
        var encodedBase64 = new List<int>();
        while (index < encodedBinary.Length)
        {
            var subset = new string(encodedBinary.Skip(index).Take(6).ToArray());
            encodedBase64.Add(FromBitString(subset));
            index += 6;
        }

        var template = new string(encodedBase64.Select(b => DecodingLookupTable[b]).ToArray());
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
            Base64Decoded = template.Select(c => DecodingLookupTable.IndexOf(c)).ToList(),
        };
        buildMetadata.BinaryDecoded = buildMetadata.Base64Decoded.Select(ToBitString).ToList();

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
}

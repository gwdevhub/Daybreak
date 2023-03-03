using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daybreak.Services.BuildTemplates;

public sealed class BuildTemplateManager : IBuildTemplateManager
{
    private const string DecodingLookupTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    private readonly static string BuildsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Guild Wars\\Templates\\Skills";

    private readonly ILogger<BuildTemplateManager> logger;

    public BuildTemplateManager(
        ILogger<BuildTemplateManager> logger)
    {
        this.logger = logger.ThrowIfNull(nameof(logger));
    }

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

    public BuildEntry CreateBuild()
    {
        var emptyBuild = new Build();
        var name = Guid.NewGuid().ToString();
        var entry = new BuildEntry { Build = emptyBuild, Name = name, PreviousName = string.Empty };
        return entry;
    }

    public BuildEntry CreateBuild(string name)
    {
        var emptyBuild = new Build();
        var entry = new BuildEntry { Build = emptyBuild, Name = name, PreviousName = string.Empty };
        return entry;
    }

    public void SaveBuild(BuildEntry buildEntry)
    {
        var encodedBuild = this.EncodeTemplate(buildEntry.Build!);
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
    }

    public void RemoveBuild(BuildEntry buildEntry)
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

    public async Task<Result<BuildEntry, Exception>> GetBuild(string name)
    {
        var path = Path.Combine(BuildsPath, $"{name}.txt");
        if (File.Exists(path) is false)
        {
            return new InvalidOperationException("Unable to find build file");
        }

        var content = await File.ReadAllTextAsync(path);
        if (this.TryDecodeTemplate(content, out var build) is false)
        {
            return new InvalidOperationException("Unable to parse build file");
        }

        return new BuildEntry { Build = build, Name = name, PreviousName = name };
    }

    public void ClearBuilds()
    {
        foreach(var file in Directory.GetFiles(BuildsPath, "*.txt", SearchOption.AllDirectories))
        {
            File.Delete(file);
        }
    }

    public async IAsyncEnumerable<BuildEntry> GetBuilds()
    {
        if (Directory.Exists(BuildsPath) is false)
        {
            yield break;
        }

        foreach (var file in Directory.GetFiles(BuildsPath, "*.txt", SearchOption.AllDirectories))
        {
            var maybeBuild = this.DecodeTemplateInner(await File.ReadAllTextAsync(file));
            var build = maybeBuild.Switch(
                onSuccess: build => build,
                onFailure: exception =>
                {
                    this.logger.LogError(exception, "Failed to parse build");
                    return default!;
                });
            
            if (build is not null)
            {
                yield return new BuildEntry { Build = build, Name = Path.GetRelativePath(BuildsPath, file).Replace(".txt", string.Empty), PreviousName = Path.GetRelativePath(BuildsPath, file).Replace(".txt", string.Empty) };
            }
        }
    }

    public Build DecodeTemplate(string template)
    {
        var maybeTemplate = this.DecodeTemplateInner(template);
        return maybeTemplate.Switch(
            onSuccess: build => build,
            onFailure: exception => throw exception);
    }

    public bool TryDecodeTemplate(string template, out Build build)
    {
        var maybeBuild = this.DecodeTemplateInner(template);
        (var result, var parsedBuild) = maybeBuild.Switch(
            onSuccess: parsedBuild => (true, parsedBuild),
            onFailure: _ => (false, default!));

        build = parsedBuild;
        return result;
    }

    public string EncodeTemplate(Build build)
    {
        return this.EncodeTemplateInner(build);
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
            Skills = new()
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
                this.logger.LogError($"Failed to parse attribute with id {attributeId} for professions {primaryProfession.Name}/{secondaryProfession.Name}");
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
            AttributeCount = build.Attributes.Count,
            AttributesIds = build.Attributes.Select(attrEntry => attrEntry.Attribute!.Id).ToList(),
            AttributePoints = build.Attributes.Select(attrEntry => attrEntry.Points).ToList(),
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
        buildMetadata.BinaryDecoded = buildMetadata.Base64Decoded.Select(b => ToBitString(b)).ToList();

        var stream = new DecodeCharStream(buildMetadata.BinaryDecoded.ToArray());
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
        var desiredAttributesLength = GetBitLength(buildMetadata.AttributesIds.Any() ? buildMetadata.AttributesIds.Max() : 0);
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

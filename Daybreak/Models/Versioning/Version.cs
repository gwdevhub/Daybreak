using System;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;

namespace Daybreak.Models.Versioning;

public sealed class Version : IEquatable<Version>, IComparable<Version>
{
    public static readonly Version Zero = new("0");

    private List<VersionToken> parts = [];

    public bool HasPrefix { get; set; }
    public IEnumerable<VersionToken> VersionTokens { get => this.parts; }
    public string VersionString { get => this.ToString(); }

    private Version()
    {
    }

    public Version(string version)
    {
        if (TryParseParts(version, out var parts, out var hasPrefix) is false)
        {
            throw new ArgumentException($"Provided argument is not valid: {version}");
        }

        this.parts = parts;
        this.HasPrefix = hasPrefix;
    }

    public override string ToString()
    {
        var version = string.Join('.', this.parts.OfType<VersionNumberToken>());
        if (this.parts.Last() is VersionStringToken)
        {
            version += "-" + this.parts.Last();
        }

        if (this.HasPrefix)
        {
            version = 'v' + version;
        }

        return version;
    }

    public override int GetHashCode()
    {
        var hashCode = 0;
        foreach(var versionToken in this.VersionTokens)
        {
            hashCode += versionToken.GetHashCode();
        }

        return hashCode;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Version otherVersion)
        {
            return this.Equals(otherVersion);
        }

        return base.Equals(obj);
    }

    public bool Equals(Version? other)
    {
        return this.CompareTo(other) == 0;
    }

    public int CompareTo(Version? other)
    {
        var thisTokens = this.VersionTokens.ToList();
        var otherTokens = other?.VersionTokens.ToList();
        if (otherTokens is null)
        {
            return 1;
        }

        var comparisonIndex = 0;
        for(; comparisonIndex < Math.Min(thisTokens.Count, otherTokens.Count); comparisonIndex++)
        {
            var thisVersionToken = thisTokens[comparisonIndex];
            var otherVersionToken = otherTokens[comparisonIndex];
            if (thisVersionToken is VersionNumberToken thisVersionNumberToken && otherVersionToken is VersionNumberToken otherVersionNumberToken)
            {
                if (thisVersionNumberToken.Number < otherVersionNumberToken.Number)
                {
                    return -1;
                }
                else if (thisVersionNumberToken.Number > otherVersionNumberToken.Number)
                {
                    return 1;
                }
            }
            else if (thisVersionToken is VersionNumberToken && otherVersionToken is VersionStringToken)
            {
                return 1;
            }
            else if (thisVersionToken is VersionStringToken && otherVersionToken is VersionNumberToken)
            {
                return -1;
            }
            else if (thisVersionToken is VersionStringToken thisVersionStringToken && otherVersionToken is VersionStringToken otherVersionStringToken)
            {
                return string.Compare(thisVersionStringToken.Token, otherVersionStringToken.Token, StringComparison.Ordinal);
            }
        }

        if (thisTokens.Count == otherTokens.Count)
        {
            return 0;
        }

        if (thisTokens.Count > otherTokens.Count)
        {
            return 1;
        }
        else
        {
            return -1;
        }

        throw new InvalidOperationException($"Failed to compare {this.ToString()} with {other.ToString()}");
    }

    public static Version Parse(string version)
    {
        if (TryParse(version, out var parsedVersion) is false)
        {
            throw new InvalidOperationException($"Unable to parse version: {version}");
        }

        return parsedVersion;
    }

    public static bool TryParse(string? version, out Version parsedVersion)
    {
        if (TryParseParts(version, out var parts, out var hasPrefix) is false)
        {
            parsedVersion = default!;
            return false;
        }

        for (int i = parts.Count - 1; i >= 1; i--)
        {
            if (parts[i] is VersionNumberToken && parts[i].ToString() == "0")
            {
                parts.RemoveAt(i);
            }
            else
            {
                break;
            }
        }

        parsedVersion = new Version
        {
            parts = parts,
            HasPrefix = hasPrefix
        };
        return true;
    }
    
    private static bool TryParseParts(string? version, out List<VersionToken> parts, out bool hasPrefix)
    {
        hasPrefix = false;
        parts = default!;
        if (version is null || version.IsNullOrWhiteSpace())
        {
            return false;
        }

        if (version.StartsWith('.') || version.EndsWith('.'))
        {
            return false;
        }

        var tokens = version.Trim().Split('.');
        if (tokens.Length == 0)
        {
            return false;
        }

        if (tokens[0].StartsWith('v'))
        {
            hasPrefix = true;
            tokens[0] = tokens[0][1..];
        }

        parts = [];
        for(int i = 0; i < tokens.Length - 1; i++)
        {
            var token = tokens[i];
            if (token.IsNullOrWhiteSpace() || token.All(c => char.IsDigit(c)) is false)
            {
                return false;
            }

            parts.Add(VersionToken.Parse(token));
        }

        var lastToken = tokens[^1];
        if (lastToken.Contains('-'))
        {
            var lastTokenParts = lastToken.Split('-');
            if (lastTokenParts.Length > 2 || lastTokenParts.Length < 2)
            {
                return false;
            }

            if (lastTokenParts[0].IsNullOrWhiteSpace() || lastTokenParts[0].All(c => char.IsDigit(c)) is false)
            {
                return false;
            }

            parts.Add(VersionToken.Parse(lastTokenParts[0]));
            if (lastTokenParts[1].IsNullOrWhiteSpace() || lastTokenParts[1].All(c => char.IsLetter(c)) is false)
            {
                return false;
            }

            parts.Add(VersionToken.Parse(lastTokenParts[1]));
        }
        else
        {
            if (lastToken.IsNullOrWhiteSpace() || lastToken.Any(c => char.IsDigit(c) is false))
            {
                return false;
            }

            parts.Add(VersionToken.Parse(lastToken));
        }

        return true;
    }
}

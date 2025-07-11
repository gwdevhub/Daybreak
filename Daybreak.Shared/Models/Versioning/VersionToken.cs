﻿namespace Daybreak.Shared.Models.Versioning;

public abstract class VersionToken
{
    public static VersionToken Parse(string token)
    {
        if (token.All(char.IsDigit))
        {
            var number = int.Parse(token);
            return new VersionNumberToken(number);
        }

        if (token.All(char.IsLetter))
        {
            return new VersionStringToken(token);
        }

        throw new ArgumentException($"Failed to parse version token {token}");
    }

    protected abstract string Stringify();

    public override string ToString()
    {
        return this.Stringify();
    }
}

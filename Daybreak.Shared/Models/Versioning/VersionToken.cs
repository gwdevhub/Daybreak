using System;
using System.Linq;

namespace Daybreak.Models.Versioning;

public abstract class VersionToken
{
    public static VersionToken Parse(string token)
    {
        if (token.All(c => char.IsDigit(c)))
        {
            var number = int.Parse(token);
            return new VersionNumberToken(number);
        }

        if (token.All(c => char.IsLetter(c)))
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

using Daybreak.Shared.Utils;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Daybreak.Shared.Models;

[Serializable]
public sealed class SecureString
{
    public static SecureString Empty { get => new(string.Empty); }

    private byte[]? encryptedBytes;
    private readonly byte[] key;

    private byte[]? DecryptedValue
    {
        get => this.encryptedBytes?.DecryptBytes(this.key);
        set => this.encryptedBytes = value?.EncryptBytes(this.key);
    }
    [JsonProperty("value")]
    public string? Value
    {
        get
        {
            return this.encryptedBytes?.DecryptBytes(this.key).AsString();
        }
        set
        {
            this.encryptedBytes = value?.AsBytes().EncryptBytes(this.key);
        }
    }
    private SecureString(byte[] value)
    {
        this.key = new byte[32];
        using var crypto = RandomNumberGenerator.Create();
        crypto.GetBytes(this.key);
        this.DecryptedValue = value;
    }
    public SecureString(string value)
    {
        this.key = new byte[32];
        using var crypto = RandomNumberGenerator.Create();
        crypto.GetBytes(this.key);
        this.Value = value;
    }

    public static implicit operator string?(SecureString? ss) => ss is null ? string.Empty : ss.Value;
    public static implicit operator SecureString(string s) => new(s);
    public static SecureString operator +(SecureString? ss1, SecureString? ss2)
    {
        if (ss1 is null) throw new ArgumentNullException(nameof(ss1));
        if (ss2 is null) throw new ArgumentNullException(nameof(ss2));

        return new SecureString(ss1.DecryptedValue!.Concat(ss2.DecryptedValue!).ToArray());
    }
    public static SecureString operator +(SecureString? ss1, string s2)
    {
        if (ss1 is null) throw new ArgumentNullException(nameof(ss1));

        return new SecureString(ss1.DecryptedValue!.Concat(s2.AsBytes()).ToArray());
    }
    public static SecureString operator +(SecureString? ss1, char c)
    {
        if (ss1 is null) throw new ArgumentNullException(nameof(ss1));
        
        return new SecureString(ss1.DecryptedValue!.Append(Convert.ToByte(c)).ToArray());
    }
    public static bool operator ==(SecureString? ss1, SecureString? ss2)
    {
        if (ss1 is null && ss2 is null) return true;
        if (ss1 is null) return false;
        if (ss2 is null) return false;

        var ss1Value = ss1.DecryptedValue;
        var ss2Value = ss2.DecryptedValue;

        if (ss1Value!.Length != ss2Value!.Length) return false;

        for (int i = 0; i < ss1Value.Length; i++)
        {
            if (ss1Value[i] != ss2Value[i]) return false;
        }

        return true;
    }
    public static bool operator !=(SecureString? ss1, SecureString? ss2)
    {
        return !(ss1 == ss2);
    }
    public static bool operator ==(SecureString? ss1, string s2)
    {
        if (ss1 is null) return false;

        return ss1.Value == s2;
    }
    public static bool operator !=(SecureString? ss1, string s2)
    {
        if (ss1 is null) return true;

        return !(ss1.Value == s2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is string)
        {
            return this == (obj as string)!;
        }
        else if (obj is SecureString)
        {
            return this == (obj as SecureString)!;
        }
        else
        {
            return base.Equals(obj);
        }
    }

    public override int GetHashCode()
    {
        return this.Value!.GetHashCode();
    }

    public override string ToString()
    {
        return this.Value!;
    }
}

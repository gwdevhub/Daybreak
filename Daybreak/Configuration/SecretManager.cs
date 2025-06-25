using Newtonsoft.Json.Linq;
using System;
using System.Extensions;
using System.Linq;
using System.Reflection;

namespace Daybreak.Configuration;

public static class SecretManager
{
    private static JObject? SecretsHolder;

    public static string GetSecret(SecretKeys secretKey)
    {
        SecretsHolder ??= LoadSecrets();
        return SecretsHolder.Value<string>(secretKey.Key) ?? throw new InvalidOperationException($"Could not find secret by key {secretKey.Key}");
    }

    public static T GetSecret<T>(SecretKeys secretKey)
    {
        SecretsHolder ??= LoadSecrets();
        return SecretsHolder.Value<T>(secretKey.Key) ?? throw new InvalidOperationException($"Could not find secret by key {secretKey.Key}");
    }

    private static JObject LoadSecrets()
    {
        var serializedSecrets = Assembly.GetExecutingAssembly().GetManifestResourceStream("Daybreak.secrets.json")?.ReadAllBytes().GetString() ?? throw new InvalidOperationException("Could not load Daybreak.secrets.json from assembly");
        serializedSecrets = TrimUnwantedCharacters(serializedSecrets);
        return JObject.Parse(serializedSecrets);
    }

    private static string TrimUnwantedCharacters(string s)
    {
        return new string([.. s.Where(c =>
            char.IsWhiteSpace(c) ||
            char.IsLetterOrDigit(c) ||
            char.IsSymbol(c) ||
            char.IsSeparator(c) ||
            char.IsPunctuation(c))]);
    }
}

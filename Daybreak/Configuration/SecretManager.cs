using System.Extensions;
using System.Reflection;
using System.Text.Json;

namespace Daybreak.Configuration;

public static class SecretManager
{
    private static JsonDocument? SecretsHolder;

    public static string GetSecret(SecretKeys secretKey)
    {
        SecretsHolder ??= LoadSecrets();
        if (SecretsHolder.RootElement.TryGetProperty(secretKey.Key, out var element))
        {
            return element.GetString() ?? throw new InvalidOperationException($"Could not find secret by key {secretKey.Key}");
        }

        throw new InvalidOperationException($"Could not find secret by key {secretKey.Key}");
    }

    public static T GetSecret<T>(SecretKeys secretKey)
    {
        SecretsHolder ??= LoadSecrets();
        if (SecretsHolder.RootElement.TryGetProperty(secretKey.Key, out var element))
        {
            return element.Deserialize<T>() ?? throw new InvalidOperationException($"Could not find secret by key {secretKey.Key}");
        }

        throw new InvalidOperationException($"Could not find secret by key {secretKey.Key}");
    }

    private static JsonDocument LoadSecrets()
    {
        var serializedSecrets = Assembly.GetExecutingAssembly().GetManifestResourceStream("Daybreak.secrets.json")?.ReadAllBytes().GetString() ?? throw new InvalidOperationException("Could not load Daybreak.secrets.json from assembly");
        serializedSecrets = TrimUnwantedCharacters(serializedSecrets);
        return JsonDocument.Parse(serializedSecrets);
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

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
            return element.GetString() ?? string.Empty;
        }

        return string.Empty;
    }

    private static JsonDocument LoadSecrets()
    {
        try
        {
            var serializedSecrets = Assembly.GetExecutingAssembly().GetManifestResourceStream("Daybreak.secrets.json")?.ReadAllBytes().GetString() ?? throw new InvalidOperationException("Could not load Daybreak.secrets.json from assembly");
            serializedSecrets = TrimUnwantedCharacters(serializedSecrets);
            return JsonDocument.Parse(serializedSecrets);
        }
        catch
        {
            return JsonDocument.Parse("{}");
        }
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

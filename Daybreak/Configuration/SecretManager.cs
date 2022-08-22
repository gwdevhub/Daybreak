using Newtonsoft.Json.Linq;
using System.Extensions;
using System.Linq;
using System.Reflection;

namespace Daybreak.Configuration;

public static class SecretManager
{
    private static JObject SecretsHolder;

    public static string GetSecret(SecretKeys secretKey)
    {
        if (SecretsHolder is null)
        {
            LoadSecrets();
        }

        return SecretsHolder.Value<string>(secretKey.Key);
    }

    private static void LoadSecrets()
    {
        var serializedSecrets = Assembly.GetExecutingAssembly().GetManifestResourceStream("Daybreak.secrets.json").ReadAllBytes().GetString();
        serializedSecrets = TrimUnwantedCharacters(serializedSecrets);
        SecretsHolder = JObject.Parse(serializedSecrets);
    }

    private static string TrimUnwantedCharacters(string s)
    {
        return new string(s.Where(c =>
            char.IsWhiteSpace(c) ||
            char.IsLetterOrDigit(c) ||
            char.IsSymbol(c) ||
            char.IsSeparator(c) ||
            char.IsPunctuation(c))
            .ToArray());
    }
}

using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Credentials;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Extensions;
using System.Security.Cryptography;
using System.Text;
using Convert = System.Convert;

namespace Daybreak.Services.Credentials;

internal sealed class CredentialManager : ICredentialManager
{
    private static readonly byte[] Entropy = Convert.FromBase64String("uXB8Vmz5MmuDar36v8SRGzpALi0Wv5Gx");
    private readonly ILogger<CredentialManager> logger;
    private readonly ILiveUpdateableOptions<CredentialManagerOptions> liveOptions;

    public CredentialManager(
        ILogger<CredentialManager> logger,
        ILiveUpdateableOptions<CredentialManagerOptions> liveOptions)
    {
        this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
        this.logger = logger.ThrowIfNull(nameof(logger));
    }

    public bool TryGetCredentialsByIdentifier(string identifier, out LoginCredentials? loginCredentials)
    {
        loginCredentials = default;
        if (this.GetCredentialList().FirstOrDefault(l => l.Identifier == identifier) is LoginCredentials foundCredentials)
        {
            loginCredentials = foundCredentials;
            return true;
        }

        return false;
    }

    public List<LoginCredentials> GetCredentialList()
    {
        this.logger.LogDebug("Retrieving credentials");
        var config = this.liveOptions.Value;
        if (config.ProtectedLoginCredentials is null || config.ProtectedLoginCredentials.Count == 0)
        {
            this.logger.LogDebug("No credentials found");
            return [];
        }

        return [.. config
            .ProtectedLoginCredentials
            .Select(this.UnprotectCredentials)
            .Where(this.CredentialsUnprotected)
            .Select(this.ExtractCredentials)];
    }

    public void StoreCredentials(List<LoginCredentials> loginCredentials)
    {
        this.logger.LogDebug("Storing credentials");
        this.liveOptions.Value.ProtectedLoginCredentials = loginCredentials
            .Select(this.ProtectCredentials)
            .Where(this.CredentialsProtected)
            .Select(this.ExtractProtectedCredentials)
            .ToList();
        this.liveOptions.UpdateOption();
    }

    public LoginCredentials CreateUniqueCredentials()
    {
        return new LoginCredentials
        {
            Username = string.Empty,
            Password = string.Empty,
            Identifier = Guid.NewGuid().ToString()
        };
    }

    private Optional<LoginCredentials> UnprotectCredentials(ProtectedLoginCredentials protectedLoginCredentials)
    {
        try
        {
            var usrbytes = Convert.FromBase64String(protectedLoginCredentials.ProtectedUsername!);
            var psdBytes = Convert.FromBase64String(protectedLoginCredentials.ProtectedPassword!);
            return new LoginCredentials
            {
                Identifier = protectedLoginCredentials.Identifier,
                Username = Encoding.UTF8.GetString(ProtectedData.Unprotect(usrbytes, Entropy, DataProtectionScope.LocalMachine)),
                Password = Encoding.UTF8.GetString(ProtectedData.Unprotect(psdBytes, Entropy, DataProtectionScope.LocalMachine)),
            };
        }
        catch (Exception e)
        {
            this.logger.LogError($"Unable to retrieve credentials. Details: {e}");
            return Optional.None<LoginCredentials>();
        }
    }

    private Optional<ProtectedLoginCredentials> ProtectCredentials(LoginCredentials loginCredentials)
    {
        try
        {
            var usrBytes = Encoding.UTF8.GetBytes(loginCredentials.Username!);
            var psdBytes = Encoding.UTF8.GetBytes(loginCredentials.Password!);
            return new ProtectedLoginCredentials
            {
                Identifier = loginCredentials.Identifier,
                ProtectedUsername = Convert.ToBase64String(ProtectedData.Protect(usrBytes, Entropy, DataProtectionScope.LocalMachine)),
                ProtectedPassword = Convert.ToBase64String(ProtectedData.Protect(psdBytes, Entropy, DataProtectionScope.LocalMachine)),
            };
        }
        catch
        {
            return Optional.None<ProtectedLoginCredentials>();
        }
    }

    private bool CredentialsUnprotected(Optional<LoginCredentials> optional)
    {
        return optional
            .Switch(onSome: _ => true, onNone: () => false)
            .ExtractValue();
    }

    private bool CredentialsProtected(Optional<ProtectedLoginCredentials> optional)
    {
        return optional
            .Switch(onSome: _ => true, onNone: () => false)
            .ExtractValue();
    }

    private ProtectedLoginCredentials ExtractProtectedCredentials(Optional<ProtectedLoginCredentials> optional)
    {
        return optional.ExtractValue()!;
    }

    private LoginCredentials ExtractCredentials(Optional<LoginCredentials> optional)
    {
        return optional.ExtractValue()!;
    }
}

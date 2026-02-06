using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Extensions;
using System.Extensions.Core;
using System.Text;
using Convert = System.Convert;

namespace Daybreak.Services.Credentials;


internal sealed class CredentialManager(
    ICredentialProtector credentialProtector,
    IOptionsProvider optionsProvider,
    IOptionsMonitor<CredentialManagerOptions> liveOptions,
    ILogger<CredentialManager> logger) : ICredentialManager
{
    private readonly ICredentialProtector credentialProtector = credentialProtector.ThrowIfNull(nameof(credentialProtector));
    private readonly ILogger<CredentialManager> logger = logger.ThrowIfNull(nameof(logger));
    private readonly IOptionsMonitor<CredentialManagerOptions> liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull(nameof(optionsProvider));

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
        var config = this.liveOptions.CurrentValue;
        if (config.ProtectedLoginCredentials is null || config.ProtectedLoginCredentials.Count == 0)
        {
            this.logger.LogDebug("No credentials found");
            return [];
        }

        return [.. config
            .ProtectedLoginCredentials
            .Select(this.UnprotectCredentials)
            .OfType<LoginCredentials>()];
    }

    public void StoreCredentials(List<LoginCredentials> loginCredentials)
    {
        this.logger.LogDebug("Storing credentials");
        var options = this.liveOptions.CurrentValue;
        options.ProtectedLoginCredentials = [.. loginCredentials
            .Select(this.ProtectCredentials)
            .OfType<ProtectedLoginCredentials>()];
        this.optionsProvider.SaveOption(options);
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

    private LoginCredentials? UnprotectCredentials(ProtectedLoginCredentials protectedLoginCredentials)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var usrBytes = protectedLoginCredentials.ProtectedUsername is not null ? Convert.FromBase64String(protectedLoginCredentials.ProtectedUsername) : default;
            var psdBytes = protectedLoginCredentials.ProtectedPassword is not null ? Convert.FromBase64String(protectedLoginCredentials.ProtectedPassword) : default;

            var decryptedUsr = usrBytes is not null ? this.credentialProtector.Unprotect(usrBytes) : default;
            var decryptedPsd = psdBytes is not null ? this.credentialProtector.Unprotect(psdBytes) : default;

            return new LoginCredentials
            {
                Identifier = protectedLoginCredentials.Identifier,
                Username = decryptedUsr is not null ? Encoding.UTF8.GetString(decryptedUsr) : default,
                Password = decryptedPsd is not null ? Encoding.UTF8.GetString(decryptedPsd) : default,
            };
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Unable to retrieve credentials");
            return default;
        }
    }

    private ProtectedLoginCredentials? ProtectCredentials(LoginCredentials loginCredentials)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var usrBytes = loginCredentials.Username is not null ? Encoding.UTF8.GetBytes(loginCredentials.Username) : default;
            var psdBytes = loginCredentials.Password is not null ? Encoding.UTF8.GetBytes(loginCredentials.Password) : default;

            var encryptedUsr = usrBytes is not null ? this.credentialProtector.Protect(usrBytes) : default;
            var encryptedPsd = psdBytes is not null ? this.credentialProtector.Protect(psdBytes) : default;

            return new ProtectedLoginCredentials
            {
                Identifier = loginCredentials.Identifier,
                ProtectedUsername = encryptedUsr is not null ? Convert.ToBase64String(encryptedUsr) : default,
                ProtectedPassword = encryptedPsd is not null ? Convert.ToBase64String(encryptedPsd) : default,
            };
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Unable to encrypt credentials");
            return default;
        }
    }
}

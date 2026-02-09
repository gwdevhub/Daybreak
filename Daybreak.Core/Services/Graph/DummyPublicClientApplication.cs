using Microsoft.Identity.Client;
using System.Security;

namespace Daybreak.Services.Graph;

/// <summary>
/// A dummy implementation of IPublicClientApplication used as a fallback
/// when MSAL initialization fails. Returns empty/null results instead of throwing
/// so the application can gracefully handle unavailable authentication.
/// </summary>
internal sealed class DummyPublicClientApplication : IPublicClientApplication
{
    public bool IsSystemWebViewAvailable => false;
    public IAppConfig AppConfig { get; } = default!;
    public ITokenCache UserTokenCache { get; } = default!;
    public string Authority { get; } = string.Empty;

    public AcquireTokenByIntegratedWindowsAuthParameterBuilder AcquireTokenByIntegratedWindowsAuth(IEnumerable<string> scopes)
    {
        throw new InvalidOperationException("MSAL is not available. Authentication cannot be performed.");
    }

    public AcquireTokenByUsernamePasswordParameterBuilder AcquireTokenByUsernamePassword(IEnumerable<string> scopes, string username, SecureString password)
    {
        throw new InvalidOperationException("MSAL is not available. Authentication cannot be performed.");
    }

    public AcquireTokenByUsernamePasswordParameterBuilder AcquireTokenByUsernamePassword(IEnumerable<string> scopes, string username, string password)
    {
        throw new InvalidOperationException("MSAL is not available. Authentication cannot be performed.");
    }

    public AcquireTokenInteractiveParameterBuilder AcquireTokenInteractive(IEnumerable<string> scopes)
    {
        throw new InvalidOperationException("MSAL is not available. Authentication cannot be performed.");
    }

    public AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, IAccount account)
    {
        throw new InvalidOperationException("MSAL is not available. Authentication cannot be performed.");
    }

    public AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, string loginHint)
    {
        throw new InvalidOperationException("MSAL is not available. Authentication cannot be performed.");
    }

    public AcquireTokenWithDeviceCodeParameterBuilder AcquireTokenWithDeviceCode(IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback)
    {
        throw new InvalidOperationException("MSAL is not available. Authentication cannot be performed.");
    }

    public Task<IAccount?> GetAccountAsync(string identifier)
    {
        return Task.FromResult<IAccount?>(null);
    }

    public Task<IEnumerable<IAccount>> GetAccountsAsync()
    {
        return Task.FromResult<IEnumerable<IAccount>>([]);
    }

    public Task<IEnumerable<IAccount>> GetAccountsAsync(string userFlow)
    {
        return Task.FromResult<IEnumerable<IAccount>>([]);
    }

    public Task RemoveAsync(IAccount account)
    {
        return Task.CompletedTask;
    }
}

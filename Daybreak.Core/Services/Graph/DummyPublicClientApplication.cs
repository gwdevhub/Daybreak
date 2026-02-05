using Microsoft.Identity.Client;
using System.Security;

namespace Daybreak.Services.Graph;

internal sealed class DummyPublicClientApplication : IPublicClientApplication
{
    public bool IsSystemWebViewAvailable { get; }
    public IAppConfig AppConfig { get; } = default!;
    public ITokenCache UserTokenCache { get; } = default!;
    public string Authority { get; } = string.Empty;

    public AcquireTokenByIntegratedWindowsAuthParameterBuilder AcquireTokenByIntegratedWindowsAuth(IEnumerable<string> scopes)
    {
        throw new NotImplementedException();
    }

    public AcquireTokenByUsernamePasswordParameterBuilder AcquireTokenByUsernamePassword(IEnumerable<string> scopes, string username, SecureString password)
    {
        throw new NotImplementedException();
    }

    public AcquireTokenByUsernamePasswordParameterBuilder AcquireTokenByUsernamePassword(IEnumerable<string> scopes, string username, string password)
    {
        throw new NotImplementedException();
    }

    public AcquireTokenInteractiveParameterBuilder AcquireTokenInteractive(IEnumerable<string> scopes)
    {
        throw new NotImplementedException();
    }

    public AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, IAccount account)
    {
        throw new NotImplementedException();
    }

    public AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, string loginHint)
    {
        throw new NotImplementedException();
    }

    public AcquireTokenWithDeviceCodeParameterBuilder AcquireTokenWithDeviceCode(IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback)
    {
        throw new NotImplementedException();
    }

    public Task<IAccount> GetAccountAsync(string identifier)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IAccount>> GetAccountsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IAccount>> GetAccountsAsync(string userFlow)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(IAccount account)
    {
        throw new NotImplementedException();
    }
}

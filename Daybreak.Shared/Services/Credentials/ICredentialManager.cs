using Daybreak.Shared.Models;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.Credentials;

public interface ICredentialManager
{
    void StoreCredentials(List<LoginCredentials> loginCredentials);
    List<LoginCredentials> GetCredentialList();
    bool TryGetCredentialsByIdentifier(string identifier, out LoginCredentials? loginCredentials);
    LoginCredentials CreateUniqueCredentials();
}

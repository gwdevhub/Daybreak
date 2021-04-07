using Daybreak.Models;

namespace Daybreak.Services.Credentials
{
    public interface ICredentialManager
    {
        void StoreCredentials(LoginCredentials loginCredentials);
        LoginCredentials GetCredentials();
    }
}
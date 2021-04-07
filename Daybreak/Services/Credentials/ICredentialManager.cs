using Daybreak.Models;
using System.Extensions;

namespace Daybreak.Services.Credentials
{
    public interface ICredentialManager
    {
        void StoreCredentials(LoginCredentials loginCredentials);
        Optional<LoginCredentials> GetCredentials();
    }
}
using Daybreak.Models;
using System.Collections.Generic;
using System.Extensions;
using System.Threading.Tasks;

namespace Daybreak.Services.Credentials
{
    public interface ICredentialManager
    {
        Task StoreCredentials(List<LoginCredentials> loginCredentials);
        Task<List<LoginCredentials>> GetCredentialList();
        Task<Optional<LoginCredentials>> GetDefaultCredentials();
    }
}
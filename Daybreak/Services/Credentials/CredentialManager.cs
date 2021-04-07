using Daybreak.Models;

namespace Daybreak.Services.Credentials
{
    public sealed class CredentialManager : ICredentialManager
    {
        private const string Target = "GuildwarsLogin";

        public LoginCredentials GetCredentials()
        {
            var credential = Meziantou.Framework.Win32.CredentialManager.ReadCredential(Target);
            return new LoginCredentials { Username = credential.UserName, Password = credential.Password };
        }

        public void StoreCredentials(LoginCredentials loginCredentials)
        {
            Meziantou.Framework.Win32.CredentialManager.WriteCredential(Target, loginCredentials.Username, loginCredentials.Password, Meziantou.Framework.Win32.CredentialPersistence.LocalMachine);
        }
    }
}

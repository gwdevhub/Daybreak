using Daybreak.Models;
using Daybreak.Services.Configuration;
using Daybreak.Services.Logging;
using Daybreak.Utils;
using System;
using System.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace Daybreak.Services.Credentials
{
    public sealed class CredentialManager : ICredentialManager
    {
        private static readonly byte[] Entropy = Convert.FromBase64String("R3VpbGR3YXJz");
        private readonly ILogger logger;
        private readonly IConfigurationManager configurationManager;

        public CredentialManager(
            ILogger logger,
            IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
            this.logger = logger.ThrowIfNull(nameof(logger));
        }

        public Optional<LoginCredentials> GetCredentials()
        {
            this.logger.LogInformation("Retrieving credentials");
            var config = this.configurationManager.GetConfiguration();
            if (string.IsNullOrEmpty(config.ProtectedUsername) ||
                string.IsNullOrEmpty(config.ProtectedPassword))
            {
                this.logger.LogInformation("No credentials found");
                return Optional.None<LoginCredentials>();
            }

            try
            {
                var usrbytes = Convert.FromBase64String(config.ProtectedUsername);
                var psdBytes = Convert.FromBase64String(config.ProtectedPassword);
                return new LoginCredentials
                {
                    Username = Encoding.UTF8.GetString(ProtectedData.Unprotect(usrbytes, Entropy, DataProtectionScope.LocalMachine)),
                    Password = Encoding.UTF8.GetString(ProtectedData.Unprotect(psdBytes, Entropy, DataProtectionScope.LocalMachine))
                };
            }
            catch(Exception e)
            {
                this.logger.LogError($"Unable to retrieve credentials. Details: {e}");
                return Optional.None<LoginCredentials>();
            }
        }

        public void StoreCredentials(LoginCredentials loginCredentials)
        {
            this.logger.LogInformation("Storing credentials");
            var usrBytes = Encoding.UTF8.GetBytes(loginCredentials.Username);
            var psdBytes = Encoding.UTF8.GetBytes(loginCredentials.Password);
            var config = this.configurationManager.GetConfiguration();
            config.ProtectedUsername = Convert.ToBase64String(ProtectedData.Protect(usrBytes, Entropy, DataProtectionScope.LocalMachine));
            config.ProtectedPassword = Convert.ToBase64String(ProtectedData.Protect(psdBytes, Entropy, DataProtectionScope.LocalMachine));
            this.configurationManager.SaveConfiguration(config);
        }
    }
}

using Daybreak.Models;
using Daybreak.Services.Configuration;
using Daybreak.Services.Logging;
using Daybreak.Utils;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public Task<Optional<LoginCredentials>> GetDefaultCredentials()
        {
            return Task.Run(async () =>
            {
                this.logger.LogInformation("Retrieving default credentials");
                var defaultCredentials = (await this.GetCredentialList())
                    .Where(creds => creds.Default)
                    .ToList();
                if (defaultCredentials.Count == 0)
                {
                    this.logger.LogWarning("No default credentials");
                    return Optional.None<LoginCredentials>();
                }

                if (defaultCredentials.Count > 1)
                {
                    this.logger.LogError("Multiple credentials set as default");
                    return Optional.None<LoginCredentials>();
                }

                return defaultCredentials.FirstOrDefault();
            });
        }
        public Task<List<LoginCredentials>> GetCredentialList()
        {
            return Task.Run(() =>
            {
                this.logger.LogInformation("Retrieving credentials");
                var config = this.configurationManager.GetConfiguration();
                if (config.ProtectedLoginCredentials is null || config.ProtectedLoginCredentials.Count == 0)
                {
                    this.logger.LogInformation("No credentials found");
                    return new List<LoginCredentials>();
                }

                return config
                    .ProtectedLoginCredentials
                    .Select(UnprotectCredentials)
                    .Where(CredentialsUnprotected)
                    .Select(ExtractCredentials)
                    .ToList();
            });
        }
        public Task StoreCredentials(List<LoginCredentials> loginCredentials)
        {
            return Task.Run(() =>
            {
                this.logger.LogInformation("Storing credentials");
                var config = this.configurationManager.GetConfiguration();
                config.ProtectedLoginCredentials = loginCredentials
                    .Select(ProtectCredentials)
                    .Where(CredentialsProtected)
                    .Select(ExtractProtectedCredentials)
                    .ToList();
                this.configurationManager.SaveConfiguration(config);
            });   
        }

        private Optional<LoginCredentials> UnprotectCredentials(ProtectedLoginCredentials protectedLoginCredentials)
        {
            try
            {
                var usrbytes = Convert.FromBase64String(protectedLoginCredentials.ProtectedUsername);
                var psdBytes = Convert.FromBase64String(protectedLoginCredentials.ProtectedPassword);
                return new LoginCredentials
                {
                    Username = Encoding.UTF8.GetString(ProtectedData.Unprotect(usrbytes, Entropy, DataProtectionScope.LocalMachine)),
                    Password = Encoding.UTF8.GetString(ProtectedData.Unprotect(psdBytes, Entropy, DataProtectionScope.LocalMachine)),
                    CharacterName = protectedLoginCredentials.CharacterName,
                    Default = protectedLoginCredentials.Default
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
                var usrBytes = Encoding.UTF8.GetBytes(loginCredentials.Username);
                var psdBytes = Encoding.UTF8.GetBytes(loginCredentials.Password);
                return new ProtectedLoginCredentials
                {
                    ProtectedUsername = Convert.ToBase64String(ProtectedData.Protect(usrBytes, Entropy, DataProtectionScope.LocalMachine)),
                    ProtectedPassword = Convert.ToBase64String(ProtectedData.Protect(psdBytes, Entropy, DataProtectionScope.LocalMachine)),
                    CharacterName = loginCredentials.CharacterName,
                    Default = loginCredentials.Default
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
            return optional.ExtractValue();
        }
        private LoginCredentials ExtractCredentials(Optional<LoginCredentials> optional)
        {
            return optional.ExtractValue();
        }
    }
}

using Daybreak.Shared.Services.Credentials;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Daybreak.Windows.Services.Credentials;

/// <summary>
/// Windows-specific credential protector using DPAPI (Data Protection API).
/// </summary>
public sealed class CredentialProtector(ILogger<CredentialProtector> logger) : ICredentialProtector
{
    private static readonly byte[] Entropy = Convert.FromBase64String("uXB8Vmz5MmuDar36v8SRGzpALi0Wv5Gx");

    private readonly ILogger<CredentialProtector> logger = logger;
    public byte[]? Protect(byte[] data)
    {
        try
        {
            return ProtectedData.Protect(data, Entropy, DataProtectionScope.LocalMachine);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to protect data using DPAPI");
            return null;
        }
    }

    public byte[]? Unprotect(byte[] data)
    {
        try
        {
            return ProtectedData.Unprotect(data, Entropy, DataProtectionScope.LocalMachine);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to unprotect data using DPAPI");
            return null;
        }
    }
}

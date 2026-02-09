using Daybreak.Shared.Services.Credentials;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Daybreak.Linux.Services.Credentials;

/// <summary>
/// Linux-specific credential protector using AES encryption with a machine-derived key.
/// The key is derived from /etc/machine-id which is unique per Linux installation.
/// </summary>
public sealed class CredentialProtector : ICredentialProtector
{
    private const string MachineIdPath = "/etc/machine-id";
    private const string FallbackMachineIdPath = "/var/lib/dbus/machine-id";
    private static readonly byte[] Salt = Convert.FromBase64String("uXB8Vmz5MmuDar36v8SRGzpALi0Wv5Gx");

    private readonly ILogger<CredentialProtector> logger;
    private readonly byte[]? derivedKey;

    public CredentialProtector(ILogger<CredentialProtector> logger)
    {
        this.logger = logger;
        this.derivedKey = this.DeriveKeyFromMachineId();
    }

    public byte[]? Protect(byte[] data)
    {
        if (this.derivedKey is null)
        {
            this.logger.LogError("Cannot protect data: machine key not available");
            return null;
        }

        try
        {
            using var aes = Aes.Create();
            aes.Key = this.derivedKey;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);

            // Prepend IV to the encrypted data so we can decrypt later
            var result = new byte[aes.IV.Length + encrypted.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

            return result;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to protect data using AES");
            return null;
        }
    }

    public byte[]? Unprotect(byte[] data)
    {
        if (this.derivedKey is null)
        {
            this.logger.LogError("Cannot unprotect data: machine key not available");
            return null;
        }

        try
        {
            using var aes = Aes.Create();
            aes.Key = this.derivedKey;

            // Extract IV from the beginning of the data
            var iv = new byte[aes.BlockSize / 8];
            var encrypted = new byte[data.Length - iv.Length];

            Buffer.BlockCopy(data, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(data, iv.Length, encrypted, 0, encrypted.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to unprotect data using AES");
            return null;
        }
    }

    private byte[]? DeriveKeyFromMachineId()
    {
        try
        {
            string? machineId = null;

            if (File.Exists(MachineIdPath))
            {
                machineId = File.ReadAllText(MachineIdPath).Trim();
            }
            else if (File.Exists(FallbackMachineIdPath))
            {
                machineId = File.ReadAllText(FallbackMachineIdPath).Trim();
            }

            if (string.IsNullOrEmpty(machineId))
            {
                this.logger.LogWarning("Could not read machine-id. Falling back to hostname");
                machineId = Environment.MachineName;
            }

            // Derive a 256-bit key using PBKDF2
            return Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(machineId),
                Salt,
                iterations: 100000,
                HashAlgorithmName.SHA256,
                outputLength: 32); // 256 bits for AES-256
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to derive encryption key from machine-id");
            return null;
        }
    }
}

namespace Daybreak.Shared.Services.Credentials;

/// <summary>
/// Platform-specific credential protection service.
/// Encrypts and decrypts credential data using platform-appropriate methods.
/// </summary>
public interface ICredentialProtector
{
    /// <summary>
    /// Encrypts the given plaintext bytes.
    /// </summary>
    /// <param name="data">The data to encrypt.</param>
    /// <returns>The encrypted data, or null if encryption failed.</returns>
    byte[]? Protect(byte[] data);

    /// <summary>
    /// Decrypts the given encrypted bytes.
    /// </summary>
    /// <param name="data">The encrypted data.</param>
    /// <returns>The decrypted data, or null if decryption failed.</returns>
    byte[]? Unprotect(byte[] data);
}

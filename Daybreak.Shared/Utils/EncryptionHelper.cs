﻿using System.IO;
using System.Security.Cryptography;

namespace Daybreak.Shared.Utils;

internal static class EncryptionHelper
{
    private static Aes Aes { get; } = CreateDefault();
    private static int Iterations { get; } = 10000;
    private static RandomNumberGenerator Rng { get; } = RandomNumberGenerator.Create();

    public static int BlockSize { get => Aes.BlockSize; }

    public static byte[] EncryptBytes(this byte[] bytes, byte[] key)
    {
        var saltBytes = Generate128BitsOfRandomEntropy();
        var ivBytes = Generate128BitsOfRandomEntropy();

        using var password = new Rfc2898DeriveBytes(key, saltBytes, Iterations, HashAlgorithmName.SHA512);
        var keyBytes = password.GetBytes(Aes.KeySize / 8);
        using var encryptor = Aes.CreateEncryptor(keyBytes, ivBytes);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.FlushFinalBlock();
        // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
        using var encryptedMemoryStream = new MemoryStream((int)(saltBytes.Length + ivBytes.Length + memoryStream.Length));
        encryptedMemoryStream.Write(saltBytes, 0, saltBytes.Length);
        encryptedMemoryStream.Write(ivBytes, 0, ivBytes.Length);
        encryptedMemoryStream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
        return encryptedMemoryStream.ToArray();
    }

    public static byte[] DecryptBytes(this byte[] bytes, byte[] key)
    {
        var saltBytes = new byte[Aes.BlockSize / 8];
        var ivBytes = new byte[Aes.BlockSize / 8];
        var cipherBytes = new byte[bytes.Length - (Aes.BlockSize / 4)];

        using var encryptedStream = new MemoryStream(bytes);
        encryptedStream.Read(saltBytes, 0, saltBytes.Length);
        encryptedStream.Read(ivBytes, 0, ivBytes.Length);
        encryptedStream.Read(cipherBytes, 0, cipherBytes.Length);

        using var password = new Rfc2898DeriveBytes(key, saltBytes, Iterations, HashAlgorithmName.SHA512);
        var keyBytes = password.GetBytes(Aes.KeySize / 8);
        using var decryptor = Aes.CreateDecryptor(keyBytes, ivBytes);
        using var memoryStream = new MemoryStream(cipherBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        var plainTextBytes = new byte[memoryStream.Length];
        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        return [.. plainTextBytes.Take(decryptedByteCount)];
    }

    private static byte[] Generate128BitsOfRandomEntropy()
    {
        var randomBytes = new byte[16];
        Rng.GetBytes(randomBytes);
        return randomBytes;
    }

    private static Aes CreateDefault()
    {
        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }
}

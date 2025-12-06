using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Backend.Services;

public static class AesEncryption
{
    private const int NonceSize = 12;
    private const int TagSize = 16;

    private static byte[] GetKey()
    {
        // Prefer environment variable for secrets
        var b64 = Environment.GetEnvironmentVariable("AES_KEY_BASE64");

        // If env var not present, try appsettings.json (Encryption:AES_KEY_BASE64)
        if (string.IsNullOrEmpty(b64))
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

                var config = builder.Build();
                b64 = config["Encryption:AES_KEY_BASE64"];
            }
            catch
            {
                // ignore and let validation below throw a helpful message
                b64 = null;
            }
        }

        if (string.IsNullOrEmpty(b64))
            throw new InvalidOperationException("AES key not configured. Set environment variable AES_KEY_BASE64 or put Encryption:AES_KEY_BASE64 in appsettings.json (base64 of 32 bytes).");

        byte[] key;
        try
        {
            key = Convert.FromBase64String(b64);
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException("AES key is not a valid Base64 string.", ex);
        }

        if (key.Length != 32)
            throw new InvalidOperationException("AES key must be 32 bytes (base64 of 32 bytes) for AES-256.");

        return key;
    }

    public static string? Encrypt(string? plainText)
    {
        if (plainText == null) return null;

        var key = GetKey();
        var plaintextBytes = Encoding.UTF8.GetBytes(plainText);

        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        var cipher = new byte[plaintextBytes.Length];
        var tag = new byte[TagSize];

        using var aes = new AesGcm(key);
        aes.Encrypt(nonce, plaintextBytes, cipher, tag, null);

        var outBytes = new byte[NonceSize + TagSize + cipher.Length];
        Buffer.BlockCopy(nonce, 0, outBytes, 0, NonceSize);
        Buffer.BlockCopy(tag, 0, outBytes, NonceSize, TagSize);
        Buffer.BlockCopy(cipher, 0, outBytes, NonceSize + TagSize, cipher.Length);

        return Convert.ToBase64String(outBytes);
    }

    public static string? Decrypt(string? base64)
    {
        if (base64 == null) return null;

        var key = GetKey();
        byte[] all;
        try
        {
            all = Convert.FromBase64String(base64);
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException("Encrypted value is not valid Base64.", ex);
        }

        if (all.Length < NonceSize + TagSize)
            throw new InvalidOperationException("Encrypted payload is too short.");

        var nonce = new byte[NonceSize];
        var tag = new byte[TagSize];
        Buffer.BlockCopy(all, 0, nonce, 0, NonceSize);
        Buffer.BlockCopy(all, NonceSize, tag, 0, TagSize);

        var cipherLen = all.Length - (NonceSize + TagSize);
        var cipher = new byte[cipherLen];
        Buffer.BlockCopy(all, NonceSize + TagSize, cipher, 0, cipherLen);

        var plain = new byte[cipherLen];
        using var aes = new AesGcm(key);
        aes.Decrypt(nonce, cipher, tag, plain, null);

        return Encoding.UTF8.GetString(plain);
    }
}

using System.Text;
using KimlykNet.Contracts;
using KimlykNet.Data.Abstractions;

using Microsoft.Extensions.Options;

namespace KimlykNet.Services;

public class IdEncoder(IOptions<EncoderOptions> encoderOptions) : IIdEncoder
{
    public string Encode(string plainText)
    {
        using var aes = CreateAes();
        using var encryptor = aes.CreateEncryptor();
        var textBytes = Encoding.UTF8.GetBytes((plainText));
        var encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);

        string base64 = Convert.ToBase64String(encryptedBytes);
        return base64
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    public string Decode(string secretValue)
    {
        // 1. Convert URL-safe Base64 back to standard Base64
        string standardBase64 = secretValue
            .Replace('-', '+')
            .Replace('_', '/');

        // Add padding back if necessary (Base64 length must be a multiple of 4)
        while (standardBase64.Length % 4 != 0)
        {
            standardBase64 += '=';
        }

        using var aes = CreateAes();
        using var decryptor = aes.CreateDecryptor();
        var bytes = Convert.FromBase64String(standardBase64);
        return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(bytes, 0, bytes.Length));
    }

    private System.Security.Cryptography.Aes CreateAes()
    {
        byte[] key = encoderOptions.Value.Key;
        byte[] vector = encoderOptions.Value.IV;

        var aes = System.Security.Cryptography.Aes.Create();

        aes.Key = key;
        aes.IV = vector;

        return aes;
    }
}
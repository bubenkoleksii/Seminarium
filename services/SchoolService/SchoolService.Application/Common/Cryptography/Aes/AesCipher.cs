using AesAlgorithm = System.Security.Cryptography.Aes;

namespace SchoolService.Application.Common.Cryptography.Aes;

public class AesCipher : IAesCipher
{
    private readonly byte[] _encryptionKeyBytes;
    private readonly byte[] _initializationVectorBytes;

    public AesCipher(IConfiguration configuration)
    {
        var encryptionKeyBase64 = configuration["EncryptionSettings:EncryptionKeyBase64"]!;
        _encryptionKeyBytes = Convert.FromBase64String(encryptionKeyBase64);

        var initializationVectorBase64 = configuration["EncryptionSettings:InitializationVectorBase64"]!;
        _initializationVectorBytes = Convert.FromBase64String(initializationVectorBase64);
    }

    public string Encrypt(string plainText)
    {
        using var aes = AesAlgorithm.Create();
        aes.Key = _encryptionKeyBytes;
        aes.IV = _initializationVectorBytes;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);

        using var memoryStream = new MemoryStream();
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        }

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        var cipherBytes = Convert.FromBase64String(cipherText);

        using var aes = AesAlgorithm.Create();
        aes.Key = _encryptionKeyBytes;
        aes.IV = _initializationVectorBytes;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(cipherBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return streamReader.ReadToEnd();
    }
}

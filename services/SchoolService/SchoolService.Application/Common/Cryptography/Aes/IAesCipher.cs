namespace SchoolService.Application.Common.Cryptography.Aes;

public interface IAesCipher
{
    public string Encrypt(string plainText);

    public string Decrypt(string cipherText);
}

using Microsoft.Extensions.Configuration;
using Moq;
using SchoolService.Application.Common.Cryptography.Aes;

namespace SchoolService.Application.Tests;

[TestClass]
public class AesCipherTests
{
    private Mock<IConfiguration> _mockConfiguration;

    [TestInitialize]
    public void Setup()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.SetupGet(c => c["EncryptionSettings:EncryptionKeyBase64"]).Returns("M0/fZl2ZhG8ZpQSKzX9vRQ==");
        _mockConfiguration.SetupGet(c => c["EncryptionSettings:InitializationVectorBase64"]).Returns("hMjPllTyCdQyI0y7O1Ck1Q==");
    }

    [TestMethod]
    public void Encrypt_Decrypt_Success()
    {
        var plainText = "Hello, world!";
        var aesCipher = new AesCipher(_mockConfiguration.Object);

        var encryptedText = aesCipher.Encrypt(plainText);
        var decryptedText = aesCipher.Decrypt(encryptedText);

        Assert.IsNotNull(encryptedText);
        Assert.AreNotEqual(plainText, encryptedText);
        Assert.AreEqual(plainText, decryptedText);
    }

    [TestMethod]
    public void Encrypt_Decrypt_WithDifferentInstances_Success()
    {
        var plainText = "Hello, world!";
        var aesCipher1 = new AesCipher(_mockConfiguration.Object);
        var aesCipher2 = new AesCipher(_mockConfiguration.Object);

        var encryptedText1 = aesCipher1.Encrypt(plainText);
        var encryptedText2 = aesCipher2.Encrypt(plainText);

        var decryptedText1 = aesCipher1.Decrypt(encryptedText1);
        var decryptedText2 = aesCipher2.Decrypt(encryptedText2);

        Assert.AreEqual(encryptedText1, encryptedText2);
        Assert.AreEqual(decryptedText1, decryptedText2);
    }

    [TestMethod]
    public void Decrypt_WithInvalidBase64CipherText_ThrowsFormatException()
    {
        var invalidCipherText = "InvalidCipherText";
        var aesCipher = new AesCipher(_mockConfiguration.Object);

        Assert.ThrowsException<FormatException>(() => aesCipher.Decrypt(invalidCipherText));
    }

    [TestMethod]
    public void Encrypt_WithNullPlainText_ThrowsArgumentNullException()
    {
        // Arrange
        string nullPlainText = null;
        var aesCipher = new AesCipher(_mockConfiguration.Object);

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => aesCipher.Encrypt(nullPlainText));
    }

    [TestMethod]
    public void Decrypt_WithEmptyCipherText_ReturnsEmptyString()
    {
        // Arrange
        var emptyCipherText = string.Empty;
        var aesCipher = new AesCipher(_mockConfiguration.Object);

        // Act
        var decryptedText = aesCipher.Decrypt(emptyCipherText);

        // Assert
        Assert.AreEqual(string.Empty, decryptedText);
    }

    [TestMethod]
    public void Encrypt_Decrypt_WithLongPlainText_Success()
    {
        // Arrange
        var longPlainText = new string('a', 1000); // 1000 characters long
        var aesCipher = new AesCipher(_mockConfiguration.Object);

        // Act
        var encryptedText = aesCipher.Encrypt(longPlainText);
        var decryptedText = aesCipher.Decrypt(encryptedText);

        // Assert
        Assert.AreEqual(longPlainText, decryptedText);
    }

    [TestMethod]
    public void Encrypt_Decrypt_WithLargeCipherText_Success()
    {
        // Arrange
        var longPlainText = new string('a', 1000);
        var aesCipher = new AesCipher(_mockConfiguration.Object);

        // Act
        var encryptedText = aesCipher.Encrypt(longPlainText);
        var decryptedText = aesCipher.Decrypt(encryptedText);

        // Assert
        Assert.AreEqual(longPlainText, decryptedText);
    }
}

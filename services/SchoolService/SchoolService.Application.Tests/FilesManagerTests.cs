using LanguageExt;
using Microsoft.Extensions.Options;
using Moq;
using SchoolService.Application.Common.Files;
using Shared.Contracts.Errors;
using Shared.Contracts.Errors.Invalid;
using Shared.Contracts.Files;
using Shared.Contracts.Options;
using Shared.Utils.File;

namespace SchoolService.Application.Tests;

[TestClass]
public class FilesManagerTests
{
    private Mock<IS3Service> _mockS3Service;
    private Mock<IOptions<S3Options>> _mockS3Options;

    [TestInitialize]
    public void Setup()
    {
        _mockS3Service = new Mock<IS3Service>();
        _mockS3Options = new Mock<IOptions<S3Options>>();
        _mockS3Options.SetupGet(s => s.Value).Returns(new S3Options { Bucket = "test-bucket" });
    }

    [TestMethod]
    public async Task UploadFile_ValidStream_ReturnsFileSuccess()
    {
        // Arrange
        var stream = new MemoryStream();
        var fileName = "test-file.txt";
        var filesManager = new FilesManager(_mockS3Service.Object, _mockS3Options.Object);
        _mockS3Service.Setup(s => s.UploadOne(It.IsAny<SaveFileRequest>())).ReturnsAsync(Either<FileSuccess, Error>.Left(new FileSuccess("1.txt", "some")));

        // Act
        var result = await filesManager.UploadFile(stream, fileName, null);

        // Assert
        Assert.IsTrue(result.IsLeft);
    }

    [TestMethod]
    public async Task UploadFile_NullStream_ReturnsInvalidError()
    {
        // Arrange
        Stream stream = null;
        var fileName = "test-file.txt";
        var filesManager = new FilesManager(_mockS3Service.Object, _mockS3Options.Object);
        _mockS3Service.Setup(s => s.UploadOne(It.IsAny<SaveFileRequest>())).ReturnsAsync(Either<FileSuccess, Error>.Right(new InvalidError()));

        // Act
        var result = await filesManager.UploadFile(stream, fileName, null);

        // Assert
        Assert.IsTrue(result.IsRight);
    }

    [TestMethod]
    public async Task DeleteFileIfExists_ExistingName_ReturnsNone()
    {
        // Arrange
        var fileName = "existing-file.txt";
        _mockS3Service.Setup(s => s.DeleteOne(It.IsAny<DeleteFileRequest>())).ReturnsAsync(Option<Error>.None);
        var filesManager = new FilesManager(_mockS3Service.Object, _mockS3Options.Object);

        // Act
        var result = await filesManager.DeleteFileIfExists(fileName);

        // Assert
        Assert.IsFalse(result.IsSome);
    }

    [TestMethod]
    public async Task DeleteFileIfExists_NullName_ReturnsNone()
    {
        // Arrange
        string fileName = null;
        var filesManager = new FilesManager(_mockS3Service.Object, _mockS3Options.Object);

        // Act
        var result = await filesManager.DeleteFileIfExists(fileName);

        // Assert
        Assert.IsFalse(result.IsSome);
    }

    [TestMethod]
    public void GetFile_InvalidName_ReturnsInvalidError()
    {
        // Arrange
        var fileName = "invalid-file.txt";
        var filesManager = new FilesManager(_mockS3Service.Object, _mockS3Options.Object);
        _mockS3Service.Setup(s => s.GetOne(It.IsAny<GetFileRequest>())).Returns(Either<FileSuccess, Error>.Right(new InvalidError()));

        // Act
        var result = filesManager.GetFile(fileName);

        // Assert
        Assert.IsTrue(result.IsRight);
    }
}

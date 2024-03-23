namespace Shared.Contracts.File;

public record SaveFile(Stream Stream, string Name, string BucketName, Guid? ResourceId);
namespace Shared.Contracts.File;

public record SaveFileCommand(byte[] Stream, string Name, string BucketName, Guid ResourceId);

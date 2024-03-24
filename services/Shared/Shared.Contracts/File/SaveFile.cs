namespace Shared.Contracts.File;

public record SaveFile(byte[] Stream, string Name, string BucketName, Guid ResourceId);

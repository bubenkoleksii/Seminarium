namespace S3Service.Api.Contracts;

public record CreateFileContract(Stream Stream, string Name, Guid? ResourceId);

namespace Shared.Contracts.Files;

public record SaveFileRequest(Stream Stream, string Name, string Bucket);

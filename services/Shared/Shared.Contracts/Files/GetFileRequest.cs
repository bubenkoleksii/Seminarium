namespace Shared.Contracts.Files;

public record GetFileRequest(string Name, string Bucket, int UrlExpirationInMin = 100);

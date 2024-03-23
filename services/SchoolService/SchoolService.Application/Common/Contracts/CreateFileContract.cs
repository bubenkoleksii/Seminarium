namespace SchoolService.Application.Common.Contracts;

public record CreateFileContract(Stream Stream, string Name, Guid? ResourceId);

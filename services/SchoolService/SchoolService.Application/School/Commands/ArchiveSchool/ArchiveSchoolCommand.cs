namespace SchoolService.Application.School.Commands.ArchiveSchool;

public record ArchiveSchoolCommand(Guid Id) : IRequest<Option<Error>>;

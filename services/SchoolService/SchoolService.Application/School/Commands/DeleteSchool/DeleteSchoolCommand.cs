namespace SchoolService.Application.School.Commands.DeleteSchool;

public record DeleteSchoolCommand(Guid Id) : IRequest<Option<Error>>;

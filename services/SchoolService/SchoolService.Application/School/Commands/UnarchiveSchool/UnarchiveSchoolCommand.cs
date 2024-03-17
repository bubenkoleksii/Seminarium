namespace SchoolService.Application.School.Commands.UnarchiveSchool;

public record UnarchiveSchoolCommand(Guid Id) : IRequest<Option<Error>>;

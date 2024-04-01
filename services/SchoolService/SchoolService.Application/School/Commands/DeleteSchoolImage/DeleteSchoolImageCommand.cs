namespace SchoolService.Application.School.Commands.DeleteSchoolImage;

public record DeleteSchoolImageCommand(Guid SchoolId) : IRequest<Option<Error>>;

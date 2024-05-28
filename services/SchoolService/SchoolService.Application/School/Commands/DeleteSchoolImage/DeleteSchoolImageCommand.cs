namespace SchoolService.Application.School.Commands.DeleteSchoolImage;

public record DeleteSchoolImageCommand(
    Guid SchoolId,

    Guid? UserId
) : IRequest<Option<Error>>;

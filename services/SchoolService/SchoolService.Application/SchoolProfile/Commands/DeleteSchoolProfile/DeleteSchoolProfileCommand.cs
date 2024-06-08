namespace SchoolService.Application.SchoolProfile.Commands.DeleteSchoolProfile;

public record DeleteSchoolProfileCommand(
    Guid Id,

    Guid UserId
) : IRequest<Option<Error>>;

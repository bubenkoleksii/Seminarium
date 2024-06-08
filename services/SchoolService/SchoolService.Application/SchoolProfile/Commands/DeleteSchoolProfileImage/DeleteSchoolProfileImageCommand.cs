namespace SchoolService.Application.SchoolProfile.Commands.DeleteSchoolProfileImage;

public record DeleteSchoolProfileImageCommand(
    Guid SchoolProfileId,

    Guid UserId
) : IRequest<Option<Error>>;

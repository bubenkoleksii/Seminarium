namespace SchoolService.Application.School.Commands.CreateTeacherInvitation;

public record CreateTeacherInvitationCommand(
    Guid SchoolId,

    Guid? UserId
) : IRequest<Either<string, Error>>;

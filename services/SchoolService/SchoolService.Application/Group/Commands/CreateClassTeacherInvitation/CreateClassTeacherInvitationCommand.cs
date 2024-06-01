namespace SchoolService.Application.Group.Commands.CreateClassTeacherInvitation;

public record CreateClassTeacherInvitationCommand(
    Guid GroupId,

    Guid UserId
) : IRequest<Either<string, Error>>;

namespace SchoolService.Application.Group.Commands.CreateStudentInvitation;

public record CreateStudentInvitationCommand(
    Guid GroupId,

    Guid UserId
) : IRequest<Either<string, Error>>;

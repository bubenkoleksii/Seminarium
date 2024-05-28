namespace SchoolService.Application.School.Commands.CreateInvitation;

public record CreateInvitationCommand(
    Guid SchoolId,

    Guid? UserId
) : IRequest<Either<string, Error>>;

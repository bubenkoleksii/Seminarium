namespace SchoolService.Application.SchoolProfile.Commands.CreateParentInvitation;

public record CreateParentInvitationCommand(
    Guid ChildId,

    Guid UserId
) : IRequest<Either<string, Error>>;
